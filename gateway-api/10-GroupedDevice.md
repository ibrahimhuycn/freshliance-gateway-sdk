# Grouped Device List

## Obtain the page of devices that can be added within the group

**Interface Description**

Obtain the page of devices that can be added within the group.

**Interface Method**

`gw.groupDevice.pageUnAllocatedDevice`

**Request Parameter**

| Name     | Type   | Required | Description                        | Example      |
|----------|--------|----------|------------------------------------|--------------|
| pageNum  | int    | Yes      | Page number, starting from 1       | 1            |
| pageSize | int    | Yes      | Data quantity each page, 1-50      | 10           |
| groupId  | int    | Yes      | Group ID                           | 125          |
| deviceSn | string | No       | Device ID                          | 250100000W   |

**Return Data**

| Name                  | Type          | Required | Description                                                               | Example     |
|-----------------------|---------------|----------|---------------------------------------------------------------------------|--------------|
| total                 | int           | Yes      | Total quantity                                                             | 1           |
| rows                  | array[object] | Yes      | Device list                                                                |              |
| rows.userDeviceId     | int           | Yes      | User device ID                                                             | 646          |
| rows.deviceSn         | string        | Yes      | Device ID                                                                  | 200700090W   |
| rows.parentId         | int           | Yes      | Parent device ID                                                           | 111          |
| rows.deviceName       | string        | Yes      | Device name                                                                | gateway      |
| rows.deviceCode       | string        | Yes      | Device MAC address                                                         | ADC14BB390   |
| rows.productType      | int           | Yes      | Product type: 1:Gateway, 2:Sensor, 3:COEUS, 4:GSP                          | 1            |
| rows.deviceStatus     | int           | Yes      | Device status: 0:Not activated, 1:Online, 2:Offline, 3:Abnormal            | 1            |
| rows.productModel     | string        | Yes      | Device model                                                               | G1000-S      |
| rows.subDeviceCount   | int           | Yes      | Sub-device quantity                                                        | 2            |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.groupDevice.pageUnAllocatedDevice",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "pageNum": 1,
    "pageSize": 10,
    "groupId": 303,
    "deviceSn": ""
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": {
    "total": 4,
    "rows": [
      {
        "productModel": "COEUS-WIFI",
        "subDeviceCount": 0,
        "userDeviceId": 4523,
        "deviceCode": "48E729577FC6",
        "deviceSn": "250300478F",
        "deviceName": "250300478F",
        "parentId": 2068,
        "productType": 3,
        "deviceStatus": 2
      },
      {
        "productModel": "G1000-S",
        "subDeviceCount": 0,
        "userDeviceId": 4525,
        "deviceCode": "D2B39B96A6D3",
        "deviceSn": "201200010W",
        "deviceName": "201200010W",
        "parentId": 0,
        "productType": 1,
        "deviceStatus": 1
      }
    ]
  },
  "msg": "success",
  "sign": "RMe2d3iqqpDQd85ECO+QyflQAk/YvEtZQqH2JVJUSegbTUI/Lv+U10s7hHYTPAn5mqhY19O6/b4R4d/L0cJjr5T15aVZ4Aywgey7LsVYzYPHMWFoLd+RlaA25wPJuGHaQ5UBNjvYHfWMlkJt2dDPcZSyGV8INKf4qeQYNe2TCA0R7UiG2OjQ2OFYEG+ZpafveC8dKj+j2Ccm6YsqHBGm7eMgAtWoSLLkFE05JLeCWCJYtEoNPqtOf9mAkNjAFG5hpQC5iHUxeQezNhdSQhFnVym/co+mMxkbZmCpIB7lEz2PMweSD6igdz7krdoY4SGNV4I4dEs+fibhmA0rQPXpmw==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Group Page for Assigned Devices

**Interface Description**

Obtain the page of the grouped devices based on the query conditions.

**Interface Method**

`gw.groupDevice.pageAllocatedDevice`

**Request Parameter**

