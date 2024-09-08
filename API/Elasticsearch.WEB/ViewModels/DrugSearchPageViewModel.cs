namespace Elasticsearch.WEB.Models
{
    public class DrugSearchPageViewModel
    {
        public long TotalCount { get; set; }
        public List<DrugsElasticsearch> List { get; set; }
        public DrugSearchViewModel SearcViewModel { get; set; }
    }
}