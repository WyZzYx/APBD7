using System.Text.Json.Serialization;

namespace Domain;

public class PCDevice : Device {
    [JsonConstructor]
    public PCDevice() { }

    public PCDevice(bool canBeTurnedOn)
    {
        CanBeTurnedOn = canBeTurnedOn;
    }

    public bool CanBeTurnedOn { get; set; }
}