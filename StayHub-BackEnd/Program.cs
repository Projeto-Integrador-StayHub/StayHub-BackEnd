using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.Services.Admin;
using StayHub_BackEnd.Services.Avaliacao;
using StayHub_BackEnd.Services.DonoHotel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAdmin, AdminService>();
builder.Services.AddScoped<IDonoHotel, DonoHotelService>();
builder.Services.AddScoped<IAvaliacao, AvaliacaoService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
