using System.ComponentModel.DataAnnotations.Schema;

namespace Elasticsearch.WEB.Models
{
    public class Drug
    {
        public int Id { get; set; }  // Primary Key
        public string DrugName { get; set; }
        public double Barcode { get; set; }
        public string Producer { get; set; }
        [Column("RECIPETYPE")]  // Veritabanındaki sütun adı farklıysa
        public string RecipeType { get; set; }
        public string Status_ { get; set; }

        // Navigation Property for Relationship
        public ICollection<DrugBarcode> DrugBarcodes { get; set; }  // Bir ilacın birden fazla barkodu olabilir
    }
}