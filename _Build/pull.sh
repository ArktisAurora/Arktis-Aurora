#!/usr/bin/env bash
# Fast-forwards the Arktis root and every repository one level below it from their remotes. Usage: pull.sh
root="$(cd "$(dirname "$0")/.." && pwd)"

repos="."
for d in "$root"/*/; do
	[ -e "$d.git" ] && repos="$repos $(basename "$d")"
done

failed=""
for r in $repos; do
	dir="$root/$r"
	echo "== $r ($(git -C "$dir" branch --show-current))"
	if [ "$(git -C "$dir" remote | wc -l)" -gt 1 ]; then
		git -C "$dir" fetch --all --prune || { failed="$failed $r"; continue; }
	fi
	git -C "$dir" pull --ff-only || failed="$failed $r"
done

if [ -n "$failed" ]; then
	echo "Failed:$failed"
	exit 1
fi
