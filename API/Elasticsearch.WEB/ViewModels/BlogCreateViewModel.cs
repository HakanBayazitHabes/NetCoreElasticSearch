using System.ComponentModel.DataAnnotations;

namespace Elasticsearch.WEB.ViewModels;

public class BlogCreateViewModel
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
}
