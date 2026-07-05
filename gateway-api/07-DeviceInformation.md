# Device Information

## Obtain Device Category

**Interface Description**

Obtain current product information category.

**Interface Method**

`gw.device.category`

**Request Parameter**

None

**Return Data**

| Name           | Type   | Required | Description                                                                                   | Example       |
|----------------|--------|----------|-----------------------------------------------------------------------------------------------|---------------|
| categoryId     | int    | Yes      | Product category ID                                                                           | 20            |
| categoryName   | string | Yes      | Product category name                                                                         | TH30/TH20     |
| inTemHigh      | double | Yes      | Max. temperature value of built-in sensor                                                     | 100.0         |
| inTemLow       | double | Yes      | Min. temperature value of built-in sensor                                                     | -30.0         |
| extTemHigh     | double | Yes      | Max. temperature value of external probe                                                      | 100.0         |
| extTemLow      | double | Yes      | Min. temperature value of external probe                                                      | -30.0         |
| productSensor  | int    |          | Built-in sensor type: 0:Built-in sensor, 1:Built-in sensor+One external probe, 2:Built-in sensor+2 external probes | 1 |
| remark         | string | Yes      | Remarks                                                                                       |               |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.device.category",
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
      "productSensor": 0,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "T10",
      "categoryId": 1
    },
    {
      "productSensor": 0,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "TH20",
      "categoryId": 2
    },
    {
      "extTemHigh": 70,
      "extTemLow": -30,
      "productSensor": 2,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "TH10B/TH10R/TH30B/TH30R",
      "categoryId": 3
    },
    {
      "extTemHigh": 500,
      "extTemLow": -200,
      "productSensor": 2,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "TH30R-ETU",
      "categoryId": 4
    },
    {
      "extTemHigh": 200,
      "extTemLow": -80,
      "productSensor": 2,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "TH30R-I/TH10-IRS",
      "categoryId": 5
    },
    {
      "extTemHigh": 200,
      "extTemLow": -200,
      "productSensor": 1,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "LoRa",
      "categoryId": 6
    },
    {
      "extTemHigh": 200,
      "extTemLow": -80,
      "productSensor": 2,
      "inTemHigh": 85,
      "inTemLow": -40,
      "categoryName": "COEUS-WIFI/4G",
      "categoryId": 7
    },
    {
      "extTemHigh": 200,
      "extTemLow": -200,
      "productSensor": 0,
      "inTemHigh": 70,
      "inTemLow": -30,
      "categoryName": "TH30-ERS",
      "categoryId": 8
    },
    {
      "extTemHigh": 190,
      "extTemLow": -90,
      "productSensor": 2,
      "categoryName": "GSP",
      "categoryId": 9
    }
  ],
  "msg": "success",
  "sign": "BavuPcfuDO7T0qD33Bm/oqTqeeoj4vw9eHwPQ9D8AQ5pJTPrzeD/sbMdUYJHYu5px+HuaFuyxk6tiEMkVDLGJd9I/pA66Pv9w16bpEUas15ReUhsoZ4LK10zoR8yc7QN+r/Le4tFnFpP8FTIgYlclUVoWHvbVRPNy8qPmiohNr/QTuPhFSWzVYjfpzDw/rtEFUtvCr7gXCEjlBh15gPR/GjYJC9P5/74eVAr7SmFFD+YFPZfpU2DAgADr3EjR4fyujvDxz85wNIFWs/hpfV2uU3snTvTPC1HTL6CwSGkY/k++qSakS8f6UkJUXyg2juz6MjzvfwxtS7oWhJsX+RO1A==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Obtain Device List Page

**Interface Description**

Obtain the device information list of the user.

**Interface Method**

`gw.deviceInfo.page`

**Request Parameter**

