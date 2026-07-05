# Device Group List

## Device Group Tree List

**Interface Description**

Obtain the device tree structure list based on the query conditions.

**Interface Method**

`gw.deviceGroup.treeList`

**Request Parameter**

None

**Return Data**

| Name                                  | Type          | Required | Description                                    | Example |
|---------------------------------------|---------------|----------|------------------------------------------------|---------|
| data                                  | array[object] | Yes      | Group information                               |         |
| data.groupId                          | int           | Yes      | Group ID                                        | 12      |
| data.parentId                         | int           | Yes      | Parent group id                                 | 0       |
| data.groupName                        | string        | Yes      | Group name                                      | group-1 |
| data.deviceGroupCount                 | object        | Yes      | Quantity of group devices in each status        |         |
| data.deviceGroupCount.deviceCount     | int           | Yes      | Quantity of sub-devices                         | 0       |
| data.deviceGroupCount.onlineCount     | int           | Yes      | Quantity of online devices                      | 0       |
| data.deviceGroupCount.offlineCount    | int           | Yes      | Quantity of offline devices                     | 0       |
| data.deviceGroupCount.abnormalCount   | int           | Yes      | Quantity of abnormal devices                    | 0       |
| data.deviceGroupCount.inactiveCount   | int           | Yes      | Quantity of unactivated devices                 | 0       |
| data.deviceGroupCount.alarmCount      | int           | Yes      | Quantity of alarm devices                       | 0       |
| data.subDeviceGroupList               | array[object] | No       | Sub-group                                       |         |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceGroup.treeList",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": [
    {
      "groupName": "Group 1",
      "groupId": 305,
      "parentId": 0,
      "deviceGroupCount": {
        "alarmCount": 0,
        "onlineCount": 0,
        "offlineCount": 0,
        "deviceCount": 0,
        "inactiveCount": 0,
        "abnormalCount": 0
      }
    },
    {
      "groupName": "Group 2",
      "groupId": 303,
      "subDeviceGroupList": [
        {
          "groupName": "Group 3",
          "groupId": 304,
          "parentId": 303,
          "deviceGroupCount": {
            "alarmCount": 0,
            "onlineCount": 0,
            "offlineCount": 0,
            "deviceCount": 0,
            "inactiveCount": 0,
            "abnormalCount": 0
          }
        }
      ],
      "parentId": 0,
      "deviceGroupCount": {
        "alarmCount": 0,
        "onlineCount": 0,
        "offlineCount": 0,
        "deviceCount": 0,
        "inactiveCount": 0,
        "abnormalCount": 0
      }
    }
  ],
  "msg": "success",
  "sign": "YFC+WKr/ykCpumHQVK4ekqFEi2BtOMKaUN2kzdBsjDuz24CrHtU03PVVDDph0mmFyJR4qMIr5g81/mskbMkp6Dlc/FV/GeOv5TQMjD04MId6yB5RVcbNNXp2MaQ5NkXqP0x4m9YbI6LrEMU4VTw3YPoLyE7OahzBxSaYcctjKMBUmSYc+Cy2Hsdo+ZM1qwwXqNAPWCChTtXwzHDYoQQYpfc/e081PoX28ShijZtCux8FM7M6pXXA4pOsjfiPqVeTs6V5YPKetoj0OIBHpqGKspUBA73ai7z0BRbbe6lXztZhFtsSMhgIikUfuxfREXenay/8lKonEySK0cocmngQTQ==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Obtain Sub-group List

**Interface Description**

Obtain sub-group list based on parent group ID.

**Interface Method**

`gw.deviceGroup.list`

**Request Parameter**

| Name     | Type   | Required | Description                                            | Example |
|----------|--------|----------|--------------------------------------------------------|---------|
| parentId | int    | Yes      | Parent group ID (For top group query, parent group ID is 0) | 0    |
| keyword  | string | No       | Keyword search                                         |         |

**Return Data**

| Name           | Type          | Required | Description                    | Example  |
|----------------|---------------|----------|--------------------------------|----------|
| data           | array[object] | Yes      | Group data                     |          |
| data.groupId   | int           | Yes      | Group ID                       | 223      |
| data.parentId  | int           | Yes      | Parent group ID                | 0        |
| data.groupName | string        | Yes      | Group name                     | group-1  |
| data.deviceCount | int         | Yes      | Quantity of group devices      | 0        |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceGroup.list",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "parentId": 0,
    "keyword": ""
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": [
    {
      "groupName": "Group 1",
      "deviceCount": 0,
      "groupId": 303,
      "parentId": 0
    },
    {
      "groupName": "Group 2",
      "deviceCount": 0,
      "groupId": 305,
      "parentId": 0
    }
  ],
  "msg": "success",
  "sign": "Mwhlp7DKAxD+sTet2WUun4/ndGqS2KjuHyST/zstMjjALVZqFaiskEUExlB8+sXOdvNwl2rDY3KZgRSswQ58wi/hz7zTKcqYCx6T2YmWjQFexne8V5M4T/5JOsMbdB7i5QYT8Wlcy41EGk3TpjDQdpo4YbA4xOi65NmSPWnKXAFy6ZUuPoljn1wB7W883XmIHM2gmmtcXbPJohNu7W1A8RZXOv5ZKc8DPM/yLOweXYcDa5xajkShIPhK78L5D0eYjR1vrC1JxwL7RlcksyiOqqyJyTuFs6TDZ/rPRbtKrN0wuwnun3tJBQy7bvak9PTgH1CGeMxZnXA+0Jl1GK7J3Q==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Create A New Group

