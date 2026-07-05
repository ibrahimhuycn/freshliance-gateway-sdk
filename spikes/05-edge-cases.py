"""
Spike 5: Signature edge cases
=================================================================
Test boundary conditions that could break the signing algorithm:
  - Unicode characters in parameter values
  - Nested JSON objects in bizContent
  - Arrays in bizContent
  - Very long strings
  - Special characters (&, =) in values
  - Numeric vs string bizContent serialization
  - Optional parameters omitted (null filter)
"""

import json
import base64
from collections import OrderedDict
from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.asymmetric import rsa, padding
from cryptography.hazmat.backends import default_backend


def build_signing_string(params: dict) -> str:
    filtered = {k: str(v) for k, v in params.items()
                if k != "sign" and v is not None and str(v) != ""}
    sorted_keys = sorted(filtered.keys())
    return "&".join(f"{k}={filtered[k]}" for k in sorted_keys)


def test_unicode_values():
    """Chinese characters, emoji, etc. in parameter values."""
    params = OrderedDict()
    params["appId"] = "test"
    params["method"] = "测试.方法"
    params["timestamp"] = "123"
    params["version"] = "1.0"
    params["bizContent"] = '{"name":"仓库1"}'

    result = build_signing_string(params)
    assert "测试.方法" in result, f"Unicode lost: {result}"
    assert "仓库1" in result, f"Chinese lost: {result}"
    print("[OK] Unicode values preserved in signing string")


def test_nested_json_biz_content():
    """bizContent with nested objects — must serialize to compact JSON."""
    biz = {
        "sensorConfig": {
            "startDelay": 0,
            "collectInterval": 1,
            "templateName": "Template 1"
        },
        "sensorAlarmList": [
            {"alarmZone": "L", "alarmProperty": 3, "alarmType": 1, "probeType": 0}
        ],
        "categoryId": 9,
        "productCode": "97"
    }

    params = OrderedDict()
    params["appId"] = "test"
    params["method"] = "gw.configTemplate.create"
    params["timestamp"] = "123"
    params["version"] = "1.0"
    params["bizContent"] = json.dumps(biz, ensure_ascii=False, separators=(',', ':'))

    result = build_signing_string(params)

    # Verify it's valid JSON
    parsed_biz = json.loads(params["bizContent"])
    assert parsed_biz["categoryId"] == 9
    assert len(parsed_biz["sensorAlarmList"]) == 1

    # Verify compact separators (values may contain spaces like "Template 1")
    compact = json.dumps(biz, ensure_ascii=False, separators=(',', ':'))
    assert ": " not in compact, f"Colon-space separator: {compact}"
    assert ", " not in compact, f"Comma-space separator: {compact}"
    print("[OK] Nested JSON bizContent serializes correctly (compact separators)")


def test_array_in_biz_content():
    """Arrays in bizContent — userDeviceIds in bindDevice."""
    biz = {"userDeviceIds": [4503, 4507, 4522], "groupId": 303}

    params = OrderedDict()
    params["appId"] = "test"
    params["method"] = "gw.groupDevice.bindDevice"
    params["timestamp"] = "123"
    params["version"] = "1.0"
    params["bizContent"] = json.dumps(biz, ensure_ascii=False, separators=(',', ':'))

    result = build_signing_string(params)
    # bizContent should contain the array
    assert "[4503,4507,4522]" in result, f"Array in bizContent lost: {result}"
    print("[OK] Arrays in bizContent serialized correctly")


def test_special_characters_in_values():
    """&, = in parameter values could break the parsing."""
    params = OrderedDict()
    params["appId"] = "app&id=test"    # contains &
    params["timestamp"] = "123"
    params["version"] = "1.0"

    result = build_signing_string(params)
    # The & in appId should still be there as its literal value
    assert "app&id=test" in result
    print("[OK] Special characters (&, =) preserved in values")


def test_numeric_types_in_biz_content():
    """Ints, floats, bools — must serialize correctly in JSON."""
    biz = {
        "pageNum": 1,          # int
        "pageSize": 20,        # int
        "temperature": 26.7,   # float
        "isActive": True,      # bool
        "value": None          # null (should serialize as null)
    }

    params = OrderedDict()
    params["appId"] = "test"
    params["timestamp"] = "123"
    params["version"] = "1.0"
    params["bizContent"] = json.dumps(biz, ensure_ascii=False, separators=(',', ':'))

    result = build_signing_string(params)

    # Check JSON format
    parsed = json.loads(params["bizContent"])
    assert parsed["pageNum"] == 1
    assert parsed["temperature"] == 26.7
    assert parsed["isActive"] is True
    assert parsed["value"] is None
    print("[OK] Numeric/boolean/null types serialize correctly in JSON")


def test_empty_biz_content():
    """When bizContent is empty or None, it should be excluded entirely."""
    # Case 1: None bizContent
    params = OrderedDict()
    params["appId"] = "test"
    params["timestamp"] = "123"
    params["version"] = "1.0"

    result = build_signing_string(params)
    assert "bizContent" not in result, f"None bizContent should be excluded: {result}"

    # Case 2: Empty string bizContent (if someone passes it)
    params["bizContent"] = ""
    result2 = build_signing_string(params)
    assert "bizContent" not in result2, f"Empty bizContent should be excluded: {result2}"

    print("[OK] Empty/None bizContent correctly excluded")


def test_long_timestamp():
    """Timestamp can be very long (19 digits)."""
    params = OrderedDict()
    params["appId"] = "test"
    params["timestamp"] = "1747208216323000000"  # microsecond timestamp
    params["version"] = "1.0"

    result = build_signing_string(params)
    assert "1747208216323000000" in result
    print("[OK] Long timestamps handled correctly")


def test_sign_roundtrip_with_edge_cases():
    """Full sign + verify with edge-case inputs."""
    private_key = rsa.generate_private_key(65537, 2048, default_backend())
    public_key = private_key.public_key()

    test_cases = [
        ("simple", "appId=test&timestamp=123&version=1.0"),
        ("unicode", "appId=test&bizContent={\"name\":\"仓库\"}&timestamp=123&version=1.0"),
        ("long", "appId=test&bizContent=" + json.dumps({"data": "x" * 1000}) + "&timestamp=123&version=1.0"),
    ]

    for name, msg in test_cases:
        sig = private_key.sign(msg.encode(), padding.PKCS1v15(), hashes.SHA256())
        sig_b64 = base64.b64encode(sig).decode()
        public_key.verify(base64.b64decode(sig_b64), msg.encode(), padding.PKCS1v15(), hashes.SHA256())
        print(f"[OK] Sign/verify roundtrip OK: {name}")


if __name__ == "__main__":
    print("=" * 60)
    print("SPIKE 5: Signature Edge Cases")
    print("=" * 60)

    try:
        test_unicode_values()
        test_nested_json_biz_content()
        test_array_in_biz_content()
        test_special_characters_in_values()
        test_numeric_types_in_biz_content()
        test_empty_biz_content()
        test_long_timestamp()
        test_sign_roundtrip_with_edge_cases()
        print("\n" + "=" * 60)
        print("ALL EDGE CASE TESTS PASSED")
        print("=" * 60)
    except Exception as e:
        print(f"\n[FAIL] FAILED: {e}")
        import traceback
        traceback.print_exc()
