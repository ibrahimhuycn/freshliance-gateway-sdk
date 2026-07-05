# Parameter

## Request Address

`https://api.freshliance.com/api`

## Request Method

`POST` `application/json`

## Common Request Parameter

| Parameter | Type   | Required | Max Length | Description                                                                                         | Example          |
|-----------|--------|----------|------------|-----------------------------------------------------------------------------------------------------|------------------|
| appId     | String | Yes      | 32         | Application ID assigned to the developer by the platform                                            | 2025050100002694 |
| method    | String | Yes      | 128        | Interface name                                                                                      | tracker.userInfo.get |
| format    | String | Yes      | 40         | JSON only                                                                                           | JSON             |
| charset   | String | Yes      | 10         | Coded format of the request, with the character set of UTF-8                                        | UTF-8            |
| signType  | String | Yes      | 10         | Signature algorithm type used for signature string generation, with the signature of RSA2           | RSA2             |
| sign      | String | Yes      | 344        | Signature of request parameter                                                                      | See the example for details |
| timestamp | String | Yes      | 19         | Timestamp for sending the request                                                                   | 1747187954506    |
| version   | String | Yes      | 3          | The version of the called interface, fixed to: 1.0                                                  | 1.0              |
| bizContent | String | Yes    |            | The set of request parameters, with no limitation on the maximum length. All request parameters, except the common parameters, must be transferred with this parameter. | XXX |

## Service Request Parameter

bizContent field is the service request parameter. Please refer to the interface file for details.

## Common Response Parameter

The response parameters remain constant, as shown below.

| Parameter | Type   | Required | Description                              | Example            |
|-----------|--------|----------|------------------------------------------|--------------------|
| code      | String | Yes      | Gateway return code                      | 40000              |
| msg       | String | Yes      | Description of the gateway return code   | Parameter error    |
| subCode   | String | No       | Service return code                      | invalid-parameter  |
| subMsg    | String | No       | Description of the service return code   | Invalid parameter  |
| sign      | String | Yes      | Signature                                | See the example for details |
| data      | String | No       | Service response parameter               | XXX                |

## Service Response Parameter

The data field is the service response parameter. Please refer to the interface file for details.
