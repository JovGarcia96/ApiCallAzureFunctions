using DispatchBalanceAPI.Controllers;
using DispatchBalanceAPI.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var apiCorsPolicy = "apiCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
               builder =>
               {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DispatchBalanceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseHttpsRedirection();

app.UseCors(apiCorsPolicy);

app.UseAuthorization();

app.MapControllers();

//app.MapSystemDateEndpoints();

//app.MapDispatchBalanceHeaderEndpoints();

app.Run();
