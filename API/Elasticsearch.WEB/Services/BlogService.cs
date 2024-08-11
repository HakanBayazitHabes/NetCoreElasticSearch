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
            Tags = model.Tags.Split(","),
            UserId = Guid.NewGuid(),
        };

        var response = await _repository.SaveAsync(blog);

        return response != null;
    }

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        return await _repository.SearchAsync(searchText);
    }
}
