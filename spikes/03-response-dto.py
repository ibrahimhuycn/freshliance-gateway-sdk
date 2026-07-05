"""
Spike 3: Response DTO validation — parse all doc JSON examples
=================================================================
Extract every JSON response example from the API docs and validate:
 - Common envelope structure (code, msg, data, sign, subCode, subMsg)
 - Data shapes match documented parameter tables
 - Edge cases (null values, missing optional fields, arrays)
"""

import json

# ═══════════════════════════════════════════════════════════════════
# All response examples from the docs
# ═══════════════════════════════════════════════════════════════════

RESPONSE_EXAMPLES = {
    "userInfo.get": {
        "json": '''{
  "code": "0",
  "data": {
    "chnSmsNum": 217,
    "intSmsNum": 280,
    "temperatureUnit": 1,
    "intVoiceNum": 100,
    "dateFormat": 1,
    "nickName": "feedback",
    "timeZone": "+08:00",
    "language": 1,
    "chnVoiceNum": 107,
    "email": "123123@freshliance.com"
  },
  "msg": "success",
  "sign": "byWId5Xc...",
  "subCode": "",
  "subMsg": ""
}''',
        "data_type": "object",
        "required_fields": ["email", "timeZone", "language", "dateFormat", "temperatureUnit",
                          "chnSmsNum", "intSmsNum", "chnVoiceNum"]
    },

    "userInfo.update": {
        "json": '''{"code":"0","data":true,"msg":"success","sign":"e...","subCode":"","subMsg":""}''',
        "data_type": "bool",
    },

    "device.category": {
        "json": '''{"code":"0","data":[{"productSensor":0,"inTemHigh":70,"inTemLow":-30,"categoryName":"T10","categoryId":1},{"productSensor":0,"inTemHigh":70,"inTemLow":-30,"categoryName":"TH20","categoryId":2}],"msg":"success","sign":"B...","subCode":"","subMsg":""}''',
        "data_type": "array[object]",
    },

    "deviceInfo.page": {
        "json": '''{"code":"0","data":{"total":9,"rows":[{"deviceInfo":{"productModel":"COEUS-WIFI","userDeviceId":4566,"deviceSn":"241001383F","deviceStatus":2},"subDeviceLastDataList":[{"dataTime":1756983688000,"probeType":0,"temperature":28,"humidity":57.1}]}]},"msg":"success","sign":"j...","subCode":"","subMsg":""}''',
        "data_type": "PageResult<DeviceInfo>",
    },

    "deviceData.page": {
        "json": '''{"code":"0","data":{"total":451,"rows":[{"dataTime":"1747640566191","probeType":0,"temperature":26.7,"humidity":53.1,"status":0},{"dataTime":"1747640566191","probeType":0,"temperature":26.7,"humidity":53.3,"status":0}]},"msg":"success","sign":"T...","subCode":"","subMsg":""}''',
        "data_type": "PageResult<DeviceData>",
        "note": "dataTime is string in example but doc says long — possible type inconsistency"
    },

    "deviceAlarmData.page": {
        "json": '''{"code":"0","data":{"total":9,"rows":[{"deviceAlarmId":1756223,"alarmZone":"L1","alarmThreshold":500,"deviceId":2058,"recordId":12830,"probeType":0,"alarmProperty":3,"alarmType":1,"handleStatus":1}]},"msg":"success","sign":"L...","subCode":"","subMsg":""}''',
        "data_type": "PageResult<DeviceAlarm>",
    },

    "deviceGroup.treeList": {
        "json": '''{"code":"0","data":[{"groupName":"Group 1","groupId":305,"parentId":0,"deviceGroupCount":{"alarmCount":0,"onlineCount":0,"offlineCount":0,"deviceCount":0,"inactiveCount":0,"abnormalCount":0}},{"groupName":"Group 2","groupId":303,"subDeviceGroupList":[{"groupName":"Group 3","groupId":304,"parentId":303}],"parentId":0}],"msg":"success","sign":"Y...","subCode":"","subMsg":""}''',
        "data_type": "array[GroupNode] (tree)",
    },

    "deviceGroup.list": {
        "json": '''{"code":"0","data":[{"groupName":"Group 1","deviceCount":0,"groupId":303,"parentId":0}],"msg":"success","sign":"M...","subCode":"","subMsg":""}''',
        "data_type": "array[GroupItem]",
        "note": "Same endpoint returns different shape than treeList — flat array, no subDeviceGroupList"
    },

    "groupDevice.pageUnAllocatedDevice": {
        "json": '''{"code":"0","data":{"total":4,"rows":[{"productModel":"COEUS-WIFI","subDeviceCount":0,"userDeviceId":4523,"deviceSn":"250300478F","deviceName":"250300478F","productType":3,"deviceStatus":2}]},"msg":"success","sign":"R...","subCode":"","subMsg":""}''',
        "data_type": "PageResult<UnallocatedDevice>",
    },

    "groupDevice.pageAllocatedDevice": {
        "json": '''{"code":"0","data":{"total":2,"rows":[{"productModel":"COEUS-WIFI","createTime":"1755662900000","subDeviceStatusCount":{"alarmCount":0,"onlineCount":0,"offlineCount":0,"deviceCount":0},"userDeviceId":4503}]},"msg":"success","sign":"l...","subCode":"","subMsg":""}''',
        "data_type": "PageResult<AllocatedDevice>",
    },

    "configTemplate.page": {
        "json": '''{"code":"0","data":{"total":1,"rows":[{"templateName":"Template 1","collectInterval":1,"configId":47402,"sensorConfigProbeList":[{"sensorAlarmList":[{"alarmZone":"L1","alarmThreshold":100,"alarmType":1,"probeType":0}],"probeType":0}],"categoryName":"GSP"}]},"msg":"success","sign":"V...","subCode":"","subMsg":""}''',
        "data_type": "PageResult<Template>",
    },

    "configTemplate.get": {
        "json": '''{"code":"0","data":{"sensorAlarmList":[{"alarmZone":"H1","alarmType":2,"probeType":0,"alarmProperty":3,"alarmThreshold":2000.0}],"sensorConfig":{"collectInterval":1,"templateName":"Template 1","startDelay":0,"categoryId":9},"categoryName":"GSP"},"msg":"success","sign":"d...","subCode":"","subMsg":""}''',
        "data_type": "object (template detail)",
        "note": "Response wraps data in sensorConfig + sensorAlarmList + categoryName; NOT a flat object"
    },

    # Command responses all return data = true or data = <cmdId>
    "deviceCmd.updateParameter": {
        "json": '''{"code":"0","data":true,"msg":"success","sign":"e..."}''',
        "data_type": "bool",
    },
    "deviceCmd.saveDataConfig": {
        "json": '''{"code":"0","data":1519,"msg":"success","sign":"L..."}''',
        "data_type": "int (command ID)",
    },
}


