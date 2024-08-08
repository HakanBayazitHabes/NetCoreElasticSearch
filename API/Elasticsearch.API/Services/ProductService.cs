using System.Net;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.DTOs;
using Elasticsearch.API.Repositories;

namespace Elasticsearch.API.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {
        var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());

        if (responseProduct is null)
        {
            return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında bir hata meydana geldi." }, HttpStatusCode.InternalServerError);
        }

        return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);
    }

    public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
    {
        var responseProducts = await _productRepository.GetAllAsync();

        var productListDto = responseProducts.Select(x => x.CreateDto()).ToList();

        return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {
        var hasProduct = await _productRepository.GetByIdAsync(id);

        if (hasProduct is null)
        {
            return ResponseDto<ProductDto>.Fail("Ürün bulunamadı.", HttpStatusCode.NotFound);
        }

        return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
    {
        var isSuccess = await _productRepository.UpdateAsync(updateProduct);

        if (!isSuccess)
        {
            return ResponseDto<bool>.Fail("Güncelleme işlemi sırasında bir hata meydana geldi.", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);

    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        var deleteResponse = await _productRepository.DeleteAsync(id);

        if (!deleteResponse.IsValidResponse && deleteResponse.Result == Result.NotFound)
        {
            return ResponseDto<bool>.Fail("Silme işlemi sırasında bir hata meydana geldi.", HttpStatusCode.NotFound);
        }

        if (!deleteResponse.IsValidResponse)
        {
            deleteResponse.TryGetOriginalException(out Exception? exception);
            _logger.LogError(exception, deleteResponse.ElasticsearchServerError.Error.ToString());
            return ResponseDto<bool>.Fail("Silme işlemi sırasında bir hata meydana geldi.", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
    }
}
