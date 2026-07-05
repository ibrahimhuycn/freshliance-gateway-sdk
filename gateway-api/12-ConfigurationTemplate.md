# Sensor Template

## Obtain Sensor Template Page

**Interface Description**

Obtain the user device configuration template page based on the query conditions.

**Interface Method**

`gw.configTemplate.page`

**Request Parameter**

| Name         | Type   | Required | Description                     | Example   |
|--------------|--------|----------|---------------------------------|-----------|
| pageNum      | int    | Yes      | Page number, starting from 1    | 1         |
| pageSize     | int    | Yes      | Page size, Max. 50              | 10        |
| templateName | string | No       | Template name                   | template  |

**Return Data**

| Name                                                          | Type          | Required | Description                                                       | Example     |
|---------------------------------------------------------------|---------------|----------|-------------------------------------------------------------------|-------------|
| total                                                         | int           | Yes      | Total quantity                                                     | 4           |
| rows                                                          | array[object] | Yes      | Data list                                                          |             |
| rows.configId                                                 | int           | Yes      | Configuration ID                                                   | 47402       |
| rows.startDelay                                               | int           | Yes      | Start delay                                                        | 0           |
| rows.collectInterval                                          | int           | Yes      | Logging interval                                                   | 1           |
| rows.templateName                                             | string        | Yes      | Template name                                                      | Template 1  |
| rows.categoryName                                             | string        | Yes      | Category name                                                      | GSP         |
| rows.categoryId                                               | int           | Yes      | Category id                                                        | 9           |
| rows.sensorConfigProbeList                                    | array[object] | Yes      | Probe information                                                  |             |
| rows.sensorConfigProbeList.configId                           | int           | Yes      | Configuration ID                                                   |             |
| rows.sensorConfigProbeList.probeType                          | int           | Yes      | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2     | 0           |
| rows.sensorConfigProbeList.temHigh                            | double        | No       | Min. value of high temperature alarm                               | 10.0        |
| rows.sensorConfigProbeList.temLow                             | double        | No       | Max. value of low temperature alarm                                | 10.0        |
| rows.sensorConfigProbeList.humHigh                            | double        | No       | Min. value of high humidity alarm                                  | 10.0        |
| rows.sensorConfigProbeList.humLow                             | double        | No       | Max. value of low humidity alarm                                   | 10.0        |
| rows.sensorConfigProbeList.lightHigh                          | double        | No       | Min. value of high illumination alarm                              | 10.0        |
| rows.sensorConfigProbeList.lightLow                           | double        | No       | Max. value of low illumination alarm                               | 10.0        |
| rows.sensorConfigProbeList.co2High                            | double        | No       | Min. value of high CO2 alarm                                       | 10.0        |
| rows.sensorConfigProbeList.co2Low                             | double        | No       | Max. value of low CO2 alarm                                        | 10.0        |
| rows.sensorConfigProbeList.sensorAlarmList                    | array[object] | No       | Alarm point information                                            |             |
| rows.sensorConfigProbeList.sensorAlarmList.alarmId            | int           | No       | Sensor alarm ID                                                    | 100         |
| rows.sensorConfigProbeList.sensorAlarmList.probeType          | int           | No       | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2     | 0           |
| rows.sensorConfigProbeList.sensorAlarmList.alarmZone          | string        | No       | Alarm zone: High alarm:H, Low alarm:L                              | H           |
| rows.sensorConfigProbeList.sensorAlarmList.alarmProperty      | int           | No       | Alarm property: 1:Temperature, 2:Humidity, 3:Illumination, 4:CO2   | 1           |
| rows.sensorConfigProbeList.sensorAlarmList.alarmType          | int           | No       | Alarm type: 1:Low alarm, 2:High alarm                              | 1           |
| rows.sensorConfigProbeList.sensorAlarmList.alarmWay           | int           | No       | Alarm way: 1:Single, 2:Cumulative                                  | 1           |
| rows.sensorConfigProbeList.sensorAlarmList.alarmDelay         | int           | No       | Alarm delay                                                        |             |
| rows.sensorConfigProbeList.sensorAlarmList.alarmThreshold     | double        | No       | Alarm threshold                                                    |             |
| rows.sensorConfigProbeList.sensorAlarmList.sort               | int           | No       | Alarm point list (start from 1)                                    |             |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.configTemplate.page",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "templateName": "Template 1",
    "pageNum": 1,
    "pageSize": 10
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": {
    "total": 1,
    "rows": [
      {
        "createTime": "1755662900000",
        "templateName": "Template 1",
        "collectInterval": 1,
        "configId": 47402,
        "sensorConfigProbeList": [
          {
            "sensorAlarmList": [
              {
                "alarmZone": "L1",
                "alarmThreshold": 100,
                "sort": 1,
                "alarmDelay": 0,
                "alarmWay": 1,
                "alarmType": 1,
                "probeType": 0,
                "configId": 47402,
                "alarmProperty": 3,
                "alarmId": 13384
              }
            ],
            "probeType": 0,
            "configId": 47402
          }
        ],
        "startDelay": 0,
        "categoryName": "GSP",
        "categoryId": 9
      }
    ]
  },
  "msg": "success",
  "sign": "V+6cn7jXzLxiB14jub3i3r8+GfJHU28KDeVK3CHDDWHY/HskvomktMIfPCKqFRSU/D8DgqGLYlGtusAfFNcKgQsMB/vQkiozqN4BlKSz6vwRX19hquk+exaQ/Mb4Dsah20A0jzbsLOS47zEiDlw2vZ6GxGquomwNn7ogdob30vA39+hJrj3zeP/D5ZkaCLrQP21hDiJW1aSDHspncjlfMY1JujYwWAJuLkg/Bh0beSAjLbybJIP+Hxzn3bulThkBvAwY7u3zQTjSUAFA4+7U2YaeE+YUm9hGOEiUq8HwC0m9r3Igkfv22lZRvyOwSqpBsMuUCXGuPEUwSf/A0FXJsA==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Obtain Sensor Template Details

