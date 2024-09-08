namespace Elasticsearch.WEB.Models
{
    public class DrugBarcode
{
    public long Id { get; set; }  // Primary Key
    public Guid QRCode { get; set; }
    public int DrugId { get; set; }  // Foreign Key
    public DateTime CreateDate { get; set; }

    // Navigation Property for Relationship
    public Drug Drug { get; set; }  // Her barkodun bir ilacı olur
}
}