"""What holds the pre-commit hook and CI together as guards are added.

The hook exists to run CI's offline guards earlier (decision 0037). Nothing in
git makes that true tomorrow: a fifth guard lands as a step in ci.yml, the hook
keeps running four, and the divergence is silent -- the hook still passes, the
push still fails, and the round trip the hook removed is back for one check.

So the relationship is asserted rather than remembered. CI is read as the source
of truth for which guards exist, the hook for which of them run before a commit,
and the difference between the two sets has to be exactly the exclusion decision
0037 wrote down. Adding a guard to CI without a decision about the hook fails
here, which is the point.

`OFFLINE_EXCLUSIONS` is the guards CI runs that the hook deliberately does not.
`check_nuspec_dependencies.py` reads the `.nuspec` files inside a packed
`./artifacts`, so running it before a commit would mean packing four projects
first -- named in decision 0037. `check_adr_immutable.py` needs `--base`, the
pull request's own base commit, which a commit made before a pull request
exists has none to name -- decision 0046, its own ADR rather than an edit to
0037, per the rule 0046 exists to enforce. `check_repeated_literals.py` takes
`--base` for that reason and one of its own: without a change to compare it
would print tools/ 's standing 108 findings on every commit, and a hook that
noisy is turned off -- decision 0064.

The floor guard needs no exclusion. CI passes it `--check-feed` and the hook does
not, but that is a flag rather than a guard, and its two offline rules run in
both places -- so the set stays about what cannot run offline at all.

Both files are read as text rather than parsed. ci.yml would need a YAML
dependency this repository does not have for its tool tests, and the hook is
shell; a regular expression over `tools/check_*.py` is what the two spellings
have in common, which is why the hook spells its guards as paths.

The prose that describes the loop is held to it here too (#612). Three sentences
say which guards run before a commit -- the hook's own opening comment, and two
paragraphs of CONTRIBUTING.md -- and all three had drifted before the thirteenth
guard was written: the comment said six of twelve, the list named seven of them,
and the exclusion paragraph said two when decisions 0046 and 0064 had made it
three. Every one of them was true when written, which is what makes a hand-written
list of this kind worth asserting rather than proof-reading: #586, #597 and #610
are the same bug in a workflow, a README and a release list.

So the loop is read as the source and the prose is held to it, the way the ADR
index prose is held to the directory in test_adr_count_coherence.py. The counts
are spelled out in words, which no grep for digits finds and which is the half a
contributor forgets -- the guard being added is next to the loop, the sentence is
in another file. Adding a guard to the hook now fails here until both texts say
so, which is the point.
"""
import re
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
HOOK = REPO / ".githooks" / "pre-commit"
CONTRIBUTING = REPO / "CONTRIBUTING.md"
WORKFLOWS = REPO / ".github" / "workflows"

OFFLINE_EXCLUSIONS = {
    "check_nuspec_dependencies", "check_adr_immutable", "check_repeated_literals"}

GUARD = re.compile(r"tools/(check_\w+)\.py")

# The hook names guards outside the loop as well -- the comment above it says which
# two are absent and why -- so what the loop runs is read from the loop alone.
LOOP = re.compile(r"^for guard in(.*?)^do$", re.MULTILINE | re.DOTALL)

# CONTRIBUTING.md's two sentences, each anchored on its own opening words and taken
# to the end of its paragraph. Rewording either is what the failure message asks for.
CONTRIBUTING_LIST = re.compile(
    r"^`\.githooks/pre-commit` then runs the (\w+) offline guards(.*?)(?=\n\n)",
    re.MULTILINE | re.DOTALL)
CONTRIBUTING_EXCLUSIONS = re.compile(
    r"^(\w+) guards CI runs stay out of it:(.*?)(?=\n\n)", re.MULTILINE | re.DOTALL)

# The prose spells its counts, so the comparison needs the words. Only the range a
# guard list can reach; test_adr_count_coherence.py has the parser that goes higher.
NUMBER_WORDS = {
    "one": 1, "two": 2, "three": 3, "four": 4, "five": 5, "six": 6, "seven": 7,
    "eight": 8, "nine": 9, "ten": 10, "eleven": 11, "twelve": 12, "thirteen": 13,
    "fourteen": 14, "fifteen": 15, "sixteen": 16, "seventeen": 17, "eighteen": 18,
    "nineteen": 19, "twenty": 20,
}
IN_WORDS = {value: word for word, value in NUMBER_WORDS.items()}

# CONTRIBUTING.md names its guards in backticks without the `tools/` the hook spells,
# which is presentation rather than drift: the sentence is prose, not a command.
PROSE_GUARD = re.compile(r"`(check_\w+)\.py`")


def guards_in(text):
    """Every `tools/check_*.py` a file invokes, as bare guard names."""
    return set(GUARD.findall(text))


def hook_text():
    return HOOK.read_text(encoding="utf-8")


def contributing_text():
    return CONTRIBUTING.read_text(encoding="utf-8")


def loop_guards():
    """The guards the hook's `for guard in ... do` loop runs, in run order."""
    loop = LOOP.search(hook_text())
    assert loop, (
        "no `for guard in ... do` loop in .githooks/pre-commit. The loop is the source "
        "every assertion in this file compares against, so rewriting it means rewriting "
        "LOOP here.")
    guards = GUARD.findall(loop.group(1))
    assert guards, "the hook's loop runs no `tools/check_*.py`, which cannot be right"
    return guards