| Name         | Type   | Required | Description                                                                | Example      |
|--------------|--------|----------|----------------------------------------------------------------------------|--------------|
| deviceSn     | string | No       | Device ID                                                                  | 201200000W   |
| deviceName   | string | No       | Device name                                                                | Warehouse 1  |
| deviceStatus | int    | No       | Device status: 0:Not activated, 1:Online, 2:Offline, 3:Abnormal            | 1            |
| alarmStatus  | int    | No       | Alarm status: 1:Alarm, 2:Normal                                            | 1            |
| productType  | int    | No       | Product type: 1:Gateway, 2:Sensor, 3:COEUS, 4:GSP                         | 1            |
| powerStatus  | int    | No       | Low battery: 0:Normal, 1:Low battery <=20                                  | 0            |
| keyword      | string | No       | Keyword search                                                             |              |
| pageNum      | int    | Yes      | Page number, starting from 1                                               | 1            |
| pageSize     | int    | Yes      | Data quantity each page, Max. 50                                           | 20           |

**Return Data**

| Name                                            | Type           | Required | Description                                                                                             | Example       |
|-------------------------------------------------|----------------|----------|---------------------------------------------------------------------------------------------------------|---------------|
| total                                           | int            | Yes      | Total data quantity                                                                                      | 22            |
| rows                                            | array[object]  | Yes      | Data                                                                                                     |               |
| rows.deviceInfo                                 | object         | Yes      | Device information                                                                                       |               |
| rows.deviceInfo.userDeviceId                    | int            | Yes      | User device ID                                                                                           | 2334          |
| rows.userParentId                               | int            | Yes      | Parent device ID of the user                                                                             |               |
| rows.deviceInfo.productCode                     | string         | Yes      | Device type                                                                                              | A0            |
| rows.deviceInfo.deviceSn                        | string         | Yes      | Device ID                                                                                                | 200200210F    |
| rows.deviceInfo.deviceCode                      | string         | Yes      | MAC address                                                                                              | 48E700577E13  |
| rows.deviceInfo.deviceName                      | string         | Yes      | Device name                                                                                              | Warehouse 1   |
| rows.deviceInfo.deviceId                        | int            | Yes      | Device ID                                                                                                | 1248          |
| rows.deviceInfo.recordId                        | int            | Yes      | Record ID                                                                                                | 44651         |
| rows.deviceInfo.productType                     | int            | Yes      | Product type: 1:Gateway, 2:Sensor, 3:COEUS, 4:GSP                                                       | 3             |
| rows.deviceInfo.productModel                    | string         | Yes      | Device model                                                                                             | COEUS-WIFI    |
| rows.deviceInfo.userParentId                    | int            | Yes      | Device ID of parent device user                                                                          | 123           |
| rows.deviceInfo.deviceStatus                    | int            | Yes      | Device status: 0:Not activated, 1:Online, 2:Offline, 3:Abnormal                                          | 0             |
| rows.deviceInfo.statusTime                      | long           | Yes      | Status time                                                                                              | 1755662900000 |
| rows.deviceInfo.alarmStatus                     | int            | Yes      | Alarm status: 1:Normal, 2:Alarm                                                                          | 2             |
| rows.deviceInfo.alarmTime                       | long           | Yes      | Alarm time                                                                                               | 1755662900000 |
| rows.deviceInfo.devicePower                     | int            | Yes      | Device power                                                                                             | 22            |
| rows.deviceInfo.powerTime                       | long           | Yes      | Power time                                                                                               | 1755662900000 |
| rows.deviceInfo.gatewaySn                       | string         | No       | Home gateway ID                                                                                          | 250700000W    |
| rows.subDeviceLastDataList                      | array[object]  | Yes      | Sub-device data information                                                                              |               |
| rows.subDeviceLastDataList.temperature          | double         | No       | Temperature                                                                                              | 20.0          |
| rows.subDeviceLastDataList.humidity             | double         | No       | Humidity                                                                                                 | 20.0          |
| rows.subDeviceLastDataList.light                | double         | No       | Illumination                                                                                             | 6000          |
| rows.subDeviceLastDataList.co2                  | double         | No       | CO2                                                                                                      | 2000          |
| rows.subDeviceLastDataList.dataTime             | long           | Yes      | Data time                                                                                                | 1755662900000 |
| rows.subDeviceLastDataList.temHigh              | double         | No       | Min. value of high temperature alarm                                                                     | 20.0          |
| rows.subDeviceLastDataList.temLow               | double         | No       | Max. value of low temperature alarm                                                                      | 10.0          |
| rows.subDeviceLastDataList.humHigh              | double         | No       | Min. value of high humidity alarm                                                                        | 20.0          |
| rows.subDeviceLastDataList.humLow               | double         | No       | Max. value of low humidity alarm                                                                         | 10.0          |
| rows.subDeviceLastDataList.lightHigh            | double         | No       | Min. value of high illumination alarm                                                                    | 10000         |
| rows.subDeviceLastDataList.lightLow             | double         | No       | Max. value of low illumination alarm                                                                     | 1000          |
| rows.subDeviceLastDataList.co2High              | double         | No       | Min. value of high CO2 alarm                                                                             | 10000         |
| rows.subDeviceLastDataList.co2Low               | double         | No       | Max. value of low CO2 alarm                                                                              | 1000          |
| rows.subDeviceLastDataList.probeType            | int            | Yes      | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2                                           | 1             |
| rows.subDeviceLastDataList.probeProperty        | int            | Yes      | Probe property: 0:Invalid, 1:Temperature, 2:Temperature and humidity, 3:Illumination, 4:CO2              | 0             |
| rows.subDeviceLastDataList.probeAlarmStatus     | int            | Yes      | Probe alarm status: 0:Normal, 1:Temperature alarm, 2:Temperature and humidity alarm, 3:Humidity alarm    | 0             |
| rows.subDeviceLastDataList.probeAlarmTime       | long           | No       | Probe alarm time                                                                                         | 1755662900000 |
| rows.subDeviceLastDataList.status               | int            | Yes      | Data status: 0:Normal, 1:Probe not connected, 2:Mismatched probe type or malfunction                     | 0             |
| rows.deviceDeviceStateCount                     | object         | Yes      | Quantity of the gateway sub-devices in each status                                                       |               |
| rows.deviceDeviceStateCount.deviceCount         | int            | Yes      | Total quantity of devices                                                                                | 1             |
| rows.deviceDeviceStateCount.onlineCount         | int            | Yes      | Quantity of online devices                                                                               | 0             |
| rows.deviceDeviceStateCount.offlineCount        | int            | Yes      | Quantity of offline devices                                                                              | 1             |
| rows.deviceDeviceStateCount.alarmCount          | int            | Yes      | Quantity of alarm devices                                                                                | 0             |
| rows.deviceDeviceStateCount.abnormalCount       | int            | Yes      | Quantity of abnormal devices                                                                             | 0             |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceInfo.page",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
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
        "deviceDeviceStateCount": {
          "alarmCount": 0,
          "onlineCount": 0,
          "offlineCount": 0,
          "deviceCount": 0,
          "abnormalCount": 0
        },
        "deviceInfo": {
          "productModel": "COEUS-WIFI",
          "userDeviceId": 4566,
          "deviceCode": "C4D8D5078C78",
          "deviceSn": "241001383F",
          "deviceId": 2071,
          "deviceName": "241001383F",
          "deviceStatus": 2,
          "recordId": 12922,
          "alarmStatus": 1,
          "powerTime": 1756992726000,
          "productCode": "94",
          "productScene": 2,
          "userParentId": 4566,
          "statusTime": 1757032347000,
          "devicePower": 29,
          "productType": 3,
          "gatewaySn": "241001383F"
        },
        "subDeviceLastDataList": [
          {
            "dataTime": 1756983688000,
            "probeType": 0,
            "probeProperty": 2,
            "temperature": 28,
            "humidity": 57.1,
            "probeAlarmStatus": 0,
            "status": 0
          }
        ]
      },
      {
        "deviceDeviceStateCount": {
          "alarmCount": 0,
          "onlineCount": 0,
          "offlineCount": 0,
          "deviceCount": 0,
          "abnormalCount": 0
        },
        "deviceInfo": {
          "productModel": "COEUS-WIFI",
          "userDeviceId": 4565,
          "deviceCode": "94C960FA2BE6",
          "deviceSn": "200700727F",
          "deviceId": 2085,
          "deviceName": "200700727F",
          "deviceStatus": 2,
          "recordId": 12920,
          "alarmStatus": 1,
          "powerTime": 1756799640000,
          "productCode": "94",
          "productScene": 2,
          "userParentId": 4565,
          "statusTime": 1756800302000,
          "devicePower": 1,
          "productType": 3,
          "gatewaySn": "200700727F"
        },
        "subDeviceLastDataList": [
          {
            "dataTime": 1756799628000,
            "probeType": 0,
            "probeProperty": 2,
            "temperature": 25,
            "humidity": 50.7,
            "probeAlarmStatus": 0,
            "status": 0
          }
        ]
      }
    ]
  },
  "msg": "success",
  "sign": "jiPBJNk0lSwkn4VVqqQsXJVzrgBztf0XOa9DHtO4zTjS6m36w/A636iy00/x57b7SdK6JSvbTqiOa/iiz5sZvvcFY4R6hFDpzS7A6iMQmiD8sSw6HFoaSEDofTbdIeF1Vcc9k7iu1qOY2cDLxfPcj/cdThVyVboYNXAG6TmqahX0gkxuMDKB6C1WSOm78zOSAgcQtbWMfysttpnwUg50Hfpa3h+JHHgyPQlbRnQTqEbeHACMF7Gi31tEwavl5fSxLu0SkkKXzaYeyv0vddUC7XZl+9K8j95y9HCXYkk5YFCFnwUKOI+wPIOkaHe3DIHv1iFULWB27GLpuOYWAlnTUw==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Obtain Usage Record Page

