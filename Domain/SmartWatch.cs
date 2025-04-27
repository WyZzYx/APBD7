using System.Text.Json.Serialization;

namespace Domain;

public class Smartwatch : Device {
    [JsonConstructor]
    public Smartwatch() { }      

    public int BatteryPercent { get; set; }

    public Smartwatch(int batteryPercent)
    {
        BatteryPercent = batteryPercent;
    }
}