using System.Text.Json.Serialization;

namespace Domain;

public class EmbeddedDevice : Device {
    
    [JsonConstructor]
    public EmbeddedDevice() { }

    public EmbeddedDevice(string ip, string network)
    {
        Ip = ip;
        Network = network;
    }

    public string Ip { get; set; }
    public string Network { get; set; }
}