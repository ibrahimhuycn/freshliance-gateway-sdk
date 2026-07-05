# Device Data

## Obtain Device Data Page

**Interface Description**

Obtain the page of device record data.

**Interface Method**

`gw.deviceData.page`

**Request Parameter**

| Name      | Type       | Required | Description                                                   | Example                      |
|-----------|------------|----------|---------------------------------------------------------------|------------------------------|
| recordId  | int        | Yes      | Record ID                                                     | 1234                         |
| probeType | int        | Yes      | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2 | 0                          |
| dataTime  | array[long]| No       | Start time and end time of data recording                     | [1747640566191,1747640566191] |
| pageNum   | int        | Yes      | Page number, starting from 1                                  | 1                            |
| pageSize  | int        | Yes      | Data quantity each page, Max. 50                              | 10                           |

**Return Data**

| Name            | Type          | Required | Description                                                                                   | Example         |
|-----------------|---------------|----------|-----------------------------------------------------------------------------------------------|------------------|
| total           | int           | Yes      | Total quantity                                                                                 | 4                |
| rows            | array[object] | Yes      | Data list                                                                                      |                  |
| rows.temperature| double        | No       | Temperature                                                                                    | 10.0             |
| rows.humidity   | double        | No       | Humidity                                                                                       | 10.0             |
| rows.light      | double        | No       | Illumination                                                                                   | 10.0             |
| rows.co2        | double        | No       | CO2                                                                                            | 10.0             |
| rows.dataTime   | long          | Yes      | Data time                                                                                      | 1747640566191    |
| rows.probeType  | int           | Yes      | Probe type: 0:Built-in, 1:External probe1, 2:External probe 2                                  | 0                |
| rows.status     | int           | Yes      | Probe data status: 0:Normal, 1:Probe not connected, 2:Mismatched probe type or malfunction     | 0                |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceData.page",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "recordId": 12822,
    "probeType": 0,
    "pageNum": 1,
    "pageSize": 2
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": {
    "total": 451,
    "rows": [
      {
        "dataTime": "1747640566191",
        "probeType": 0,
        "temperature": 26.7,
        "humidity": 53.1,
        "status": 0
      },
      {
        "dataTime": "1747640566191",
        "probeType": 0,
        "temperature": 26.7,
        "humidity": 53.3,
        "status": 0
      }
    ]
  },
  "msg": "success",
  "sign": "TlvOSCzXEOmx3TrCICyBgrJ/WJCZhRkmRVYiRV6QsyDWW6jhCQSK6BygYTpK5wICdWuxF5r+MIDukIJ8qR32TyM9iFfrR2uGw9t9tqpna3x6+O3z5F5vk9neslDVHf1BYofjMX6cizvAZLEQ6bbsDWgTZIZML5dwflj5u3mWyo1AFaN42Km2ZEZgPq+sIrppYDXm3VtfqCz9EVdPxm1Ar8qXB98OltZPyYLyQ0nkVW0kmZbehvt+9MQMV/yTokWK47tr4tu/irLQW+0wLo7itEGzm81LXXmSFd0Z40HnnvAVQq7htfJotItoE2h6aHFvYmeYOkXxIkcASzcElZnp+w==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Obtain the Page of Alarm Record

**Interface Description**

Obtain the alarm data page of device record.

**Interface Method**

`gw.deviceAlarmData.page`

**Request Parameter**

| Name           | Type | Required | Description                                                   | Example         |
|----------------|------|----------|---------------------------------------------------------------|------------------|
| recordId       | int  | Yes      | Record ID                                                     | 1234             |
| alarmProperty  | int  | Yes      | Property type: 1:Temperature, 2:Humidity, 3:Illumination, 4:CO2 | 1              |
| probeType      | int  | No       | Probe type: 0:Built-in, 1:External probe1, 2:External probe 2  | 0              |
| alarmType      | int  | No       | Alarm type: 1:Low alarm, 2:High alarm                          | 1                |
| alarmStartTime | long | No       | Start time                                                    | 1747640566191    |
| alarmEndTime   | long | No       | End time                                                      | 1747640566191    |
| pageNum        | int  | Yes      | Page number, starting from 1                                  | 1                |
| pageSize       | int  | Yes      | Data quantity each page, Max. 50                              | 10               |

**Return Data**

