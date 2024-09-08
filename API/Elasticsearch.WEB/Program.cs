using Elasticsearch.WEB.Extensions;
using Elasticsearch.WEB.Repositories;
using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.Services.DrugElasticsearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddElastic(builder.Configuration);
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<ECommerceService>();
builder.Services.AddScoped<ECommerceRepository>();
builder.Services.AddScoped<DrugBarcodeRepository>();
builder.Services.AddScoped<DrugBarcodeService>();
builder.Services.AddScoped<DrugElasticSearchRepository>();
builder.Services.AddScoped<DrugElasticsearchService>();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
