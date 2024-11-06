using web_db.Models;
using web_db.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<BikestoreContext>();
//builder.Services.AddScoped<IProductServices, ProductService>();
//builder.Services.AddScoped<ICategoryServices, CategoryService>();
//builder.Services.AddScoped<IStockServices, StockService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
