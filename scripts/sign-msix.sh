#!/usr/bin/env bash

package_path="./publish/IconOverlay.msix"
certificate_path="./certificates/Incog.pfx"

read -rsp "Enter PFX password: " pfx_password
echo

echo "Signing MSIX package..."
psign-tool sign \
  --pfx "$certificate_path" \
  --password "$pfx_password" \
  --digest sha256 \
  "$package_path"

unset pfx_password
echo "Signed package created: $package_path"