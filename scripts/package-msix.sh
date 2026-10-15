#!/usr/bin/env bash
set -euo pipefail

echo "Running package-msix.sh"
echo "PATH=$PATH"
echo "makemsix: $(command -v makemsix || echo NOT FOUND)"
echo "psign-tool: $(command -v psign-tool || echo NOT FOUND)"

# Arguments: publish directory, output package, signing certificate
publish_dir="$1"
package_path="$2"

echo "Creating MSIX package..."
#makemsix pack -d "$publish_dir" -p "$package_path"


source "$(conda info --base)/etc/profile.d/conda.sh"
conda activate openappx

openappx pack --root "$publish_dir" --out "$package_path"
#openappx sign   --package ~/coding_projects/dotnet/IconOverlay/publish/IconOverlay.msix   --pfx ~/coding_projects/signing/Incog/IconOverlay/Incog.pfx

conda deactivate
