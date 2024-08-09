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
}
