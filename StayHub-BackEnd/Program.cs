using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.Services.Admin;
using StayHub_BackEnd.Services.Avaliacao;
using StayHub_BackEnd.Services.DonoHotel;
using StayHub_BackEnd.Services.Hospede;
using StayHub_BackEnd.Services.Quarto;
using StayHub_BackEnd.Services.Reserva;
using Stripe;
using StayHub_BackEnd.Services.Pagamentos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
         options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
     });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddScoped<IAdmin, AdminService>();
builder.Services.AddScoped<IDonoHotel, DonoHotelService>();
builder.Services.AddScoped<IReserva, ReservaService>();
builder.Services.AddScoped<IHospede, HospedeService>();
builder.Services.AddScoped<IAvaliacao, AvaliacaoService>();
builder.Services.AddScoped<IQuarto, QuartoService>();
builder.Services.AddScoped<IPagamento, PagamentoService>();   

IServiceCollection serviceCollection = builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
