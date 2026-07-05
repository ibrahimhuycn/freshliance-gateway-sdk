namespace FreshlianceGateway.Sdk;

/// <summary>
/// Device operational status.
/// </summary>
public enum DeviceStatus
{
    /// <summary>Device is not activated yet.</summary>
    Inactive = 0,
    /// <summary>Device is online and reporting data.</summary>
    Online = 1,
    /// <summary>Device is offline.</summary>
    Offline = 2,
    /// <summary>Device has an abnormal condition.</summary>
    Abnormal = 3
}

/// <summary>
/// Alarm status indicating whether a monitored value is within normal range.
/// </summary>
public enum AlarmStatus
{
    /// <summary>Value is within the normal range.</summary>
    Normal = 1,
    /// <summary>Alarm condition triggered.</summary>
    Alarm = 2
}

/// <summary>
/// Type of hardware product or device.
/// </summary>
public enum ProductType
{
    /// <summary>Gateway device that aggregates sensor data.</summary>
    Gateway = 1,
    /// <summary>Sensor device for environmental monitoring.</summary>
    Sensor = 2,
    /// <summary>Coeus series device.</summary>
    Coeus = 3,
    /// <summary>GSP series device.</summary>
    Gsp = 4
}

/// <summary>
/// Supported display languages.
/// </summary>
public enum Language
{
    /// <summary>English language.</summary>
    English = 1,
    /// <summary>Chinese language.</summary>
    Chinese = 2,
    /// <summary>French language.</summary>
    French = 3,
    /// <summary>German language.</summary>
    German = 4,
    /// <summary>Russian language.</summary>
    Russian = 5,
    /// <summary>Spanish language.</summary>
    Spanish = 6
}

/// <summary>
/// Date display format.
/// </summary>
public enum DateFormat
{
    /// <summary>Year-Month-Day format (e.g., 2025-01-31).</summary>
    YyyyMmDd = 1,
    /// <summary>Day-Month-Year format (e.g., 31-01-2025).</summary>
    DdMmYyyy = 2,
    /// <summary>Month-Day-Year format (e.g., 01-31-2025).</summary>
    MmDdYyyy = 3
}

/// <summary>
/// Temperature measurement unit.
/// </summary>
public enum TemperatureUnit
{
    /// <summary>Celsius temperature scale.</summary>
    Celsius = 1,
    /// <summary>Fahrenheit temperature scale.</summary>
    Fahrenheit = 2
}

/// <summary>
/// Type of probe connected to a device.
/// </summary>
public enum ProbeType
{
    /// <summary>Internal built-in probe.</summary>
    BuiltIn = 0,
    /// <summary>First external probe.</summary>
    External1 = 1,
    /// <summary>Second external probe.</summary>
    External2 = 2
}

/// <summary>
/// Measurement property detected by a probe.
/// </summary>
public enum ProbeProperty
{
    /// <summary>Probe property is invalid or unassigned.</summary>
    Invalid = 0,
    /// <summary>Temperature measurement.</summary>
    Temperature = 1,
    /// <summary>Temperature plus humidity measurement.</summary>
    TemperatureHumidity = 2,
    /// <summary>Humidity measurement only.</summary>
    Humidity = 3,
    /// <summary>Illumination / light level measurement.</summary>
    Illumination = 4,
    /// <summary>CO2 concentration measurement.</summary>
    Co2 = 5
}

/// <summary>
/// Property type that triggers an alarm.
/// </summary>
public enum AlarmProperty
{
    /// <summary>Temperature threshold alarm.</summary>
    Temperature = 1,
    /// <summary>Humidity threshold alarm.</summary>
    Humidity = 2,
    /// <summary>Illumination threshold alarm.</summary>
    Illumination = 3,
    /// <summary>CO2 concentration threshold alarm.</summary>
    Co2 = 4
}

/// <summary>
/// Alarm threshold direction.
/// </summary>
public enum AlarmType
{
    /// <summary>Alarm triggers when value falls below the threshold.</summary>
    Low = 1,
    /// <summary>Alarm triggers when value exceeds the threshold.</summary>
    High = 2
}

/// <summary>
/// Alarm trigger mode.
/// </summary>
public enum AlarmWay
{
    /// <summary>Alarm triggers on a single occurrence.</summary>
    Single = 1,
    /// <summary>Alarm triggers based on cumulative occurrences.</summary>
    Cumulative = 2
}

/// <summary>
/// Status of a monitoring record or trip.
/// </summary>
public enum RecordStatus
{
    /// <summary>Record is currently active and in use.</summary>
    InUse = 1,
    /// <summary>Record has been stopped.</summary>
    Stopped = 2
}

/// <summary>
/// Buzzer audible alarm state.
/// </summary>
public enum BuzzerStatus
{
    /// <summary>Buzzer is turned on.</summary>
    On = 1,
    /// <summary>Buzzer is turned off.</summary>
    Off = 2
}

/// <summary>
/// Device battery or power supply status.
/// </summary>
public enum PowerStatus
{
    /// <summary>Power level is normal.</summary>
    Normal = 0,
    /// <summary>Battery is low and needs attention.</summary>
    LowBattery = 1
}

/// <summary>
/// Status of alarm handling or resolution.
/// </summary>
public enum HandleStatus
{
    /// <summary>Alarm is being processed.</summary>
    Processing = 1,
    /// <summary>Alarm has been processed.</summary>
    Processed = 2,
    /// <summary>Alarm has been ignored.</summary>
    Ignored = 3
}

/// <summary>
/// Status of probe data reading.
/// </summary>
public enum ProbeDataStatus
{
    /// <summary>Probe data is normal.</summary>
    Normal = 0,
    /// <summary>Probe is not connected.</summary>
    NotConnected = 1,
    /// <summary>Probe type does not match the expected configuration.</summary>
    Mismatched = 2
}

/// <summary>
/// Notification availability flag.
/// </summary>
public enum NotifyFlag
{
    /// <summary>Notification is available.</summary>
    Available = 1,
    /// <summary>Notification is not available.</summary>
    NotAvailable = 2
}

/// <summary>
/// Sensor configuration type indicating the number of probes.
/// </summary>
public enum ProductSensorType
{
    /// <summary>Built-in sensor only, no external probes.</summary>
    BuiltIn = 0,
    /// <summary>Built-in sensor plus one external probe.</summary>
    BuiltInPlusOneExternal = 1,
    /// <summary>Built-in sensor plus two external probes.</summary>
    BuiltInPlusTwoExternal = 2
}
