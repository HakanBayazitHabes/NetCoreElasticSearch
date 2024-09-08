
using Elasticsearch.WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace Elasticsearch.WEB.Repositories;


public class DrugBarcodeRepository
{
    private readonly AppContext _context;

    public DrugBarcodeRepository(AppContext context)
    {
        _context = context;
    }

    // Tüm DrugBarcode'ları ve ilişkili Drug verilerini getir
    public IEnumerable<DrugBarcode> SearchDrugBarcodesWithEnumerable(string producer, string drugName, Guid qrCode)
    {

        if (string.IsNullOrEmpty(producer) && string.IsNullOrEmpty(drugName) && string.IsNullOrEmpty(qrCode.ToString()))
        {
            return [];  // Veritabanına sorgu yapmadan boş bir liste döndürüyoruz
        }
        // Veritabanından tüm DrugBarcode verilerini çekiyoruz
        var drugBarcodes = _context.DRUGBARCODE
            .Include(db => db.Drug).OrderByDescending(db => db.Id)
            .ToList();  // Verileri belleğe alıyoruz


        // Producer'a göre filtreleme yapıyoruz
        if (!string.IsNullOrEmpty(producer))
        {
            drugBarcodes = drugBarcodes.Where(db => db.Drug.Producer.Contains(producer)).ToList();
        }

        // DrugName'e göre filtreleme yapıyoruz
        if (!string.IsNullOrEmpty(drugName))
        {
            drugBarcodes = drugBarcodes.Where(db => db.Drug.DrugName.Contains(drugName)).ToList();
        }

        if (qrCode != Guid.Empty)
        {
            drugBarcodes = drugBarcodes.Where(db => db.QRCode == qrCode).ToList();
        }

        return drugBarcodes;  // Filtrelenmiş sonuçları döndürüyoruz
    }

    // Producer ve DrugName'e göre DrugBarcode'ları getir (ilişkili ilaç bilgisiyle)
    public List<DrugBarcode> SearchDrugBarcodesAsQueryable(string producer, string drugName, Guid qrCode)
    {

        if (string.IsNullOrEmpty(producer) && string.IsNullOrEmpty(drugName) && string.IsNullOrEmpty(qrCode.ToString()))
        {
            return [];  // Veritabanına sorgu yapmadan boş bir liste döndürüyoruz
        }

        var query = _context.DRUGBARCODE
            .Include(db => db.Drug)  // DrugBarcode ile ilişkili Drug verilerini de getiriyoruz
            .AsQueryable();

        if (!string.IsNullOrEmpty(producer))
        {
            query = query.Where(db => db.Drug.Producer.Contains(producer));  // Producer'a göre filtrele
        }

        if (!string.IsNullOrEmpty(drugName))
        {
            query = query.Where(db => db.Drug.DrugName.Contains(drugName));  // DrugName'e göre filtrele
        }

        if (qrCode != Guid.Empty)
        {
            query = query.Where(db => db.QRCode == qrCode);
        }

        return query.ToList();  // Sonuçları listele ve geri döndür
    }
}