# User Information

## Obtain User Information

**Interface Description**

Obtain user information.

**Interface Method**

`gw.userInfo.get`

**Request Parameter**

None

**Return Data**

| Name            | Type   | Required | Description                                    | Example                   |
|-----------------|--------|----------|------------------------------------------------|---------------------------|
| email           | string | Yes      | Email                                          | feedback@freshliance.com  |
| timeZone        | string | Yes      | Time zone (+08:00/+08:30, -01:00/-01:30...)    | +08:00                    |
| language        | int    | Yes      | Language: 1:English, 2:Chinese, 3:French, 4:German, 5:Russian, 6:Spanish | 1 |
| dateFormat      | int    | Yes      | Date format: 1:YYYY/MM/DD, 2:DD/MM/YYYY, 3:MM/DD/YYYY | 1 |
| temperatureUnit | int    | Yes      | Temperature unit: 1:℃, 2:℉                    | 1                         |
| chnSmsNum       | int    | Yes      | Remained Chinese Mainland SMS messages          | 217                       |
| intSmsNum       | int    | Yes      | Remained global SMS messages                    | 280                       |
| chnVoiceNum     | int    | Yes      | Remained Chinese Mainland voice call messages   | 100                       |
| nickname        | string | No       | User nickname                                   | feedback                  |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "tracker.userInfo.get",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1747387976177",
  "version": "1.0",
  "sign": "h/FR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
}
```

**Example for Returning Data**

```json
{
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
  "sign": "byWId5XcSjLY3tV+sfVmRLOo+bbkdnO4rnd/0sSq8JwkHoPJRK71HetuT6CUXSu/lG4Z/rt1w0N2TiSF6TKn0OdHQINI7tdrEWxvEjcPIoF0SgamAHDR+IoxwHA9TsE7QLuT4ieXQUpzvUPPPrxiTpNrEbc7etN2k1gNjaJEXKoP8I7PCQi/D+uusssvuSsLSuiYlPD/VtTDSjmye3tN7UyNa1EEImoyaTRD2iGPGoE4WqA16F5GTT/WPVA8qTgPabfU+izsrC2LDXuv+BqRHKzcGHID86xP7uDIjY/QW+sfRb/03S2Lbxm5UgXaaUIf/FkuwVsvxLqj6goJSECbUA==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Modify User Information

**Interface Description**

Modify user information.

**Interface Method**

`gw.userInfo.update`

**Request Parameter**

| Name            | Type   | Required | Description                                                                           | Example    |
|-----------------|--------|----------|---------------------------------------------------------------------------------------|------------|
| timeZone        | string | No       | Time zone (-12:30,-12:00 ... +10:30,+12:30)                                          | +08:00     |
| language        | int    | No       | Language: 1:English, 2:Chinese, 3:French, 4:German, 5:Russian, 6:Spanish             | 1          |
| dateFormat      | int    | No       | Date format: 1:YYYY/MM/DD, 2:DD/MM/YYYY, 3:MM/DD/YYYY                                | 1          |
| temperatureUnit | int    | No       | Temperature unit: 1:℃, 2:℉                                                           | 1          |
| nickname        | string | No       | User nickname (1-30 characters)                                                       | feedback   |

**Return Data**

| Name | Type | Required | Description                      | Example |
|------|------|----------|----------------------------------|---------|
| data | bool | Yes      | true: Success, false: Failed     | true    |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "tracker.userInfo.update",
  "format": "JSON",
  "charset": "UTF-8",
  "signType": "RSA2",
  "timestamp": "1747387976177",
  "version": "1.0",
  "bizContent": {
    "dateFormat": 1,
    "language": 1,
    "nickname": "freshliance",
    "temperatureUnit": 1,
    "timeZone": "+08:00"
  },
  "sign": "h/FR2xeKVOhSIbRY8A8xrXWNY98B5kFaitoKuJXXfLsDvfeyjzfYIJkvyU2RcwYgb3L+s9aq7xfxz43K/Rx1u2QQiKt30UOS0R9Wd59gqkLVke1uV0d5n40zVX/aakt0G82IlFb4LhuTH1HuGkNfLCRWawP8uq+Q97frtrlRKmXie7zEdHtIIkbvCTOu52dASfSRIKxtr20FjAUuA/Hy/LiytUSvobM6ZycOvuvifGIOyumVTDgh1pmBc/pcP6tCTb5g2JPm6W1TY97zkIZojOH7awb579wLgdIqaACNTWQoUWLBX9xkK5HFksQhzYYIBz5NxC5PLME1LHFg82fFDw=="
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
