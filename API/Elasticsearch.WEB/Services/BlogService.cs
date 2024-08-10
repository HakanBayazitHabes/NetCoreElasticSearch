using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Repositories;
using Elasticsearch.WEB.ViewModels;

namespace Elasticsearch.WEB.Services;

public class BlogService(BlogRepository repository)
{
    private readonly BlogRepository _repository = repository;


    public async Task<bool> SaveAsync(BlogCreateViewModel model)
    {
        var blog = new Blog
        {
            Title = model.Title,
            Content = model.Content,
            Tags = model.Tags.ToArray(),
            UserId = Guid.NewGuid(),
        };

        var response = await _repository.SaveAsync(blog);

        return response != null;
    }
}