**Interface Description**

Obtain sensor template details based on template configuration ID.

**Interface Method**

`gw.configTemplate.get`

**Request Parameter**

| Name     | Type | Required | Description              | Example |
|----------|------|----------|--------------------------|---------|
| configId | long | Yes      | Template configuration id | 2235    |

**Return Data**

| Name                               | Type          | Required | Description                                                         | Example                   |
|------------------------------------|---------------|----------|---------------------------------------------------------------------|---------------------------|
| sensorConfig                       | object        | Yes      | Sensor configuration information                                    |                           |
| sensorConfig.startDelay            | int           | Yes      | Start delay                                                         | 0                         |
| sensorConfig.collectInterval       | int           | Yes      | Logging interval                                                    | 0                         |
| sensorConfig.categoryId            | int           | Yes      | Category ID                                                         | 1                         |
| sensorConfig.templateName          | string        | Yes      | Template name                                                       | Configuration of Warehouse 1 |
| sensorAlarmList                    | array[object] | No       | Alarm point information                                             |                           |
| sensorAlarmList.alarmZone          | string        | No       | Alarm zone: High alarm:H, Low alarm:L                                | H                         |
| sensorAlarmList.alarmProperty      | int           | No       | Alarm property: 1:Temperature, 2:Humidity, 3:Illumination, 4:CO2     | 1                         |
| sensorAlarmList.alarmType          | int           | No       | Alarm type: 1:Low alarm, 2:High alarm                                | 1                         |
| sensorAlarmList.alarmWay           | int           | No       | Alarm way: 1:Single, 2:Cumulative                                    | 1                         |
| sensorAlarmList.probeType          | int           | No       | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2       | 0                         |
| sensorAlarmList.alarmDelay         | int           | No       | Alarm delay                                                         | 1                         |
| sensorAlarmList.alarmThreshold     | double        | No       | Alarm threshold                                                     | 20.0                      |
| categoryName                       | string        | Yes      | Category name                                                       | GSP                       |
| categoryId                         | int           | Yes      | Category id                                                         | 9                         |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.configTemplate.get",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "configId": 47360
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": {
    "sensorAlarmList": [
      {
        "alarmZone": "H1",
        "alarmType": 2,
        "probeType": 0,
        "alarmProperty": 3,
        "alarmThreshold": 2000.0,
        "alarmDelay": 0,
        "alarmWay": 1
      },
      {
        "alarmZone": "L1",
        "alarmType": 1,
        "probeType": 0,
        "alarmProperty": 3,
        "alarmThreshold": 1000.0,
        "alarmDelay": 0,
        "alarmWay": 1
      }
    ],
    "sensorConfig": {
      "collectInterval": 1,
      "templateName": "Template 1",
      "startDelay": 0,
      "categoryId": 9
    },
    "categoryName": "GSP",
    "categoryId": 9
  },
  "msg": "success",
  "sign": "dsyHf2tscg4zYgbs1UoO0DqawLhWI4dpWF+JUE1qZ6PTq02+z+jpN7wf/IvxT62ZkucoqDCMe9ArjoH6GoD1bh+tZO4JlxidBJfcF419HRzI1wt9EVjR7k5CaI2s1wwQHgnOi65kXFKm+jcYGD6TMxX17sc80VFONfVv++A0+bKDrECBGty0AnW1do3G4ozG94TQX2k73K2f50XvGXCWwjR5E8LKjr4aQj1bhjAEfcxD8EOPPoT9A6CCRAXdb08qkmzWG5stTLl1/ndXCu40QraDc81ilOmoMMfAzBpU71S0aud4mQqX1yPnxVG5xy9n5r5wbJypBvPb3+Ter9Hh4g==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Create New Configuration Template

**Interface Description**

Create a new template for gateway or sensor configuration.

**Interface Method**

`gw.configTemplate.create`

**Request Parameter**

