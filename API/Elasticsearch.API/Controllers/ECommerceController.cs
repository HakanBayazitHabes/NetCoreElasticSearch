using Elasticsearch.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ECommerceController : ControllerBase
{
    private readonly ECommerceRepository _repository;

    public ECommerceController(ECommerceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> TermQuery(string customerFirstName)
    {
        var response = await _repository.TermQuery(customerFirstName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
    {
        var response = await _repository.TermsQuery(customerFirstNameList);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> PrefixQuery(string customerFullName)
    {
        var response = await _repository.PrefixQueryAsync(customerFullName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
    {
        var response = await _repository.RangeQueryAsync(fromPrice, toPrice);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> MatchAll()
    {
        var response = await _repository.MatchAllQueryAsync();
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> PaginationQuery(int page = 1, int pageSize = 3)
    {
        var response = await _repository.PaginationQueryAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> WildCardQuery(string customerFullName)
    {
        var response = await _repository.WildCardQueryAsync(customerFullName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> FuzzyQuery(string customerName)
    {
        var response = await _repository.FuzzyQueryAsync(customerName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> MatchQueryFullText(string categoryName)
    {
        var response = await _repository.MatchQueryFullTextAsync(categoryName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> MatchBoolPrefixQueryFullText(string customerFullName)
    {
        var response = await _repository.MatchBoolPrefixFullTextAsync(customerFullName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> MatchPhraseQueryFullText(string customerFullName)
    {
        var response = await _repository.MatchPhraseFullTextAsync(customerFullName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double TaxFulTotalPrice, string categoryName, string menufacturer)
    {
        var response = await _repository.CompoundQueryExampleOneAsync(cityName, TaxFulTotalPrice, categoryName, menufacturer);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
    {
        var response = await _repository.CompoundQueryExampleTwoAsync(customerFullName);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> MultiMatchQueryFullText(string name)
    {
        var response = await _repository.MultiMatchQueryFullTextAsync(name);
        return Ok(response);
    }
}
