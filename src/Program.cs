using Microsoft.Extensions.Options;
using WebClient.ConfigOptions;
using WebClient.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// We could extract the config of these services to extension methods
// but for the sake of simplicity we will leave them here

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddHttpClient<CustomersRepository>(
    (serviceProvider, client) =>
    {
        var config = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value;
        client.BaseAddress = new Uri(config.BaseAddress.Url);
    });

builder.Services.AddTransient<CustomerIdDelegatingHandler>();
builder.Services.AddHttpClient<ProductsRepository>(
    (serviceProvider, client) =>
    {
        var config = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value;
        client.BaseAddress = new Uri(config.BaseAddress.Url);
    }).AddHttpMessageHandler<CustomerIdDelegatingHandler>();

builder.Services.AddHttpContextAccessor();

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
