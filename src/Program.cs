using WebClient.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddHttpClient<CustomersRepository>(client =>
{
    client.BaseAddress = new Uri("https://nakdbaseserviceapi20211025120549.azurewebsites.net/api/customer");
});

builder.Services.AddTransient<CustomerIdDelegatingHandler>();
builder.Services.AddHttpClient<ProductsRepository>(client =>
{
    client.BaseAddress = new Uri("https://nakdbaseserviceapi20211025120549.azurewebsites.net/api/customer");
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
