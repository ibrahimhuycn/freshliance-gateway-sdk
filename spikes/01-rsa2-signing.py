"""
Spike 1: RSA2 Signature — verify string-to-sign construction
=================================================================
The docs provide a complete signing example with known inputs.
We CAN'T verify the final signature (no private key), but we CAN:
  1. Verify the string-to-sign matches the docs exactly
  2. Generate our own key pair and test round-trip sign/verify
  3. Test the full pipeline

Edge cases tested:
  - ASCII ordering of keys
  - Empty values filtered out
  - sign field excluded from signing string
  - Compact JSON for bizContent (no spaces)
"""

import json
import base64
from collections import OrderedDict
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.primitives.asymmetric import rsa, padding
from cryptography.hazmat.backends import default_backend

# ═══════════════════════════════════════════════════════════════════
# STEP 1: Verify string-to-sign matches docs example
# ═══════════════════════════════════════════════════════════════════

def test_string_to_sign_matches_docs():
    """Verify our algorithm produces the exact signing string from the docs."""

    # From docs: the parameters BEFORE signing (sorted ASCII)
    params = OrderedDict()
    params["appId"] = "658409073956360262328652394"
    # bizContent must be compact JSON (no spaces!) for signing to match
    params["bizContent"] = '{"pageNum":1,"pageSize":10}'
    params["charset"] = "UTF-8"
    params["format"] = "JSON"
    params["method"] = "tracker.userDevice.page"
    params["signType"] = "RSA2"
    params["timestamp"] = "1747208216323"
    params["version"] = "1.0"

    # Docs expected signing string
    expected = (
        'appId=658409073956360262328652394'
        '&bizContent={"pageNum":1,"pageSize":10}'
        '&charset=UTF-8'
        '&format=JSON'
        '&method=tracker.userDevice.page'
        '&signType=RSA2'
        '&timestamp=1747208216323'
        '&version=1.0'
    )

    # Build the actual signing string
    actual = build_signing_string(params)

    assert actual == expected, f"\nEXPECTED: {expected}\nACTUAL:   {actual}"
    print("[OK] String-to-sign construction MATCHES docs example")
    return True


def test_string_to_sign_with_sign_field_filtered():
    """sign field must be excluded from the signing string."""

    params = OrderedDict()
    params["appId"] = "test"
    params["sign"] = "SHOULD_BE_IGNORED"
    params["timestamp"] = "123"
    params["version"] = "1.0"

    result = build_signing_string(params)
    assert "sign=" not in result, f"sign field leaked into signing string: {result}"
    print("[OK] sign field correctly excluded from signing string")
    return True


def test_string_to_sign_empty_values_filtered():
    """Empty values must be filtered out."""

    params = OrderedDict()
    params["appId"] = "test"
    params["bizContent"] = ""       # empty → should be removed
    params["timestamp"] = "123"
    params["emptyKey"] = None       # None → should be removed

    result = build_signing_string(params)
    assert "bizContent=" not in result, f"Empty bizContent should be filtered: {result}"
    assert "emptyKey=" not in result, f"None value should be filtered: {result}"
    print("[OK] Empty/null values correctly filtered from signing string")
    return True


def test_ascii_sorting():
    """ASCII ordering: uppercase before lowercase (A-Z then a-z)."""

    params = OrderedDict()
    params["zKey"] = "z"
    params["AKey"] = "a"
    params["aKey"] = "a"
    params["1Key"] = "1"

    result = build_signing_string(params)
    # ASCII: 1(49) < A(65) < a(97) < z(122)
    # So: 1Key, AKey, aKey, zKey
    expected = "1Key=1&AKey=a&aKey=a&zKey=z"
    assert result == expected, f"Sorting wrong:\nEXPECTED: {expected}\nACTUAL:   {result}"
    print("[OK] ASCII sorting correct: digits < uppercase < lowercase")
    return True


# ═══════════════════════════════════════════════════════════════════
# STEP 2: Generate key pair and test sign/verify round-trip
# ═══════════════════════════════════════════════════════════════════

def test_sign_and_verify_roundtrip():
    """Generate RSA key, sign a string, verify it."""

    # Generate a 2048-bit RSA key pair
    private_key = rsa.generate_private_key(
        public_exponent=65537,
        key_size=2048,
        backend=default_backend()
    )
    public_key = private_key.public_key()

    # Sign
    message = b"test signing string"
    signature = private_key.sign(
        message,
        padding.PKCS1v15(),
        hashes.SHA256()
    )
    signature_b64 = base64.b64encode(signature).decode()

    # Verify
    public_key.verify(
        base64.b64decode(signature_b64),
        message,
        padding.PKCS1v15(),
        hashes.SHA256()
    )
    print(f"[OK] SHA256WithRSA round-trip OK (sig length: {len(signature_b64)})")
    return True


