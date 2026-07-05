# Remote Configuration

## Modify Device Parameters

**Interface Description**

Modify parameters of the device itself.

**Interface Method**

`gw.deviceCmd.updateParameter`

**Request Parameter**

| Name            | Type   | Required | Description                     | Example |
|-----------------|--------|----------|---------------------------------|---------|
| recordId        | int    | Yes      | Record ID                       | 2346    |
| buzzerStatus    | int    | Yes      | Buzzer On/Off: 1:On, 2:Off     | 1       |
| temperatureUnit | int    | Yes      | Temperature unit: 1:℃, 2:℉    | 1       |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.device.updateParameter",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "recordId": 12847,
    "buzzerStatus": 1,
    "temperatureUnit": 1
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

## Save History Data and Turn Off the Device

**Interface Description**

Turn off the device after all the data within the device that has not been uploaded to the server has been successfully uploaded.

**Interface Method**

`gw.deviceCmd.saveDataShutdown`

**Request Parameter**

| Name     | Type | Required | Description | Example |
|----------|------|----------|-------------|---------|
| recordId | int  | Yes      | Record ID   | 2543    |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceCmd.saveDataShutdown",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "recordId": 12847
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

## Device Turn-off Immediately

**Interface Description**

Discard the data that the server has not received and turn down the device directly, even if there is still data that has not been sent by the device.

**Interface Method**

`gw.deviceCmd.directShutdown`

**Request Parameter**

| Name     | Type | Required | Description | Example |
|----------|------|----------|-------------|---------|
| recordId | int  | Yes      | Record ID   | 2361    |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceCmd.directShutdown",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "recordId": 12847
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
  "sign": "n+jxGTJvT/+vbv8N4o2hyBi+7B1Hyg9tFAUoo6fMLPdmrbwyG9LRWpz9JWXaLG2rbapNe2US7Fqy20o4lfyMqYVw4FQ9+bunr/BfaWF7QNynoJ3z36dNicQ78NEZXNTEN+R4u36vE1TKpUkc7/zir+DyWcyz33FGXWVFKNRapKy7rpcfTt64MgPRfScexbeNRJje7kZVeiaawJ0R1fLhPjjZ9K+u7BIDH+WPHf3Q1q+DuZGxgsUApX5Vxgm/qpMeK5NNc/15X2RHlssYv3FQWrZK/EttCnLlclqmS03+857QE5CxAy0lqJkD2PKphxEMnkHSyeDqA8u0JysxsTH81A==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Save History Data and Reconfigure Device

**Interface Description**

Reconfigure the device after all device data are sent.

**Interface Method**

`gw.deviceCmd.saveDataConfig`

**Request Parameter**