**Interface Description**

Obtain the information page of device usage record.

**Interface Method**

`gw.deviceInfo.recordPage`

**Request Parameter**

| Name         | Type   | Required | Description                                                       | Example         |
|--------------|--------|----------|-------------------------------------------------------------------|------------------|
| deviceId     | int    | No       | Device ID                                                         | 2230            |
| deviceSn     | string | No       | Device ID                                                         | 201200000W      |
| recordStatus | int    | No       | Device status: 1:In use, 2:Stopped                                | 1               |
| deviceName   | string | No       | Device name                                                       | Warehouse 1     |
| startTime    | long   | No       | Start time                                                        | 1747640566191   |
| endTime      | long   | No       | End time                                                          | 1747640566191   |
| productType  | int    | No       | Product type: 1:Gateway, 2:Sensor, 3:COEUS, 4:GSP                | 1               |
| keyword      | string | No       | Keyword search                                                    |                 |
| pageNum      | int    | Yes      | Page number, starting from 1                                      | 1               |
| pageSize     | int    | Yes      | Data quantity each page, Max. 50                                  | 20              |

**Return Data**

| Name                                            | Type           | Required | Description                                                                                                     | Example         |
|-------------------------------------------------|----------------|----------|-----------------------------------------------------------------------------------------------------------------|-----------------|
| total                                           | int            | Yes      | Total data quantity                                                                                              | 22              |
| rows                                            | array[object]  | Yes      | Data                                                                                                             |                 |
| rows.deviceRecord                               | object         | Yes      | Device record information                                                                                        |                 |
| rows.deviceRecord.recordId                      | int            | Yes      | Record ID                                                                                                        | 2234            |
| rows.deviceRecord.userDeviceRecordId            | int            | Yes      | User device record ID                                                                                            | 22498           |
| rows.deviceRecord.deviceSn                      | string         | Yes      | Device ID                                                                                                        | 250100020F      |
| rows.deviceRecord.parentDeviceId                | int            | Yes      | Parent device ID                                                                                                 | 22498           |
| rows.deviceRecord.parentDeviceSn                | string         | Yes      | Parent device serial number                                                                                      | 250100020F      |
| rows.deviceRecord.parentDeviceName              | string         | Yes      | Parent device name                                                                                               | Warehouse 1     |
| rows.deviceRecord.deviceName                    | string         | Yes      | Device name                                                                                                      | Container 1     |
| rows.deviceRecord.productModel                  | string         | Yes      | Device model                                                                                                     | COEUS-WIFI      |
| rows.deviceRecord.productType                   | int            | Yes      | Device type                                                                                                      | 3               |
| rows.deviceRecord.recordStatus                  | int            | Yes      | Record status: 1:In use, 2:Stopped                                                                               | 1               |
| rows.deviceRecord.timeZone                      | string         | No       | Time zone of the device                                                                                          | +08:00          |
| rows.deviceRecord.temperatureUnit               | int            | No       | Temperature unit: 1:℃, 2:℉                                                                                     | 1               |
| rows.deviceRecord.buzzerStatus                  | int            | No       | Buzzer On/Off: 1:On, 2:Off                                                                                      | 1               |
| rows.deviceRecord.collectInterval               | int            | Yes      | Logging interval                                                                                                 | 30              |
| rows.deviceRecord.uploadInterval                | int            | No       | Uploading interval                                                                                               |                 |
| rows.deviceRecord.startDelay                    | int            | No       | Start delay                                                                                                      |                 |
| rows.deviceRecord.alarmStatus                   | int            | Yes      | Alarm status: 1:Normal, 2:Alarm                                                                                  | 1               |
| rows.deviceRecord.alarmTime                     | long           | No       | Alarm time                                                                                                       |                 |
| rows.deviceRecord.dataCount                     | int            | Yes      | Quantity of data records                                                                                         | 20              |
| rows.sensorConfig                               | array[object]  |          | Sensor configuration information                                                                                 |                 |
| rows.sensorConfig.configId                      | int            | Yes      | Configuration ID                                                                                                 | 2698            |
| rows.sensorConfig.probeType                     | int            | Yes      | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2                                                   | 0               |
| rows.sensorConfig.categoryId                    | int            | Yes      | Category ID                                                                                                      | 7               |
| rows.sensorConfig.probeProperty                 | int            | Yes      | Probe property: 0:Invalid, 1:Temperature, 2:Temperature and humidity, 3:Humidity                                 | 2               |
| rows.sensorConfig.probeAlarmStatus              | int            | Yes      | Probe alarm status: 0:Normal, 1:Temperature alarm, 2:Temp+Humidity alarm, 3:Humidity alarm, 4:Illumination, 5:CO2 | 0            |
| rows.sensorConfig.probeAlarmTime                | long           | No       | Probe alarm time                                                                                                 |                 |
| rows.sensorConfig.probeAlarmId                  | int            | No       | Probe alarm ID                                                                                                   | 1322            |
| rows.sensorConfig.parentId                      | int            | Yes      | Parent configuration ID                                                                                          | 9700            |
| rows.sensorConfig.productCode                   | string         | Yes      | Product code                                                                                                     | 94              |
| rows.sensorConfig.temHigh                       | double         | No       | Min. value of high temperature alarm                                                                             |                 |
| rows.sensorConfig.temLow                        | double         | No       | Max. value of low temperature alarm                                                                              |                 |
| rows.sensorConfig.humHigh                       | double         | No       | Min. value of high humidity alarm                                                                                |                 |
| rows.sensorConfig.humLow                        | double         | No       | Max. value of low humidity alarm                                                                                 |                 |
| rows.sensorConfig.lightHigh                     | double         | No       | Min. value of high illumination alarm                                                                            |                 |
| rows.sensorConfig.lightLow                      | double         | No       | Max. value of low illumination alarm                                                                             |                 |
| rows.sensorConfig.co2High                       | double         | No       | Min. value of high CO2 alarm                                                                                     |                 |
| rows.sensorConfig.co2Low                        | double         | No       | Max. value of low CO2 alarm                                                                                      |                 |
| rows.subDeviceLastDataList                      | array[object]  | Yes      | Temperature and humidity information of sub-devices                                                              |                 |
| rows.subDeviceLastDataList.temperature          | double         | No       | Temperature                                                                                                      | 20.0            |
| rows.subDeviceLastDataList.humidity             | double         | No       | Humidity                                                                                                         | 20.0            |
| rows.subDeviceLastDataList.light                | double         | No       | Illumination                                                                                                     |                 |
| rows.subDeviceLastDataList.co2                  | double         | No       | CO2                                                                                                              |                 |
| rows.subDeviceLastDataList.dataTime             | long           | Yes      | Data time                                                                                                        | 1755662900000   |
| rows.subDeviceLastDataList.probeType            | int            | Yes      | Probe type: 0:Built-in, 1:External probe 1, 2:External probe 2                                                   |                 |
| rows.subDeviceLastDataList.probeProperty        | int            | Yes      | Probe property: 0:Invalid, 1:Temperature, 2:Temperature and humidity, 3:Illumination, 4:CO2                      |                 |
| rows.subDeviceLastDataList.probeAlarmStatus     | int            | Yes      | Probe alarm status: 0:Normal, 1:Temperature alarm, 2:Temperature and humidity alarm, 3:Humidity alarm            |                 |
| rows.subDeviceLastDataList.probeAlarmTime       | long           | No       | Probe alarm time                                                                                                 |                 |
| rows.subDeviceLastDataList.status               | int            | Yes      | Data status: 0:Normal, 1:Probe not connected, 2:Mismatched probe type or malfunction                             |                 |