# ═══════════════════════════════════════════════════════════════════
# STEP 3: Full pipeline — build, sign, format request
# ═══════════════════════════════════════════════════════════════════

def test_full_request_pipeline():
    """Simulate the complete request building flow."""

    # Generate keys
    private_key = rsa.generate_private_key(65537, 2048, default_backend())

    # PEM export (what you'd store in config)
    private_pem = private_key.private_bytes(
        encoding=serialization.Encoding.PEM,
        format=serialization.PrivateFormat.PKCS8,
        encryption_algorithm=serialization.NoEncryption()
    ).decode()

    # Build request
    app_id = "2025050100002694"
    method = "gw.userInfo.get"
    biz_content = None  # no bizContent needed

    request_dict = build_request(app_id, method, biz_content)
    signing_string = build_signing_string(request_dict)
    sign_b64 = rsa_sign(signing_string, private_key)

    # Add sign to request
    request_dict["sign"] = sign_b64

    # Serialize to JSON (what gets POSTed)
    body = json.dumps(request_dict, ensure_ascii=False, separators=(',', ':'))
    parsed = json.loads(body)

    assert parsed["appId"] == app_id
    assert parsed["method"] == method
    assert parsed["format"] == "JSON"
    assert parsed["charset"] == "UTF-8"
    assert parsed["signType"] == "RSA2"
    assert parsed["version"] == "1.0"
    assert parsed["sign"] == sign_b64
    assert "bizContent" not in parsed  # not included when None

    print(f"[OK] Full pipeline OK — request JSON: {body[:120]}...")

    # Also test with bizContent
    biz = {"pageNum": 1, "pageSize": 20}
    d2 = build_request(app_id, "gw.deviceInfo.page", biz)
    ss2 = build_signing_string(d2)
    sig2 = rsa_sign(ss2, private_key)
    d2["sign"] = sig2
    body2 = json.dumps(d2, ensure_ascii=False, separators=(',', ':'))
    print(f"[OK] Pipeline with bizContent OK: {body2[:160]}...")

    return True


# ═══════════════════════════════════════════════════════════════════
# Helper functions (these are the core SDK logic)
# ═══════════════════════════════════════════════════════════════════

def build_signing_string(params: dict) -> str:
    """
    Build the string to be signed following Freshliance API spec:
    1. Remove sign field and empty/null values
    2. Sort remaining keys by ASCII ordinal
    3. Concatenate as key=value&key=value...
    """
    # Filter
    filtered = {k: str(v) for k, v in params.items() 
                if k != "sign" and v is not None and str(v) != ""}

    # Sort by ASCII code of keys
    sorted_keys = sorted(filtered.keys())

    # Build
    parts = [f"{k}={filtered[k]}" for k in sorted_keys]
    return "&".join(parts)


def rsa_sign(message: str, private_key) -> str:
    """SHA256WithRSA sign and return Base64."""
    signature = private_key.sign(
        message.encode("utf-8"),
        padding.PKCS1v15(),
        hashes.SHA256()
    )
    return base64.b64encode(signature).decode()


def build_request(app_id: str, method: str, biz_content: any | None) -> dict:
    """
    Build the request parameter dictionary (before signing).
    bizContent is serialized as compact JSON string.
    """
    import time

    params = OrderedDict()
    params["appId"] = app_id
    params["method"] = method
    params["format"] = "JSON"
    params["charset"] = "UTF-8"
    params["signType"] = "RSA2"
    params["timestamp"] = str(int(time.time() * 1000))
    params["version"] = "1.0"

    if biz_content is not None:
        params["bizContent"] = json.dumps(biz_content, ensure_ascii=False, separators=(',', ':'))

    return params


# ═══════════════════════════════════════════════════════════════════
# Run all tests
# ═══════════════════════════════════════════════════════════════════

if __name__ == "__main__":
    print("=" * 60)
    print("SPIKE 1: RSA2 Signature Tests")
    print("=" * 60)

    try:
        test_string_to_sign_matches_docs()
        test_string_to_sign_with_sign_field_filtered()
        test_string_to_sign_empty_values_filtered()
        test_ascii_sorting()
        test_sign_and_verify_roundtrip()
        test_full_request_pipeline()
        print("\n" + "=" * 60)
        print("ALL SPIKE 1 TESTS PASSED")
        print("=" * 60)
    except Exception as e:
        print(f"\n[FAIL] TEST FAILED: {e}")
        import traceback
        traceback.print_exc()
