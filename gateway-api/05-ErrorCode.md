# Error Code

The platform will uniformly deal with the non-service calling errors of the developer's interface, and the return codes are as follows:

| code  | msg                          | subCode                       | subMsg                                                                 |
|-------|------------------------------|-------------------------------|------------------------------------------------------------------------|
| 0     | Success                      |                               |                                                                        |
| 10000 | Service error                | service-unknown-error         | Unknown errors occurred in the service                                 |
|       |                              | service-not-available         | The service is temporarily unavailable                                 |
| 20000 | Authentication error         | invalid-auth-token            | Invalid access token                                                   |
|       |                              | auth-token-timeout            | The access token has expired                                           |
| 30000 | Permission error             | no-permissions                | No api permissions                                                     |
|       |                              | access-forbidden              | Access forbidden                                                       |
|       |                              | ip-forbidden                  | IP access forbidden                                                    |
| 40000 | Parameter error              | missing-method                | Missing method name parameter                                          |
|       |                              | missing-signature             | Missing signature parameter                                            |
|       |                              | missing-signature-type        | Missing signature type parameter                                       |
|       |                              | missing-appId                 | Missing appId parameter                                                |
|       |                              | missing-timestamp             | Missing timestamp parameter                                            |
|       |                              | missing-version               | Missing version parameter                                              |
|       |                              | invalid-parameter             | Invalid parameter                                                      |
|       |                              | error-parameter               | Incorrect parameter                                                    |
|       |                              | upload-fail                   | File upload failed                                                     |
|       |                              | invalid-file-extension        | Invalid file extension                                                 |
|       |                              | invalid-file-size             | Invalid file size                                                      |
|       |                              | invalid-method                | Nonexistent method name                                                |
|       |                              | invalid-format                | Invalid data format                                                    |
|       |                              | invalid-signature-type        | Invalid signature type                                                 |
|       |                              | invalid-signature             | Invalid signature                                                      |
|       |                              | invalid-appId                 | Invalid appId parameter                                                |
|       |                              | invalid-timestamp             | Invalid timestamp parameter                                            |
|       |                              | invalid-charset               | Invalid character set                                                  |
|       |                              | missing-signature-config      | No key of the corresponding signature algorithm is configured          |
| 50000 | Business error               | biz-error                     | Business error                                                         |
| 60000 | Flow limit                   | request-call-limited          | Request call limit reached                                             |
| 99999 | Unknown error                | unknown-error                 | Unknown error                                                          |