**Example for Requesting Parameter**

```json
{
  "appId": "658409073956360262328652394",
  "method": "gw.deviceInfo.recordPage",
  "signType": "RSA2",
  "timestamp": "1755662900000",
  "version": "1.0",
  "bizContent": {
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
    "total": 41,
    "rows": [
      {
        "deviceRecord": {
          "buzzerStatus": 2,
          "productModel": "COEUS-WIFI",
          "userDeviceRecordId": 10523,
          "timeZone": "+08:00",
          "deviceSn": "241090710F",
          "deviceName": "241090710F",
          "dataCount": 125,
          "recordId": 12881,
          "uploadInterval": 1,
          "alarmStatus": 1,
          "recordStatus": 1,
          "collectInterval": 1,
          "temperatureUnit": 1,
          "parentDeviceName": "241090710F",
          "parentDeviceId": 2070,
          "parentDeviceSn": "241090710F",
          "startDelay": 0,
          "productType": 3
        },
        "sensorConfig": [
          {
            "parentId": 47507,
            "productCode": "94",
            "probeType": 1,
            "configId": 47508,
            "probeProperty": 0,
            "probeAlarmStatus": 0
          },
          {
            "parentId": 47507,
            "productCode": "94",
            "probeType": 2,
            "configId": 47509,
            "probeProperty": 0,
            "probeAlarmStatus": 0
          },
          {
            "parentId": 0,
            "productCode": "94",
            "probeType": 0,
            "configId": 47507,
            "probeProperty": 2,
            "probeAlarmStatus": 0
          }
        ],
        "subDeviceLastDataList": [
          {
            "dataTime": 1756204811000,
            "probeType": 0,
            "probeProperty": 2,
            "temperature": 24.7,
            "probeAlarmStatus": 0,
            "humidity": 56.8,
            "status": 0
          }
        ]
      }
    ]
  },
  "msg": "success",
  "sign": "B03A56tGEUTRvHwW1sQKBef5ISoX/FuIRle8lVb6k5opFya0i3RrTSIWOctbgHmVGMAbDqawATKq7zxdnSH/DfutNc8C4I7kqldZYcLiwpaV577pdTyYA3/OOk6q2JOZMO9SnD1h5o8Wk+S+XSXg36W+51WmJWUDCVl6BkFoVG4mreOR5yK+274lEHBrzKsa5YuUB6Of8QO6kjGOWXeA+nV76geekAxNsoi+8JpQExoS4GtX74iaBrrZLIq6ClijhYaKOkxYonMEP0gZsv49JNlyn6eT88RHoN1y2evIjgy/zU0o81G9aX97iH3Ye6E8+aR4UPjeZUbjDpqL/5e3gg==",
  "subCode": "",
  "subMsg": ""
}
```

