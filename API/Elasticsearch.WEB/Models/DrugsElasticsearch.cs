using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elasticsearch.WEB.Models;

public class DrugsElasticsearch
{
    [JsonPropertyName("BarcodeId")]
    public long BarcodeId { get; set; }

    [JsonPropertyName("CREATEDATE")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("DrugBarcode")]
    public double DrugBarcode { get; set; }

    [JsonPropertyName("DrugId")]
    public int DrugId { get; set; }

    [JsonPropertyName("DrugName")]
    public string DrugName { get; set; }

    [JsonPropertyName("Producer")]
    public string Producer { get; set; }

    [JsonPropertyName("QRCODE")]
    public Guid QRCode { get; set; }

    [JsonPropertyName("RecipeType")]
    public string RecipeType { get; set; }

    [JsonPropertyName("Status")]
    public string Status { get; set; }
}

