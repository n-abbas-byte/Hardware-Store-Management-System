using System.Text.Json.Serialization;
namespace HardwareShop;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductCategory
{
    Tools,
    Paint,
    Plumbing,
    Electrical,
    Fasteners,
    BuildingMaterials,
    Other
}