def spelled(word, where):
    """`twelve` -> 12, for a count a sentence states in words."""
    assert word.lower() in NUMBER_WORDS, (
        f"{where} states its count as {word!r}, which is not a number word this file "
        f"knows. Spell it out, in the range one to {max(NUMBER_WORDS.values())}.")
    return NUMBER_WORDS[word.lower()]


def in_words(count):
    """12 -> `twelve`, so a failure message can quote the correction."""
    return IN_WORDS.get(count, str(count))


def contributing_section(pattern, where):
    match = pattern.search(contributing_text())
    assert match, (
        f"the {where} paragraph is gone from CONTRIBUTING.md, or its opening words have "
        "changed. It is what tells a contributor which guards run before a commit, so it "
        "is read here -- reword it and the pattern in this file moves with it.")
    return match


def test_the_hook_runs_every_offline_guard_ci_runs():
    in_ci = set()
    for workflow in WORKFLOWS.glob("*.yml"):
        in_ci |= guards_in(workflow.read_text(encoding="utf-8"))

    missing = in_ci - OFFLINE_EXCLUSIONS - guards_in(HOOK.read_text(encoding="utf-8"))

    assert not missing, (
        f"CI runs {sorted(missing)} and .githooks/pre-commit does not. Add the guard "
        "to the hook, or -- if it needs the network, a pack or a build -- to "
        "OFFLINE_EXCLUSIONS here and to decision 0037's list, with the reason."
    )


def test_the_hook_runs_no_guard_that_does_not_exist():
    # A renamed script leaves the hook failing every commit on an interpreter
    # error rather than on a finding.
    for guard in guards_in(HOOK.read_text(encoding="utf-8")):
        assert (REPO / "tools" / f"{guard}.py").is_file(), f"the hook runs a missing {guard}.py"


def test_the_excluded_guards_are_still_real():
    # An exclusion outliving the guard it excuses is how a list like this rots.
    for guard in OFFLINE_EXCLUSIONS:
        assert (REPO / "tools" / f"{guard}.py").is_file(), f"{guard}.py is gone; drop the exclusion"


def test_the_hook_names_neither_interpreter_unconditionally():
    # Hard-coding either name breaks commits on the platform shipping the other,
    # which is a fault the guards themselves do not have.
    text = HOOK.read_text(encoding="utf-8")
    assert "command -v python3" in text
    assert "command -v python " in text


def test_the_hook_is_checked_out_with_unix_line_endings():
    # `#!/bin/sh\r` is not a program any kernel finds, and Git for Windows
    # checks text out as CRLF unless .gitattributes says otherwise.
    assert b"\r\n" not in HOOK.read_bytes()
    attributes = (REPO / ".gitattributes").read_text(encoding="utf-8")
    assert ".githooks/** text eol=lf" in attributes


def test_the_hook_comment_states_the_number_of_guards_the_loop_runs():
    comment = re.search(r"^# The (\w+) offline guards,", hook_text(), re.MULTILINE)
    assert comment, (
        "the opening comment of .githooks/pre-commit no longer reads `# The <n> offline "
        "guards,`. It is the first thing a reader of the hook believes, so it states the "
        "count and this test holds it to the loop.")

    said = spelled(comment.group(1), ".githooks/pre-commit's opening comment")
    runs = len(loop_guards())
    assert said == runs, (
        f".githooks/pre-commit's opening comment says {said} offline guards and its loop "
        f"runs {runs}. Say `# The {in_words(runs)} offline guards,`.")


def test_contributing_names_every_guard_the_hook_runs_in_order():
    section = contributing_section(CONTRIBUTING_LIST, "`.githooks/pre-commit` then runs")
    named = PROSE_GUARD.findall(section.group(2))
    runs = loop_guards()

    assert named == runs, (
        f"CONTRIBUTING.md says the hook runs {named} and .githooks/pre-commit runs {runs}. "
        "The sentence names every guard the loop does, in the order the loop runs them; a "
        "guard added to the hook is added to that sentence in the same commit.")

    said = spelled(section.group(1), "CONTRIBUTING.md's hook sentence")
    assert said == len(runs), (
        f"CONTRIBUTING.md's hook sentence says {said} offline guards and names {len(runs)}. "
        f"Say `the {in_words(len(runs))} offline guards`.")


def test_contributing_names_every_guard_the_hook_excludes():
    section = contributing_section(CONTRIBUTING_EXCLUSIONS, "guards CI runs stay out of it")
    named = set(PROSE_GUARD.findall(section.group(2)))

    assert named == OFFLINE_EXCLUSIONS, (
        f"CONTRIBUTING.md says {sorted(named)} stay out of the hook and OFFLINE_EXCLUSIONS "
        f"here holds {sorted(OFFLINE_EXCLUSIONS)}. The two move together: an exclusion is a "
        "decision with an ADR, and the paragraph is where a contributor reads it. Guards "
        "the paragraph mentions for another reason belong in a paragraph of their own, "
        "which is where `check_version_floor.py --check-feed` went.")

    said = spelled(section.group(1), "CONTRIBUTING.md's exclusion paragraph")
    assert said == len(OFFLINE_EXCLUSIONS), (
        f"CONTRIBUTING.md's exclusion paragraph says {said} guards stay out of the hook and "
        f"{len(OFFLINE_EXCLUSIONS)} do. Say `{in_words(len(OFFLINE_EXCLUSIONS))} guards CI "
        "runs stay out of it:`.")