| Name         | Type   | Required | Description                                                    | Example      |
|--------------|--------|----------|----------------------------------------------------------------|--------------|
| pageNum      | int    | Yes      | Page number, starting from 1                                   | 1            |
| pageSize     | int    | Yes      | Data quantity each page, 1-50                                  | 10           |
| groupId      | int    | Yes      | Group ID                                                       | 125          |
| deviceSn     | string | No       | Device ID                                                      | 250700097W   |
| deviceName   | string | No       | Device name                                                    | gateway      |
| deviceStatus | int    | No       | Device status: 0:Not activated, 1:Online, 2:Offline, 3:Abnormal | 1          |
| keyword      | string | No       | Fuzzy search                                                   | W            |

**Return Data**

| Name                                     | Type          | Required | Description                                                               | Example     |
|------------------------------------------|---------------|----------|---------------------------------------------------------------------------|--------------|
| total                                    | int           | Yes      | Total quantity                                                             | 1           |
| rows                                     | array[object] | Yes      | Data list                                                                  |              |
| rows.userDeviceId                        | int           | Yes      | User device ID                                                             | 646          |
| rows.deviceSn                            | string        | Yes      | Device ID                                                                  | 250000000W   |
| rows.deviceName                          | string        | Yes      | Device name                                                                | Warehouse 1  |
| rows.parentId                            | int           | Yes      | Parent device ID                                                           | 223          |
| rows.productType                         | int           | Yes      | Product type: 1:Gateway, 2:Sensor, 3:COEUS, 4:GSP                          | 1            |
| rows.productModel                        | string        | No       | Device model                                                               | G1000-S      |
| rows.deviceCode                          | string        | No       | Device MAC                                                                 |              |
| rows.deviceStatus                        | int           | Yes      | Device status: 0:Not activated, 1:Online, 2:Offline, 3:Abnormal            | 1            |
| rows.createTime                          | long          | Yes      | Addition time                                                              |              |
| rows.subDeviceCount                      | int           | Yes      | Sub-device quantity                                                        | 0            |
| rows.subDeviceStatusCount                | object        | Yes      | Quantity of the sub-devices at each status                                 |              |
| rows.subDeviceStatusCount.deviceCount    | int           | Yes      | Sub-device quantity                                                        | 0            |
| rows.subDeviceStatusCount.onlineCount    | int           | Yes      | Quantity of online devices in the group                                    | 0            |
| rows.subDeviceStatusCount.offlineCount   | int           | Yes      | Quantity of offline devices in the group                                   | 0            |
| rows.subDeviceStatusCount.abnormalCount  | int           | Yes      | Quantity of abnormal devices in the group                                  | 0            |
| rows.subDeviceStatusCount.inactiveCount  | int           | Yes      | Quantity of un-activated devices in the group                              | 0            |
| rows.subDeviceStatusCount.alarmCount     | int           | Yes      | Quantity of alarm devices in the group                                     | 0            |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.groupDevice.pageAllocatedDevice",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "pageNum": 1,
    "pageSize": 10,
    "groupId": 303,
    "deviceSn": "",
    "deviceName": "",
    "deviceStatus": 2,
    "keyword": ""
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": {
    "total": 2,
    "rows": [
      {
        "productModel": "COEUS-WIFI",
        "subDeviceCount": 0,
        "createTime": "1755662900000",
        "subDeviceStatusCount": {
          "alarmCount": 0,
          "onlineCount": 0,
          "offlineCount": 0,
          "deviceCount": 0,
          "inactiveCount": 0,
          "abnormalCount": 0
        },
        "userDeviceId": 4503,
        "deviceCode": "48E729573659",
        "deviceSn": "241001607F",
        "deviceName": "241001607F",
        "parentId": 2054,
        "productType": 3,
        "deviceStatus": 2
      },
      {
        "productModel": "COEUS-WIFI",
        "subDeviceCount": 0,
        "createTime": "1755662900000",
        "subDeviceStatusCount": {
          "alarmCount": 0,
          "onlineCount": 0,
          "offlineCount": 0,
          "deviceCount": 0,
          "inactiveCount": 0,
          "abnormalCount": 0
        },
        "userDeviceId": 4522,
        "deviceCode": "48E729574E22",
        "deviceSn": "250300479F",
        "deviceName": "250300479F",
        "parentId": 2069,
        "productType": 3,
        "deviceStatus": 2
      }
    ]
  },
  "msg": "success",
  "sign": "lgVgkDnJ1Lc99fBdka4Ae3XqNM2DBmhaMWqWjBtrjQd8APRb1M1dIbPDacEKblE1b4FCezpniXDpZXaw4rNo6wfBaCdf6RF1mCKBqzQ6zHC2MKoKsb6aaSD/5RUNquC65ZBI7I7PmSOntQxUNROzOy/Rf0LSl/VTJeY7P8pGtFiZfha+bJMIl0/LV+Ada4+Bt0yse8SZPD1opSH2T53/z2sJRgI0Y3mLx/Vjpk5kIJt2pPvYIlmpJ69qzzYeUngzaPoNxMyvNWsNh85QMcYiQ33BsambMfsfSxKqlq7noMPG1xFHt7xZUq1Zo+UYQmqmfMyV28t5RPeTf9kzeH7twA==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Remove the Device from the Group

