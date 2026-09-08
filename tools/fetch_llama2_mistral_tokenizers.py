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

Why Llama-2 is verified here and not written
--------------------------------------------
`meta-llama/Llama-2-7b-hf` is `license:llama2` -- the Llama 2 Community License,
bespoke and non-OSS -- and **both ungated mirrors declare no licence at all**, which
is not the same as declaring a permissive one. Decision 0003 names only permissive
sources, and the two model artifacts THIRD-PARTY-NOTICES.md carries are both MIT. So
this tool downloads the Llama-2 file, checks the two mirrors agree, and stops there:
the claim stays reproducible by anyone who runs it, without the repository
redistributing the vocabulary. Issue #552 holds the decision; flipping the `vendored`
flag below is the whole change if it goes the other way.

Mistral v0.1 is Apache-2.0 on the original and on its mirror, and is vendored.
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
        ("daryl149/llama-2-7b-chat-hf",
         "f9ffc4aede0845ab65324ce5dccb823dca2427f9a0710981e5bc2398d73d8162"),
        ("TheBloke/Llama-2-7B-fp16",
         "8eea70c4866c4f1320ba096fc986ac82038a8374dbe135212ba7628835b4a6f1"),
        True,
        False,   # verified, never written -- see the licence note below and issue #552
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


def main() -> int:
    check = "--check" in sys.argv[1:]
    failures: list[str] = []
    for local, (primary, corroborating, full, vendored) in MODELS.items():
        payloads = [download(repo, pinned) for repo, pinned in (primary, corroborating)]
        failures += [p for p in payloads if isinstance(p, str)]
        if any(isinstance(p, str) for p in payloads):
            continue

        parted = disagreements(payloads[0], payloads[1], full)
        if parted:
            failures.append(
                f"{primary[0]} and {corroborating[0]} disagree on {', '.join(parted)}; "
                "two sources standing in for one file have to agree on what it encodes to")
            continue

        if not vendored:
            print(f"{local}: {primary[0]} verified against {corroborating[0]}, not written "
                  "(licence undecided -- issue #552)")
            continue

        path = ORACLE_DIR / local
        if check:
            if not path.exists() or path.read_bytes() != payloads[0]:
                failures.append(f"{path} differs from the verified upstream file.")
        else:
            path.write_bytes(payloads[0])
            print(f"{local}: {len(payloads[0])} bytes from {primary[0]}, "
                  f"agreeing with {corroborating[0]} -> {path}")

    for failure in failures:
        print(failure, file=sys.stderr)
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main())