| Name                                             | Type          | Required | Description                                                                                 | Example                    |
|--------------------------------------------------|---------------|----------|---------------------------------------------------------------------------------------------|----------------------------|
| issuedDeviceCmd                                  | object        | Yes      | Send device command                                                                         |                            |
| issuedDeviceCmd.recordId                         | long          | Yes      | Device record ID                                                                            | 2356                       |
| sensorAlarmList                                  | array[object] | No       | List of sensor alarm points                                                                 |                            |
| sensorAlarmList.alarmZone                        | string        | No       | Alarm zone: High alarm:H, Low alarm:L                                                        | H                          |
| sensorAlarmList.alarmProperty                    | int           | No       | Alarm property: 1:Temperature, 2:Humidity, 3:Illumination, 4:CO2                             | 2                          |
| sensorAlarmList.alarmType                        | int           | No       | Alarm type: 1:Low alarm, 2:High alarm                                                        | 1                          |
| sensorAlarmList.probeType                        | int           | No       | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2                               | 0                          |
| sensorAlarmList.alarmWay                         | int           | No       | Alarm way: 1:Single, 2:Cumulative                                                            | 1                          |
| sensorAlarmList.alarmDelay                       | int           | No       | Alarm delay                                                                                  | 10                         |
| sensorAlarmList.alarmThreshold                   | double        | No       | Alarm threshold                                                                              | 50.0                       |
| wifiConfigInfo                                   | object        | Yes      | Basic device information                                                                     |                            |
| wifiConfigInfo.userDeviceId                      | int           | Yes      | User device ID                                                                               | 2356                       |
| wifiConfigInfo.notifyEmailFlag                   | int           | No       | Email notification: 1:Available, 2:Not available                                              | 1                          |
| wifiConfigInfo.notifyDayEmailCount               | int           | No       | Email notifications to be sent per day                                                       | 10                         |
| wifiConfigInfo.notifyEmailInterval               | int           | No       | Email sending interval                                                                       | 10                         |
| wifiConfigInfo.notifySingleEmailCount            | int           | No       | Email notifications per device                                                               | 10                         |
| wifiConfigInfo.notifyAlarmEmail                  | string        | No       | Email address (split multiple addresses with ,)                                               | freshliance@xjwl.com       |
| wifiConfigInfo.notifySmsFlag                     | int           | No       | SMS notifications: 1:Available, 2:Not available                                               | 1                          |
| wifiConfigInfo.notifyDaySmsCount                 | int           | No       | SMS notifications to be sent per day                                                         | 10                         |
| wifiConfigInfo.notifySmsInterval                 | int           | No       | SMS sending interval                                                                         | 10                         |
| wifiConfigInfo.notifySingleSmsCount              | int           | No       | SMS notifications per alarm point                                                            | 10                         |
| wifiConfigInfo.notifyAlarmSms                    | string        | No       | Mobile phone number (split multiple numbers with ,)                                           | +86-139*****11             |
| wifiConfigInfo.notifyTimeFlag                    | int           | No       | Time notification flag: 0:No settings, 1:Email sending time, 2:SMS notification, 3:Both      | 0                          |
| wifiConfigInfo.notifyStartTime                   | string        | No       | Start time of the notification                                                               | 00:00                      |
| wifiConfigInfo.notifyEndTime                     | string        | No       | End time of the notification                                                                 | 23:59                      |
| wifiConfigInfo.notifyDate                        | string        | No       | Notification date (0,1,2... 0=Sunday)                                                        | 0,1,2                      |
| issuedDeviceCmdConfig                            | object        | Yes      | Configuration information sent                                                               |                            |
| issuedDeviceCmdConfig.temperatureUnit            | int           | Yes      | Temperature unit: 1:℃, 2:℉                                                                 | 1                          |
| issuedDeviceCmdConfig.timeZone                   | string        | Yes      | UTC Time zone                                                                                | +08:00                     |
| issuedDeviceCmdConfig.buzzerStatus               | int           | Yes      | Buzzer On/Off: 1:On, 2:Off                                                                  | 1                          |
| issuedDeviceCmdConfig.configTime                 | string        | Yes      | Configuration time                                                                           |                            |
| issuedDeviceCmdConfig.deviceName                 | string        | No       | Device name                                                                                  |                            |
| startDelay                                       | int           | Yes      | Start delay                                                                                  | 1                          |
| collectInterval                                  | int           | Yes      | Logging interval                                                                             | 1                          |
| uploadInterval                                   | int           | Yes      | Uploading interval                                                                           | 1                          |
| probeInfoList                                    | array[object] | Yes      | Probe information                                                                            |                            |
| probeInfoList.probeType                          | int           | Yes      | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2                               | 0                          |
| probeInfoList.probeProperty                      | int           | Yes      | Probe property: 0:Invalid, 1:Temperature, 2:Temperature and humidity, 3:Humidity              | 0                          |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | int  | Yes      | Command ID (true: Success)   | 1519    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceCmd.saveDataConfig",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "issuedDeviceCmd": {
      "recordId": "83986"
    },
    "sensorAlarmList": [
      {
        "alarmProperty": 1,
        "alarmType": 2,
        "probeType": 0,
        "alarmWay": 1,
        "alarmDelay": 0,
        "alarmThreshold": -10,
        "alarmZone": "H"
      }
    ],
    "wifiConfigInfo": {
      "notifyEmailFlag": 1,
      "notifyDayEmailCount": 20,
      "notifySingleEmailCount": 1,
      "notifyEmailInterval": 30,
      "notifySmsFlag": 1,
      "notifyDaySmsCount": 20,
      "notifySmsInterval": 30,
      "notifySingleSmsCount": 1,
      "notifyVoiceFlag": 1,
      "notifyDayVoiceCount": 20,
      "notifyVoiceInterval": 0,
      "notifySingleVoiceCount": 1,
      "notifyAlarmVoice": "+86-1733531153,",
      "notifyAlarmEmail": "1813094047@qq.com",
      "notifyAlarmSms": "+86-1733531153,",
      "userDeviceId": "5996",
      "notifyTimeFlag": 3,
      "notifyStartTime": "00:00",
      "notifyEndTime": "23:59",
      "notifyDate": "0,1,2,3,4,5,6"
    },
    "issuedDeviceCmdConfig": {
      "timeZone": "+00:00",
      "temperatureUnit": 1,
      "buzzerStatus": 1
    },
    "collectInterval": 1,
    "uploadInterval": 1,
    "probeInfoList": [
      { "probeType": 0, "probeProperty": 2 },
      { "probeType": 1, "probeProperty": 2 },
      { "probeType": 2, "probeProperty": 2 }
    ]
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": 1519,
  "msg": "success",
  "sign": "LBg1K82gXXnkTaP0PKFWhXv/DIhnR2WnWu5HKT9FA3s9XQEQyMq2LbXsIG9GIXDRqYPLLiKmbLYYzDDnLV1k3MCFTpSsskvvYQZqqisyiq+Fz8eCKXgBQgZoOE1qhuy7PSebAuiUktuHacBaAQJph+eUlXAsFnri+xH7C8C6VuGCVLMVRwViBk1UO5+AVBifmOzugbgEyIPLGRwVhiqi2sSoengceeg7bVmr9BJ2cg9ceh5dsNVGuTc7jEg4gR1AWxYNuUJVw1/q02ABNH8w3Jkyd9XUofiKpTwTgErL8bQGKNx2VuGoR9ORZ27Md2o2kNbiYATulLWx+1fDS0OuUg==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Immediately Reconfigure