| Name                      | Type          | Required | Description                                                                     | Example         |
|---------------------------|---------------|----------|---------------------------------------------------------------------------------|------------------|
| total                     | int           | Yes      | Total quantity                                                                   | 4                |
| rows                      | array[object] | Yes      | Data list                                                                        |                  |
| rows.deviceAlarmId        | int           | Yes      | Device alarm ID                                                                  |                  |
| rows.recordId             | int           | Yes      | Record ID                                                                        |                  |
| rows.probeType            | int           | Yes      | Probe type: 0:Built-in, 1:External probe1, 2:External probe 2                    | 0                |
| rows.deviceId             | int           | Yes      | Device ID                                                                        |                  |
| rows.parentId             | int           | Yes      | Parent device ID                                                                 |                  |
| rows.dataId               | int           | Yes      | Data ID                                                                          |                  |
| rows.alarmZone            | int           | Yes      | Alarm zone: High alarm:H, Low alarm:L                                            | H                |
| rows.alarmProperty        | int           | Yes      | Alarm property: 1:Temperature, 2:Humidity                                        | 1                |
| rows.alarmType            | int           | Yes      | Alarm type: 1:Low alarm, 2:High alarm                                            | 1                |
| rows.alarmWay             | int           | Yes      | Alarm way: 1:Single, 2:Cumulative                                                | 1                |
| rows.alarmDelay           | int           | Yes      | Alarm delay                                                                      | 0                |
| rows.alarmThreshold       | double        | Yes      | Alarm threshold                                                                  | 20.0             |
| rows.propertyValue        | int           | Yes      | Property value                                                                   | 1                |
| rows.alarmTime            | long          | Yes      | Alarm time                                                                       |                  |
| rows.directorName         | string        | No       | Name of the person in charge                                                     |                  |
| rows.handlerId            | int           | No       | ID of the processor                                                              |                  |
| rows.handleStatus         | int           | Yes      | Processing status: 1:Processing, 2:Processed, 3:Ignored                          | 1                |
| rows.handleTime           | long          | No       | Processing time                                                                  |                  |
| rows.handleResult         | string        | No       | Result of processing                                                             |                  |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceAlarm.page",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "recordId": 12830,
    "probeType": 0,
    "pageNum": 1,
    "pageSize": 2
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": {
    "total": 9,
    "rows": [
      {
        "alarmZone": "L1",
        "alarmThreshold": 500,
        "alarmDelay": 0,
        "deviceId": 2058,
        "recordId": 12830,
        "dataId": 12166558,
        "handleStatus": 1,
        "alarmTime": "1747640566191",
        "propertyValue": 80,
        "updateTime": "1747640566191",
        "alarmWay": 1,
        "parentId": 2058,
        "alarmType": 1,
        "deleted": false,
        "createTime": "1747640566191",
        "probeType": 0,
        "alarmProperty": 3,
        "deviceAlarmId": 1756223
      },
      {
        "alarmZone": "L1",
        "alarmThreshold": 500,
        "alarmDelay": 0,
        "deviceId": 2058,
        "recordId": 12830,
        "dataId": 12166552,
        "handleStatus": 1,
        "alarmTime": "1747640566191",
        "propertyValue": 37,
        "updateTime": "1747640566191",
        "alarmWay": 1,
        "parentId": 2058,
        "alarmType": 1,
        "deleted": false,
        "createTime": "1747640566191",
        "probeType": 0,
        "alarmProperty": 3,
        "deviceAlarmId": 1756222
      }
    ]
  },
  "msg": "success",
  "sign": "LpJwyr5Patj0Ez5B6aH2xiPGngXsDzTDDT3yES2N4TnDXw2S51qWV+S5qwsP8UEpGY/nuYrzkN1hwKiNUibJTTDxk3HkbOcsLF7dyxOniL34MJ1V7LUtk5AYCcH8uDy7gsjNvISAHFauj1jSRtAfKAC2Dckj29v7YRMs9QhppZof2yPNtNgrkPuktnuN4i80ouUuXchpt6u13/mmMajGneVdrVXQkDohAd1Rng9lcSBKwzpJYc256LZNeuRTfHIJCLkwuAxJfKVLnGhfBAUAVflxSxt2JHuX41ta1xqnIKrAgRpe5fGZXahcGmUKEzd/GI8xbswd25f3sbN4SdLcKA==",
  "subCode": "",
  "subMsg": ""
}
```
