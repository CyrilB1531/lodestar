#!/usr/bin/env python3
"""Vendor the Llama-2 and Mistral v0.1 tokenizer.json files into tests/oracles/.

These are the two files #175 opens on: "files a user actually has, and neither
tokenizer here loads them". They are the SentencePiece-BPE lineage -- a Metaspace
whitespace escape plus byte_fallback -- and they write that pipeline two different
ways, which is why the corpus needs both rather than one file twice:

    Llama-2       normalizer Sequence[Prepend, Replace], pre_tokenizer null
    Mistral v0.1  normalizer null, pre_tokenizer Metaspace(split=false)

Only Mistral v0.1 is written into tests/oracles/, and only its vocabulary and merge
table -- never weights, per docs/decisions/0003-provenance-and-licensing.md. It is
Apache-2.0, and its attribution is recorded in THIRD-PARTY-NOTICES.md. Llama-2 is
downloaded and verified but **not** written; the note at the end of this docstring
says why.

    python tools/fetch_llama2_mistral_tokenizers.py           # vendor and verify
    python tools/fetch_llama2_mistral_tokenizers.py --check   # verify the fixtures

Every download is checked against the SHA-256 pinned below before anything is
written. A mismatch means the upstream file changed: read the diff, update the pin,
regenerate the oracles in the same commit, and expect ids to move.

Why two sources per model, and why they are held to different standards
----------------------------------------------------------------------
`meta-llama/Llama-2-7b-hf` is gated and returns HTTP 401 without credentials this
project does not have, so decision 0017 section 5's method applies: two independent
ungated mirrors, agreeing, stand in for reading the original. That agreement is
demonstrated here rather than asserted -- MIRROR_AGREEMENT below fails the fetch if
it ever stops holding.

The mirrors are `daryl149/llama-2-7b-chat-hf` and `TheBloke/Llama-2-7B-fp16`.
`NousResearch/Llama-2-7b-hf` is deliberately **not** one of them, and the reason is
worth recording because decision 0017 section 5 uses NousResearch for Llama-3, where
it is the right mirror. For Llama-2 it is not: its merge table holds the same 61 249
pairs as a multiset, but orders 119 of them differently, from index 61 129 onward and
all in the whitespace runs --

    idx 61129  NousResearch '▁ ▁'      daryl149 '▁▁ ▁▁'
    idx 61130  NousResearch '▁▁ ▁▁'  daryl149 '▁▁ ▁▁▁▁'

-- and merge order is rank in BPE, so that changes how runs of spaces segment.

`mistralai/Mistral-7B-v0.1` is **not** gated; it answers 200. So the original is read
directly and the two-mirror rule does not apply to it: standing in for an unreadable
original is the whole point of that rule. `TheBloke/Mistral-7B-v0.1-GPTQ` is kept as a
corroborating reading of the parts that decide encoding -- its own export is older and
spells the pipeline differently, so it is held to VOCABULARY_AGREEMENT rather than to
the full set.

Why Llama-2 is vendored, and from which mirror
----------------------------------------------
`meta-llama/Llama-2-7b-hf` is `license:llama2` -- the LLAMA 2 COMMUNITY LICENSE, which
is not one of the permissive licences decision 0003 names. Decision 0084 accepts it as
a named exception **for this artifact alone**, and the conditions it attaches are met
by the three files this tool also writes into docs/vendored/llama2/ and pins beside
the vocabulary.

`TheBloke/Llama-2-7B-fp16` is the primary rather than `daryl149/llama-2-7b-chat-hf`
because it ships `LICENSE`, `Notice` and `USE_POLICY.md` next to the artifact, so the
licence travels from the same place as the file. Its `LICENSE` is byte-identical to
`NousResearch/Llama-2-7b-hf`'s -- the two-source discipline applied to the licence text
as well as to the vocabulary. The two mirrors part on `post_processor`, which decision
0083 discards and which MIRROR_AGREEMENT therefore does not check.

Mistral v0.1 is Apache-2.0 on the original and on its mirror, and needs no exception.
"""


from __future__ import annotations

import hashlib
import json
import sys
import urllib.request
from pathlib import Path

ORACLE_DIR = Path(__file__).resolve().parent.parent / "tests" / "oracles"

# The sections that decide what a text encodes to; `post_processor` is not one --
# it differs between the Llama-2 mirrors and inserts tokens (decision 0083).
MIRROR_AGREEMENT = ("normalizer", "pre_tokenizer", "decoder", "added_tokens")
VOCABULARY_AGREEMENT = ("vocab", "merges")

