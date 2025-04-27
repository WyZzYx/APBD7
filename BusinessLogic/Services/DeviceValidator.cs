using System.Net;
using Domain;

namespace BusinessLogic.Services;

public static class DeviceValidator {
    public static void Validate(Device? d) {
        if (string.IsNullOrWhiteSpace(d.Name))
            throw new ArgumentException("Name is required");

        switch (d) {
            case PCDevice pc when !pc.CanBeTurnedOn:
                throw new ArgumentException("PC must be able to turn on");
            case EmbeddedDevice emb:
                if (!IPAddress.TryParse(emb.Ip, out _) || string.IsNullOrWhiteSpace(emb.Network))
                    throw new ArgumentException("Invalid embedded device data");
                break;
            case Smartwatch sw when sw.BatteryPercent < 0 || sw.BatteryPercent > 100:
                throw new ArgumentException("Battery percent must be 0-100");
        }
    }
}