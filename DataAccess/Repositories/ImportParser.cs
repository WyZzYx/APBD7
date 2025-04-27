using Domain;

namespace DataAccess.Repositories;

public static class ImportParser {
    public static Device? Parse(string text) {
        var parts = text.Split(';', StringSplitOptions.TrimEntries);
        if (parts.Length < 4)
            throw new ArgumentException("Invalid import format");
        var type = parts[0];
        switch (type) {
            case "PC":
                return new PCDevice {
                    DeviceType    = "PC",
                    Name          = parts[1],
                    IsEnabled     = bool.Parse(parts[2]),
                    CanBeTurnedOn = bool.Parse(parts[3])
                };
            case "Embedded":
                return new EmbeddedDevice {
                    DeviceType = "Embedded",
                    Name       = parts[1],
                    IsEnabled  = bool.Parse(parts[2]),
                    Ip         = parts[3],
                    Network    = parts[4]
                };
            case "Smartwatch":
                return new Smartwatch {
                    DeviceType     = "Smartwatch",
                    Name           = parts[1],
                    IsEnabled      = bool.Parse(parts[2]),
                    BatteryPercent = int.Parse(parts[3])
                };
            default:
                throw new ArgumentException($"Unsupported device type: {type}");
        }
    }
}