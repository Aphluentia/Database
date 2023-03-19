using DatabaseApi.Models.Settings;
using DatabaseApi.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MongoDbDatabase"));
builder.Services.AddSingleton<DatabaseServices>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