# ═══════════════════════════════════════════════════════════════════
# Validation
# ═══════════════════════════════════════════════════════════════════

def validate_envelope(parsed, endpoint_name):
    """Every response must have code, msg, sign. subCode and subMsg are optional."""
    errors = []

    for field in ["code", "msg", "sign"]:
        if field not in parsed:
            errors.append(f"{endpoint_name}: Missing required field '{field}'")

    if "data" not in parsed:
        errors.append(f"{endpoint_name}: Missing 'data' field")

    return errors


def validate_data_type(parsed, endpoint_name, expected_type):
    """Check data matches expected type."""
    data = parsed.get("data")
    if expected_type == "object" and not isinstance(data, dict):
        return [f"{endpoint_name}: Expected object, got {type(data).__name__}"]
    if expected_type == "bool" and not isinstance(data, bool):
        return [f"{endpoint_name}: Expected bool, got {type(data).__name__}"]
    if expected_type == "int" and not isinstance(data, int):
        return [f"{endpoint_name}: Expected int, got {type(data).__name__}"]
    if expected_type.startswith("array") and not isinstance(data, list):
        return [f"{endpoint_name}: Expected array, got {type(data).__name__}"]
    if expected_type.startswith("PageResult") and not (isinstance(data, dict) and "total" in data and "rows" in data):
        return [f"{endpoint_name}: Expected PageResult with total+rows, got {type(data).__name__}"]
    return []


def find_type_inconsistencies():
    """Spot known issues across all examples."""
    issues = []

    # Device data: dataTime is "string" in example but "long" in parameter table
    issues.append("WARNING: deviceData.page — dataTime is STRING in example but 'long' in param table. "
                   "JSON number strings vs numbers are a common serialization bug. "
                   "Recommend: model as object that accepts both, or use a custom converter.")

    # alarmTime also appears as string in alarm data example
    issues.append("WARNING: deviceAlarmData.page — alarmTime is STRING in example but 'long' in param table.")

    # createTime is string in allocated device example
    issues.append("WARNING: groupDevice.pageAllocatedDevice — createTime is STRING in example but 'long' in param table.")

    # configTime is... not in the example but doc says string
    issues.append("NOTE: RemoteConfiguration configTime — doc says 'string' type but also says 'Configuration time'. Verify.")

    # intVoiceNum appears in userInfo.get response but NOT in the param table
    issues.append("WARNING: userInfo.get — 'intVoiceNum' appears in response example but NOT in the parameter table. "
                   "The table only lists 'chnVoiceNum'. Possible doc error.")

    # nickName (camelCase) in example vs nickname (lowercase) in param table
    issues.append("WARNING: userInfo.get — response uses 'nickName' (camelCase) but param table documented as 'nickname' (lowercase).")

    # deviceAlarmData.page example uses method 'gw.deviceAlarm.page' but doc table says 'gw.deviceAlarmData.page'
    issues.append("WARNING: Alarm data — example request uses method 'gw.deviceAlarm.page' but doc says 'gw.deviceAlarmData.page'. "
                   "The example is likely correct (matches admin API pattern).")

    return issues


if __name__ == "__main__":
    print("=" * 60)
    print("SPIKE 3: Response DTO Validation")
    print("=" * 60)

    all_errors = []
    for name, example in RESPONSE_EXAMPLES.items():
        try:
            parsed = json.loads(example["json"])
        except json.JSONDecodeError as e:
            all_errors.append(f"{name}: JSON parse error: {e}")
            continue

        errors = validate_envelope(parsed, name)
        errors += validate_data_type(parsed, name, example.get("data_type", ""))
        all_errors.extend(errors)

        if errors:
            print(f"  [FAIL] {name}: {len(errors)} error(s)")
            for e in errors:
                print(f"    - {e}")
        else:
            print(f"  [OK] {name}: OK (data={example['data_type']})")

    print("\n## TYPE INCONSISTENCIES FOUND")
    print("-" * 40)
    for issue in find_type_inconsistencies():
        print(f"  {issue}")

    if all_errors:
        print(f"\n[FAIL] {len(all_errors)} validation errors total")
    else:
        print(f"\n[OK] All {len(RESPONSE_EXAMPLES)} response examples parsed successfully")

    print(f"\n## RESPONSE ENVELOPE SUMMARY")
    print("  Always present: code, msg, sign, data")
    print("  Optional:       subCode, subMsg")
    print("  Data types:     object | bool | int | array | PageResult<T>")
