using Scalar.AspNetCore;
using System.Buffers.Text;
using System.Reflection;
using WebApi2026.Middlewares;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",new Microsoft.OpenApi.OpenApiInfo
    {
        Title="API ForeCast Documentation By AbyLeyva",
        Version="1.0",
        Description="Can get ramdon weather by preview list",
        Contact= new Microsoft.OpenApi.OpenApiContact
        {
            Name="Aby Leyva",
            Email="me@abyleyva.com"
        }
        
    }
          
        );
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

var myAllowSpecificOrigins = "myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        p =>
        {
            p.WithOrigins("https://localhost:7002", "http://localhost:5072","http://127.0.0.1:5500");
            p.AllowAnyHeader();
            //p.AllowAnyOrigin();
            p.AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

//Custom Middlewares Here
app.UseRequestLogging();

// End Custom Middleware

app.MapControllers();

app.UseCors(myAllowSpecificOrigins);

app.Run();
