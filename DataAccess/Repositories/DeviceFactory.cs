using System.Text.Json;
using Domain;

namespace DataAccess.Repositories;

public static class DeviceFactory {
    public static Device FromRecord(
        string deviceType,
        string id,
        string name,
        bool isEnabled,
        string typeValueJson
    ) {
        Device d = deviceType switch {
            "PC" => JsonSerializer.Deserialize<PCDevice>(typeValueJson)!,
            "Embedded" => JsonSerializer.Deserialize<EmbeddedDevice>(typeValueJson)!,
            "Smartwatch" => JsonSerializer.Deserialize<Smartwatch>(typeValueJson)!,
            _ => throw new ArgumentException($"Unknown deviceType: {deviceType}")
        };
        d.Id = id;
        d.Name = name;
        d.IsEnabled = isEnabled;
        d.DeviceType = deviceType;
        return d;
    }
}