---

## Obtain the Page of Sub-devices Under the Gateway

**Interface Description**

Obtain the page of sub-devices under the gateway.

**Interface Method**

`gw.deviceInfo.subPage`

**Request Parameter**

| Name         | Type   | Required | Description                                                       | Example      |
|--------------|--------|----------|-------------------------------------------------------------------|--------------|
| userDeviceId | int    | Yes      | User device ID (cannot be empty)                                  | 2230         |
| deviceSn     | string | No       | Device ID                                                         | 201200000W   |
| deviceName   | string | No       | Device name                                                       | Warehouse 1  |
| deviceStatus | int    | No       | Device status: 0:Not activated, 1:Online, 2:Offline, 3:Abnormal   | 1            |
| alarmStatus  | int    | No       | Alarm status                                                      | 1            |
| keyword      | string | No       | Keyword search                                                    |              |
| pageNum      | int    | Yes      | Page number, starting from 1                                      | 1            |
| pageSize     | int    | Yes      | Data quantity each page, Max. 50                                  | 20           |

**Return Data**

| Name                                            | Type           | Required | Description                                                                   | Example |
|-------------------------------------------------|----------------|----------|-------------------------------------------------------------------------------|---------|
| total                                           | int            | Yes      | Total data quantity                                                            | 22      |
| rows                                            | array[object]  | Yes      | Data                                                                           |         |
| rows.subDeviceInfo                              | object         | No       | User sub-device information                                                    |         |
| rows.subDeviceInfo.userDeviceId                 | int            | Yes      | User device ID                                                                 |         |
| rows.subDeviceInfo.recordId                     | int            | Yes      | Record ID                                                                      |         |
| rows.subDeviceInfo.deviceName                   | string         | Yes      | Device name                                                                    |         |
| rows.subDeviceInfo.deviceSn                     | string         | Yes      | Device ID                                                                      |         |
| rows.subDeviceInfo.deviceStatus                 | int            | Yes      | Device status                                                                  |         |
| rows.subDeviceInfo.alarmStatus                  | int            | Yes      | Alarm status                                                                   |         |
| rows.subDeviceInfo.productCode                  | string         | Yes      | Device type                                                                    |         |
| rows.subDeviceInfo.productModel                 | string         | Yes      | Device model                                                                   |         |
| rows.subDeviceInfo.devicePower                  | int            | No       | Device battery                                                                 |         |
| rows.subDeviceInfo.collectInterval              | int            | Yes      | Logging interval                                                               |         |
| rows.subDeviceInfo.productProperty              | int            | Yes      | Product property: 1:Temperature, 2:Temperature and humidity, 3:Humidity        |         |
| rows.subDeviceLastDataList                      | array[object]  | No       | Temperature and humidity information of sub-devices                            |         |
| rows.subDeviceLastDataList.temperature          | double         | No       | Temperature                                                                    |         |
| rows.subDeviceLastDataList.humidity             | double         | No       | Humidity                                                                       |         |
| rows.subDeviceLastDataList.light                | double         | No       | Illumination                                                                   |         |
| rows.subDeviceLastDataList.co2                  | double         | No       | CO2                                                                            |         |
| rows.subDeviceLastDataList.dataTime             | long           | No       | Data time                                                                      |         |
| rows.subDeviceLastDataList.probeType            | int            | No       | Probe type                                                                     |         |
| rows.subDeviceLastDataList.probeProperty        | int            | No       | Probe property                                                                 |         |
| rows.subDeviceLastDataList.probeAlarmStatus     | int            | No       | Probe alarm status                                                             |         |
| rows.subDeviceLastDataList.probeAlarmTime       | long           | No       | Probe alarm time                                                               |         |
| rows.subDeviceLastDataList.status               | int            | No       | Data status                                                                    |         |
