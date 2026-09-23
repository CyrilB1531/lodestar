"""No module-level literal in the oracle generator may read a name bound below it.

A constant defined *below* a module-level list or tuple that reads it is a `NameError`
at import, and nothing else in this repository catches it: `check_repeated_literals.py`
parses the file with `ast` rather than importing it, the suites read the committed
corpora, and the `Oracles are reproducible` job is the first thing that runs the
generator at all -- ten minutes into CI. #1123 shipped exactly that bug twice in one
branch, and the second time only because the first fix moved one constant and not the
other.

Static rather than an import: the generator pulls in rapidfuzz, numpy, jellyfish and
the rest at module level, so an importing test skips on every interpreter that does not
carry the oracle extras -- which is most of them, CI's lint job included. Reading the
module level in order needs none of them.

Function and class bodies are skipped: those run after the module is bound, so a
forward reference inside one is legal and routine.
"""

from __future__ import annotations

import ast
import builtins
import pathlib

TOOLS = pathlib.Path(__file__).resolve().parent.parent
GENERATOR = TOOLS / "generate_oracles.py"

BUILTINS = frozenset(dir(builtins))


def _bound_by(node: ast.stmt) -> set[str]:
    """Every name this module-level statement binds."""
    bound: set[str] = set()
    for child in ast.walk(node):
        if isinstance(child, (ast.Import, ast.ImportFrom)):
            bound.update(alias.asname or alias.name.split(".")[0] for alias in child.names)
        elif isinstance(child, (ast.FunctionDef, ast.AsyncFunctionDef, ast.ClassDef)):
            bound.add(child.name)
        elif isinstance(child, ast.Name) and isinstance(child.ctx, (ast.Store, ast.Del)):
            bound.add(child.id)
        elif isinstance(child, ast.ExceptHandler) and child.name:
            bound.add(child.name)
    return bound


def _read_at_module_level(node: ast.stmt) -> set[str]:
    """Every name this statement reads *before* the module finishes binding.

    A function or class body is not walked: it runs later, so a name it reads may
    legitimately be bound further down the file.
    """
    read: set[str] = set()
    stack: list[ast.AST] = [node]
    while stack:
        current = stack.pop()
        if isinstance(current, (ast.FunctionDef, ast.AsyncFunctionDef, ast.ClassDef)):
            # The decorators and the default arguments *are* evaluated now.
            stack.extend(current.decorator_list)
            if isinstance(current, ast.ClassDef):
                stack.extend(current.bases)
            else:
                stack.extend(d for d in current.args.defaults if d is not None)
            continue
        if isinstance(current, ast.Name) and isinstance(current.ctx, ast.Load):
            read.add(current.id)
        stack.extend(ast.iter_child_nodes(current))
    return read


def test_no_module_level_forward_reference() -> None:
    tree = ast.parse(GENERATOR.read_text(encoding="utf-8"))

    bound: set[str] = set(BUILTINS) | {"__name__", "__file__", "__doc__"}
    offenders: list[str] = []

    for statement in tree.body:
        for name in sorted(_read_at_module_level(statement) - bound):
            offenders.append(f"line {statement.lineno}: reads {name!r} before it is bound")
        bound |= _bound_by(statement)

    assert not offenders, (
        f"{GENERATOR.name} would raise NameError at import:\n  " + "\n  ".join(offenders))
