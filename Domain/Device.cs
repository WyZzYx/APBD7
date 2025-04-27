using System.Text.Json.Serialization;

namespace Domain;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "deviceType")]
[JsonDerivedType(typeof(PCDevice),             "pc")]
[JsonDerivedType(typeof(EmbeddedDevice),  "embedded")]
[JsonDerivedType(typeof(Smartwatch),      "smartwatch")]
public abstract class Device {
    public Device() { }
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceType { get; set; }
}