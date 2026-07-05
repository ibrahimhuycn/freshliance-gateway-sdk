# Rules

## Time Format

To enable the system to support different time zones, all time data will be uniformly transferred with a millisecond-level timestamp, and the time can be correspondingly switched to the local time according to the time zone preset.

## Temperature Unit

To enable the system to support different temperature units, all temperature data will be uniformly transferred with the unit of ℃, and can be switched to the corresponding temperature according to the temperature unit preset.

## Data Paging

To safeguard system stability and speed, the data on each page are limited to 50 items at most.

## Internationalization

To enable the system to support different languages, please set the language parameter `Accept-Language=en-US` at the request header when calling the interface. The platform will switch itself to the corresponding language according to the code transferred.

The languages supported:

| Language  | Code   |
|-----------|--------|
| English   | en-US  |
| Chinese   | zh-CN  |
| French    | fr     |
| German    | de     |
| Russian   | ru     |
| Spanish   | es     |
| Portuguese | pt    |
