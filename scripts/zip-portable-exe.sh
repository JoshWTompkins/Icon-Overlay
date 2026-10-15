#!/usr/bin/env bash
set -euo pipefail

publish_dir="$1"
package_path="$2"


cd "$publish_dir"
rm *.pdb
zip -qr "$package_path" .