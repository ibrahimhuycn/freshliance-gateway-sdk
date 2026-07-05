# Signature

## Signature Procedure

### 1. Filter and sort

Obtain all request parameters, excluding the byte-type parameters, such as files and byte streams; remove the sign fields and the parameters with empty values, and sort by the ASCII code of the parameter names in ascending order (ascending alphabetical order). If the characters are same, they will be sorted in ascending order based on the ASCII code of the key value of the second character, and so on.

### 2. Composite Parameter

Combine the sorted parameters with their corresponding values in the format of "Parameter = Parameter value", and connect the parameters together with the & character. The string generated is the string to be signed.

### 3. Call Signature Function

Use the signature function SHA256WithRSA to sign the unsigned character strings via private key, and encode the result in Base64 format.

### 4. Composite Signature

Assign the generated signature to the sign parameter, and composite it with the request parameter.

## Signature Example

### Obtain User Device Interface

**Interface Description**

Obtain the user device list page by page based on the query conditions.

**Interface Method**

`tracker.userDevice.page`

**Request Parameter**

| Name       | Type   | Required | Description                       | Example     |
|------------|--------|----------|-----------------------------------|-------------|
| pageNum    | int    | Yes      | Page number, starting from 1      | 1           |
| pageSize   | int    | Yes      | Page size, 1-50 items             | 10          |
| deviceCode | String | No       | Device code                       | 250700097T  |

### Filter and Sort

Combine the common request parameters and the service request parameters, and then sort them in ascending order based on the ASCII code of the parameter names.

```
appId=658409073956360262328652394
bizContent={"pageNum":1,"pageSize":10}
charset=UTF-8
format=JSON
method=gw.userDevice.page
signType=RSA2
timestamp=1747208216323
version=1.0
```

### Composite Parameter

Combine the sorted parameters with their corresponding values in the format of "Parameter = Parameter value", and connect these parameters together with the & character. The character string to be signed is:

```
appId=658409073956360262328652394&bizContent={"pageNum":1,"pageSize":10}&charset=UTF-8&format=JSON&method=tracker.userDevice.page&signType=RSA2&timestamp=1747208216323&version=1.0
```

### Call Signature Function

Use the signature function SHA256WithRSA to sign the unsigned character strings via private key, and encode the results in Base64 format. The signed character string is:

```
fgGsofs1jOiWwWlEoEjl8/MeRBgLMcRB27Upx3c0WBMgaYKUvTWaB9LOcsnnSoFArA2Wn69X73271af8gqA6USqhYxC6vQDfCGOQm1k7maZ6VIMLfeY0QJR+PYQ9jbR4sizggBxBjyB3oWhgmMiRCQ5ZxUrFzjfhJi2sg6QALZsEyFJ2TpXIfdbw1DcDHRsM6825kysSWz9r4K3LcRBThSQs1HgvTM5hS3BBlsrx7FNWMXB7n+scSnugjSYewR0pNhOxv7Z3RIfd4LfNwTi5JSkjK2d5OCTH0aEjOz6jeT3jR1Xvwox5HPlrvDlkgaJZ3gOcxgNsnkWwEo0/BiY1Mw==
```

### Composite Signature

Assign the generated signature to the sign parameter, and composite it with the request parameter. The final request parameter is:

```
appId=2019032617262200001
bizContent={"pageNum":1,"pageSize":10}
charset=UTF-8
format=JSON
method=gw.userDevice.page
signType=RSA2
timestamp=1747208216323
version=1.0
sign=fgGsofs1jOiWwWlEoEjl8/MeRBgLMcRB27Upx3c0WBMgaYKUvTWaB9LOcsnnSoFArA2Wn69X73271af8gqA6USqhYxC6vQDfCGOQm1k7maZ6VIMLfeY0QJR+PYQ9jbR4sizggBxBjyB3oWhgmMiRCQ5ZxUrFzjfhJi2sg6QALZsEyFJ2TpXIfdbw1DcDHRsM6825kysSWz9r4K3LcRBThSQs1HgvTM5hS3BBlsrx7FNWMXB7n+scSnugjSYewR0pNhOxv7Z3RIfd4LfNwTi5JSkjK2d5OCTH0aEjOz6jeT3jR1Xvwox5HPlrvDlkgaJZ3gOcxgNsnkWwEo0/BiY1Mw==
```

## Verify Signature

Extract the signature value (the `sign` parameter) from the response result, and format the messages to generate the character string for final signature verification (the character string to be verified for signature); then follow the signature procedure to generate the signature and compare the generated signature with the signature extracted from the response result. If they are the same, the signature verification will be confirmed successful.
