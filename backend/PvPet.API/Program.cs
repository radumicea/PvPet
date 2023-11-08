using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PvPet.API.Configurations;
using PvPet.Data.Contexts;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using PvPet.Business.Middleware;

const string myPolicy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(static options =>
{
    options.AddPolicy(myPolicy, static policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddControllers();

builder.Services.AddDbContext<DbContext, PvPetDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Database"),
        // Avoid cartesian explosion
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    );

builder.Services.AddSingleton<AutoMapper.IConfigurationProvider>
    (
        new MapperConfiguration(cfg =>
        {
            cfg.AddMaps("PvPet.Business");
            cfg.AllowNullCollections = true;
        })
    )
    .AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

builder.Services.RegisterBusinessServices();

builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<ExceptionHandlerMiddleware>();

var app = builder.Build();

//app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PvPetDbContext>();
    await context.InitAsync();
}

app.UseHttpsRedirection();

app.UseHsts();

app.UseCors(myPolicy);

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
