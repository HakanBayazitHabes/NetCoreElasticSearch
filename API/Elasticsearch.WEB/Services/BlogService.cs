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

    public async Task<List<BlogViewModel>> SearchAsync(string searchText)
    {
        var blogList = await _repository.SearchAsync(searchText);

        return blogList.Select(b => new BlogViewModel()
        {
            Id = b.Id,
            Title = b.Title,
            Content = b.Content,
            Created = b.Created.ToShortDateString(),
            Tags = string.Join(",", b.Tags),
            UserId = b.UserId.ToString()

        }).ToList();
    }
}