# local name -> (primary (repo, sha256), corroborating (repo, sha256), full agreement?, vendored?)
MODELS = {
    "llama2_tokenizer.json": (
        ("TheBloke/Llama-2-7B-fp16",
         "8eea70c4866c4f1320ba096fc986ac82038a8374dbe135212ba7628835b4a6f1"),
        ("daryl149/llama-2-7b-chat-hf",
         "f9ffc4aede0845ab65324ce5dccb823dca2427f9a0710981e5bc2398d73d8162"),
        True,
        True,
    ),
    "mistral_v01_tokenizer.json": (
        ("mistralai/Mistral-7B-v0.1",
         "11c08db21487c885d8c792180f0be237f6a261b89a46f128a6a80a3aa4bd1720"),
        ("TheBloke/Mistral-7B-v0.1-GPTQ",
         "cdce92069938e6540fec77069211fa6ec7ea3ab5eb123c3aadad50a6f0c1e496"),
        False,
        True,
    ),
}


# The licence the vendored file ships under, pinned beside it. TheBloke is the
# primary source precisely because it carries these next to the artifact.
LICENCE_FILES = {
    "LICENSE": ("LICENSE",
                "8c17c2ebb0ea011be9981cc3922db8ca8fa61e828c5d3f44cb6ae342bf80460b"),
    "NOTICE": ("Notice",
               "62889ddbf7d51e8b94c8fcdf620577db870bcd25fddbf0da698734b500b614ea"),
    "USE_POLICY.txt": ("USE_POLICY.md",
                       "7c23fb36d80141c4ab8cdbb61ee4790102ebd2bf7aeff414453177d4f2110e5d"),
}
LICENCE_REPO = "TheBloke/Llama-2-7B-fp16"
LICENCE_DIR = Path(__file__).resolve().parent.parent / "docs" / "vendored" / "llama2"


def url(repo: str) -> str:
    return f"https://huggingface.co/{repo}/resolve/main/tokenizer.json"


def download(repo: str, pinned: str) -> bytes | str:
    """The verified bytes, or a one-line complaint naming both digests."""
    with urllib.request.urlopen(url(repo)) as response:  # noqa: S310
        payload = response.read()
    digest = hashlib.sha256(payload).hexdigest()
    if digest != pinned:
        return (f"{url(repo)}\n  expected sha256 {pinned}\n  got      sha256 {digest}")
    return payload


def disagreements(primary: bytes, other: bytes, full: bool) -> list[str]:
    """Every section the two readings do not share, named."""
    left, right = json.loads(primary), json.loads(other)
    found = [
        f"model.{key}" for key in VOCABULARY_AGREEMENT
        if left["model"].get(key) != right["model"].get(key)
    ]
    if full:
        found += [key for key in MIRROR_AGREEMENT if left.get(key) != right.get(key)]
    return found


def check_licence(check: bool) -> list[str]:
    """The Llama 2 Community License and its two companions, verified or written."""
    failures: list[str] = []
    for local, (remote, pinned) in LICENCE_FILES.items():
        with urllib.request.urlopen(  # noqa: S310
                f"https://huggingface.co/{LICENCE_REPO}/raw/main/{remote}") as response:
            payload = response.read()
        digest = hashlib.sha256(payload).hexdigest()
        if digest != pinned:
            failures.append(f"{LICENCE_REPO}/{remote}\n  expected sha256 {pinned}\n"
                            f"  got      sha256 {digest}")
            continue
        path = LICENCE_DIR / local
        failures += write_or_verify(
            path, payload, check,
            f"{local}: {len(payload)} bytes from {LICENCE_REPO} -> {path}")
    return failures


def write_or_verify(path: Path, payload: bytes, check: bool, said: str) -> list[str]:
    """Write the verified bytes, or check what is on disk already matches them."""
    if not check:
        path.write_bytes(payload)
        print(said)
        return []
    if not path.exists() or path.read_bytes() != payload:
        return [f"{path} differs from the verified upstream file."]
    return []


def vendor_model(local: str, entry: tuple, check: bool) -> list[str]:
    """One model: both sources fetched, held to their agreement, then written."""
    primary, corroborating, full, vendored = entry
    payloads = [download(repo, pinned) for repo, pinned in (primary, corroborating)]
    complaints = [p for p in payloads if isinstance(p, str)]
    if complaints:
        return complaints

    parted = disagreements(payloads[0], payloads[1], full)
    if parted:
        return [f"{primary[0]} and {corroborating[0]} disagree on {', '.join(parted)}; "
                "two sources standing in for one file have to agree on what it encodes to"]

    if not vendored:
        print(f"{local}: {primary[0]} verified against {corroborating[0]}, not written")
        return []

    path = ORACLE_DIR / local
    return write_or_verify(
        path, payloads[0], check,
        f"{local}: {len(payloads[0])} bytes from {primary[0]}, "
        f"agreeing with {corroborating[0]} -> {path}")


def main() -> int:
    check = "--check" in sys.argv[1:]
    failures = check_licence(check)
    for local, entry in MODELS.items():
        failures += vendor_model(local, entry, check)

    for failure in failures:
        print(failure, file=sys.stderr)
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main())
