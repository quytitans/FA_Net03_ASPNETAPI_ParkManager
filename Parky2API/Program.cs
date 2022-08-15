using System.Configuration;
using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Parky2API;
using Parky2API.Data;
using Parky2API.Parky2Mapper;
using Parky2API.Repository;
using Parky2API.Repository.IRepository;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnectionNVQ2");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(ParkyMappings));

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;

});

builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");

builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("ParkyAPI", new OpenApiInfo
//    {
//        Version = "ParkyAPI",
//        Title = "ToDo API",
//        Description = "An ASP.NET Core Web API for managing ToDo items",
//        TermsOfService = new Uri("https://example.com/terms"),
//        Contact = new OpenApiContact
//        {
//            Name = "Example Contact",
//            Url = new Uri("https://example.com/contact")
//        },
//        License = new OpenApiLicense
//        {
//            Name = "Example License",
//            Url = new Uri("https://example.com/license")
//        }
//    });

//    //options.SwaggerDoc("NationalPark", new OpenApiInfo
//    //{
//    //    Version = "NationalPark",
//    //    Title = "ToDo API",
//    //    Description = "An ASP.NET Core Web API for managing ToDo items",
//    //    TermsOfService = new Uri("https://example.com/terms"),
//    //    Contact = new OpenApiContact
//    //    {
//    //        Name = "Example Contact",
//    //        Url = new Uri("https://example.com/contact")
//    //    },
//    //    License = new OpenApiLicense
//    //    {
//    //        Name = "Example License",
//    //        Url = new Uri("https://example.com/license")
//    //    }
//    //});
//    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
//    options.IncludeXmlComments(cmlCommentsFullPath);
//});
//builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }

    });

    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/ParkyAPI/swagger.json", "ParkyAPI");
    //    //options.SwaggerEndpoint("/swagger/NationalPark/swagger.json", "NationalPark");
    //    //options.RoutePrefix = string.Empty;
    //});
}

app.UseHttpsRedirection();

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
