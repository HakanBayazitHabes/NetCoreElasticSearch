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
}