**Interface Description**

Immediately reconfigure the device.

**Interface Method**

`gw.deviceCmd.directConfig`

**Request Parameter**

Same parameters as "Save History Data and Reconfigure Device" above.

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | int  | Yes      | Command ID (true: Success)   | 1520    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceCmd.directConfig",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "issuedDeviceCmd": { "recordId": "83986" },
    "sensorAlarmList": [{ "alarmProperty": 1, "alarmType": 2, "probeType": 0, "alarmWay": 1, "alarmDelay": 0, "alarmThreshold": -10, "alarmZone": "H" }],
    "wifiConfigInfo": {
      "notifyEmailFlag": 1, "notifyDayEmailCount": 20, "notifySingleEmailCount": 1, "notifyEmailInterval": 30,
      "notifySmsFlag": 1, "notifyDaySmsCount": 20, "notifySmsInterval": 30, "notifySingleSmsCount": 1,
      "notifyVoiceFlag": 1, "notifyDayVoiceCount": 20, "notifyVoiceInterval": 0, "notifySingleVoiceCount": 1,
      "notifyAlarmVoice": "+86-1733531153,", "notifyAlarmEmail": "1813094047@qq.com", "notifyAlarmSms": "+86-1733531153,",
      "userDeviceId": "5996", "notifyTimeFlag": 3, "notifyStartTime": "00:00", "notifyEndTime": "23:59", "notifyDate": "0,1,2,3,4,5,6"
    },
    "issuedDeviceCmdConfig": { "timeZone": "+00:00", "temperatureUnit": 1, "buzzerStatus": 1 },
    "collectInterval": 1, "uploadInterval": 1,
    "probeInfoList": [
      { "probeType": 0, "probeProperty": 2 },
      { "probeType": 1, "probeProperty": 2 },
      { "probeType": 2, "probeProperty": 2 }
    ]
  },
  "sign": "hFR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
  "code": "0",
  "data": 1520,
  "msg": "success",
  "sign": "C+BfOvfpbKtstLSryTLNp3ib/txmOrClp9/jHRtwypn9D+CeAlN+57eXyn8u5Ukv1b8UQ7jm/HcWW2Euyx79cQ+DGnXrUTcjywHiyWnmrNVKNmDEPin2wcV96wXojCef7cvbmqxZjV98T9cQiM+6LtrkqalHcMDqIceq6HegCXRG9s4YINPurBam4BQvtMNRUs3+rdqlQ7qMkdcHKxJdoS1A4k7639GHCsurTeB2shuUIAmjLpeOncgGXZU7n7RVsowQZqx2LO2hCoaRm2Q0hxD3TB2Nye0o7p0e7TMcY7Qj/5Jd4byc8X9LSUGPPlBunequxpH63AeGcFrd18F/8A==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Delete the Remotely Sent Command

**Interface Description**

When the device has not obtained the command sent, delete the command and enable it invalid.

**Interface Method**

`gw.deviceCmd.delete`

**Request Parameter**

| Name | Type | Required | Description       | Example |
|------|------|----------|-------------------|---------|
| id   | int  | Yes      | Remote command ID | 726     |

**Return Data**

| Name | Type | Required | Description                  | Example |
|------|------|----------|------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceCmd.delete",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
    "id": 1521
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
