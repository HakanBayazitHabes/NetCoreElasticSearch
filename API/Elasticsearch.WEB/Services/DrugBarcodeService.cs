using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Repositories;

namespace Elasticsearch.WEB.Services
{
    public class DrugBarcodeService(DrugBarcodeRepository drugRepository)
    {
        private readonly DrugBarcodeRepository _drugRepository = drugRepository;

        // Tüm DrugBarcode'ları getir
        public IEnumerable<DrugBarcode> SearchDrugBarcodesWithEnumerable(string producer, string drugName, Guid qrCode)
        {
            return _drugRepository.SearchDrugBarcodesWithEnumerable(producer, drugName, qrCode);
        }

        // Producer ve DrugName'e göre filtrelenmiş DrugBarcode'ları getir
        public List<DrugBarcode> SearchDrugBarcodesQueryable(string producer, string drugName, Guid qrCode)
        {
            return _drugRepository.SearchDrugBarcodesAsQueryable(producer, drugName, qrCode);
        }
    }
}