**Interface Description**

Create a new device group.

**Interface Method**

`gw.deviceGroup.create`

**Request Parameter**

| Name      | Type   | Required | Description                                            | Example |
|-----------|--------|----------|--------------------------------------------------------|---------|
| parentId  | int    | Yes      | Parent group ID (For top group query, parent group ID is 0) | 0    |
| groupName | string | Yes      | Group name                                             | group-1 |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceGroup.create",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "parentId": 306,
    "groupName": "group-1"
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": true,
  "msg": "success",
  "sign": "eAnF+imkUl+PQSOKAT4d01WSpCg+qH2a/ai/QqDbzoGiMMAAduoXGj36aTbs0iM7WEvoCrqwrPSP7G2ASOEZVCWlCna0WK3OX15wj8JMJ1cd5USoyBo44IW0mTLHs5bTjy73yEqF/g50F1ZeJvBw+aESSji5S48oGvrH1wXaSfCTZQ5+ReL9ZHvzU2lWVSfpS9DCuQ7f6g3uYBNWKAVVeRSki95Awsv4Yiqi8SBOa2ERbXmwp6LGllBEpJNarBf5VAvHUGeDSWD2nQWPsyWceZaAfViQCYf7jdMZ8JmI6bQHrYuCnhHLAjaI84DozDGc3CadZHdgOjwsO57nz9WPeQ==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Modify Device Group

**Interface Description**

Modify device group name.

**Interface Method**

`gw.deviceGroup.update`

**Request Parameter**

| Name      | Type   | Required | Description     | Example  |
|-----------|--------|----------|-----------------|----------|
| groupId   | int    | Yes      | User group ID   | 203      |
| groupName | string | Yes      | Group name      | group-1  |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceGroup.update",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "groupId": 305,
    "groupName": "group-1"
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": true,
  "msg": "success",
  "sign": "eAnF+imkUl+PQSOKAT4d01WSpCg+qH2a/ai/QqDbzoGiMMAAduoXGj36aTbs0iM7WEvoCrqwrPSP7G2ASOEZVCWlCna0WK3OX15wj8JMJ1cd5USoyBo44IW0mTLHs5bTjy73yEqF/g50F1ZeJvBw+aESSji5S48oGvrH1wXaSfCTZQ5+ReL9ZHvzU2lWVSfpS9DCuQ7f6g3uYBNWKAVVeRSki95Awsv4Yiqi8SBOa2ERbXmwp6LGllBEpJNarBf5VAvHUGeDSWD2nQWPsyWceZaAfViQCYf7jdMZ8JmI6bQHrYuCnhHLAjaI84DozDGc3CadZHdgOjwsO57nz9WPeQ==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Delete Device Group

**Interface Description**

Delete device group.

**Interface Method**

`gw.deviceGroup.delete`

**Request Parameter**

| Name    | Type | Required | Description | Example |
|---------|------|----------|-------------|---------|
| groupId | int  | Yes      | Group id    | 203     |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceGroup.delete",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "groupId": 305
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": true,
  "msg": "success",
  "sign": "eAnF+imkUl+PQSOKAT4d01WSpCg+qH2a/ai/QqDbzoGiMMAAduoXGj36aTbs0iM7WEvoCrqwrPSP7G2ASOEZVCWlCna0WK3OX15wj8JMJ1cd5USoyBo44IW0mTLHs5bTjy73yEqF/g50F1ZeJvBw+aESSji5S48oGvrH1wXaSfCTZQ5+ReL9ZHvzU2lWVSfpS9DCuQ7f6g3uYBNWKAVVeRSki95Awsv4Yiqi8SBOa2ERbXmwp6LGllBEpJNarBf5VAvHUGeDSWD2nQWPsyWceZaAfViQCYf7jdMZ8JmI6bQHrYuCnhHLAjaI84DozDGc3CadZHdgOjwsO57nz9WPeQ==",
  "subCode": "",
  "subMsg": ""
}
```