**Interface Description**

Remove the device from the group.

**Interface Method**

`gw.groupDevice.unbindDevice`

**Request Parameter**

| Name         | Type | Required | Description     | Example |
|--------------|------|----------|-----------------|---------|
| userDeviceId | int  | Yes      | User device ID  | 635     |
| groupId      | int  | Yes      | Group ID        | 186     |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.groupDevice.unbindDevice",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1747645671183",
  "version": "1.0",
  "bizContent": {
    "userDeviceId": 645,
    "groupId": 186
  },
  "sign": "Psbn5gxDHI2mmJEezUmL0RnEfT3ty0ui9fQNm1wLV9CJbBr8QnExZw4rETK1aCOn6jSppDEwhbhKlpYa1dNZaMdALLvonwAIgUSMCt1LC0lFx+OAVjqQJ70wCNt4cUzcltaVAisSL7TZaabBgztSjDGgKPEsVKjEWOyDDbh7wZUWApRKYudErkhuOZhu4qLXlp7ZamtzSQzA2xc3xPtDTPUD1KqgpbnEosS2CtL+NewA4eeLCP/CXntIuUUie81wYl4Llvo5Er0i+1imFxMcYQnX5sOvOKIE635zRB9BoniUaWjqysHNDhTH0pwcBbv7A1/W7KximGwEEXkEHEGyDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": true,
  "msg": "success",
  "sign": "JTbz7QTYiTDKkLpCipwMmx2GK2TGM7YCVg1rtq+btve1zi/tv2qkAQ54xPmzpu3IMr0DiZ++7QT62Kpf7lmW4pePxjt9nEvM/rFy7+/OX0qKTvzoWPYup+DcMnZ1FNcseS6H6BTx83BGKp406HWBT0K1i/0v3hJTphWKnzrDAAZfgCKf5ieu2DrwGHLeRnR99Snk6ocmiE9t3fJN1u11W8+KCHQDQbL7x4NsRae6VsMP63v4lDbVtTcu2wpLOExwwzqFk4IVoJbEUU0VnyhzVoOrGzOxJpG7huoYOkyn9c740/5f7LD2Zk/8RCRPHHl29g1QmOsqk4JkKiO3M0ECIw==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Move the Device into the Group

**Interface Description**

Move the device into the group.

**Interface Method**

`gw.groupDevice.bindDevice`

**Request Parameter**

| Name         | Type        | Required | Description     | Example        |
|--------------|-------------|----------|-----------------|----------------|
| userDeviceIds| array[int]  | Yes      | User device IDs | [4503,4507,4522] |
| groupId      | int         | Yes      | User group ID   | 186            |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.groupDevice.bindDevice",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "userDeviceIds": [4503, 4507, 4522],
    "groupId": 303
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