| Name                               | Type          | Required | Description                                                         | Example          |
|------------------------------------|---------------|----------|---------------------------------------------------------------------|------------------|
| sensorConfig                       | object        | Yes      | Sensor configuration information                                    |                  |
| sensorConfig.startDelay            | int           | No       | Start delay                                                         | 30               |
| sensorConfig.collectInterval       | int           | Yes      | Logging interval                                                    | 30               |
| sensorConfig.categoryId            | int           | Yes      | Device category ID                                                  | 1                |
| sensorConfig.templateName          | string        | Yes      | Template name                                                       | Warehouse device |
| sensorAlarmList                    | array[object] | No       | Alarm point information                                             |                  |
| sensorAlarmList.alarmZone          | string        | No       | Alarm zone: High alarm:H, Low alarm:L                                | H                |
| sensorAlarmList.alarmProperty      | int           | No       | Alarm property: 1:Temperature, 2:Humidity, 3:Illumination, 4:CO2     | 1                |
| sensorAlarmList.alarmType          | int           | No       | Alarm type: 1:Low alarm, 2:High alarm                                | 1                |
| sensorAlarmList.probeType          | int           | No       | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2       | 1                |
| sensorAlarmList.alarmWay           | int           | No       | Alarm way: 1:Single, 2:Cumulative                                    | 1                |
| sensorAlarmList.alarmDelay         | int           | No       | Alarm delay                                                         | 30               |
| sensorAlarmList.alarmThreshold     | double        | No       | Alarm threshold                                                     | 30.0             |
| categoryId                         | int           | Yes      | Category ID                                                         | 1                |
| productCode                        | string        | Yes      | Product model code                                                  | A5               |

**Return Data**

| Name | Type    | Required | Description                  | Example |
|------|---------|----------|------------------------------|---------|
| data | boolean | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.configTemplate.create",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "sensorConfig": {
      "startDelay": 0,
      "collectInterval": 1,
      "templateName": "Template 1"
    },
    "sensorAlarmList": [
      { "alarmZone": "L", "alarmProperty": 3, "alarmType": 1, "probeType": 0, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": 100 },
      { "alarmZone": "L", "alarmProperty": 1, "alarmType": 1, "probeType": 1, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": 190 },
      { "alarmZone": "L", "alarmProperty": 1, "alarmType": 1, "probeType": 2, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": 50 }
    ],
    "productCode": "97",
    "categoryId": 9
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

## Modify Configuration Template

**Interface Description**

Modify the configuration information in the template based on configuration ID.

**Interface Method**

`gw.configTemplate.update`

**Request Parameter**

| Name                               | Type          | Required | Description                                                         | Example     |
|------------------------------------|---------------|----------|---------------------------------------------------------------------|-------------|
| sensorConfig                       | object        |          | Configuration template information                                  |             |
| sensorConfig.configId              | int           | Yes      | Configuration ID                                                    | 1048        |
| sensorConfig.startDelay            | int           | No       | Start delay                                                         | 10          |
| sensorConfig.collectInterval       | int           | Yes      | Logging interval                                                    | 10          |
| sensorConfig.templateName          | string        | Yes      | Template name                                                       | template-1  |
| sensorAlarmList                    | array[object] | No       | Alarm point information                                             |             |
| sensorAlarmList.alarmZone          | string        | No       | Alarm zone: High alarm:H, Low alarm:L                                | H           |
| sensorAlarmList.alarmProperty      | int           | No       | Alarm property: 1:Temperature, 2:Humidity, 3:Illumination, 4:CO2     | 1           |
| sensorAlarmList.alarmType          | int           | No       | Alarm type: 1:Low alarm, 2:High alarm                                | 1           |
| sensorAlarmList.probeType          | int           | No       | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2       | 0           |
| sensorAlarmList.alarmWay           | int           | No       | Alarm way: 1:Single, 2:Cumulative                                    | 1           |
| sensorAlarmList.alarmDelay         | int           | No       | Alarm delay                                                         | 0           |
| sensorAlarmList.alarmThreshold     | double        | No       | Alarm threshold                                                     | 30.0        |
| configId                           | int           | Yes      | Configuration ID                                                    | 1345        |

**Return Data**

| Name | Type    | Required | Description                  | Example |
|------|---------|----------|------------------------------|---------|
| data | boolean | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.configTemplate.update",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "sensorConfig": {
      "startDelay": 0,
      "collectInterval": 1,
      "templateName": "Template 1"
    },
    "sensorAlarmList": [
      { "alarmZone": "L", "alarmProperty": 3, "alarmType": 1, "probeType": 0, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": 100 },
      { "alarmZone": "L", "alarmProperty": 1, "alarmType": 1, "probeType": 1, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": 190 },
      { "alarmZone": "L", "alarmProperty": 1, "alarmType": 1, "probeType": 2, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": 50 }
    ],
    "configId": 47393
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

## Delete Template

**Interface Description**

Delete device configuration template.

**Interface Method**

`gw.configTemplate.delete`

**Request Parameter**

| Name     | Type | Required | Description                | Example |
|----------|------|----------|----------------------------|---------|
| configId | int  | Yes      | Configuration template id  | 3125    |

**Return Data**

| Name | Type    | Required | Description                  | Example |
|------|---------|----------|------------------------------|---------|
| data | boolean | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.configTemplate.delete",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "configId": 47397
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
