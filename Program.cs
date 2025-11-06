using CotizadorEquipoContratista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Cotización Equipos Contratistas", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' seguido de su token JWT."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/Cotizacion/RegistrarMacro",
    async ([FromBody] CotizacionEquipoContratistaModel paramItem) =>
    {
        try
        {
            using var client = new HttpClient();
            //var url = "https://localhost:5041/api/Cotizacion/RegistrarMacro";
            var url = "https://www.opcionseguros.seg.ar/brokersdev/api/CotizacionEquipoContratistaApi/Registrar";

            var response = await client.PostAsJsonAsync(url, paramItem);
            var content = await response.Content.ReadAsStringAsync();

            return Results.Content(content, "application/json");
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error al llamar al servicio RegistrarMacro: {ex.Message}", statusCode: 500);
        }
    })
.WithName("RegistrarMacro")
.WithOpenApi(operation =>
{
    operation.Summary = "Registrar cotización de equipos contratistas";
    operation.Description = "Envía una cotización a la API real y devuelve el resultado del registro.";
    operation.RequestBody = new OpenApiRequestBody
    {
        Required = true,
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(CotizacionEquipoContratistaModel)
                    }
                }
            }
        }
    };
    operation.Responses["200"] = new OpenApiResponse
    {
        Description = "Respuesta exitosa del registro.",
        Content =
    {
        ["application/json"] = new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["draw"] = new OpenApiSchema { Type = "integer" },
                    ["recordsFiltered"] = new OpenApiSchema { Type = "integer" },
                    ["recordsTotal"] = new OpenApiSchema { Type = "integer" },
                    ["message"] = new OpenApiSchema { Type = "string" },
                    ["isError"] = new OpenApiSchema { Type = "boolean" },
                    ["showMessage"] = new OpenApiSchema { Type = "boolean" },
                    ["data"] = new OpenApiSchema
                    {
                        Type = "object",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = nameof(SysCotizacionEquiposContratistasResult)
                        }
                    }
                }
            }
        }
    }
    };

    return operation;
});



const string ApiKey = "prueba"; 

app.MapGet("/api/Cotizacion/GetToken", async (HttpContext context) =>
{

    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var providedKey) || providedKey != ApiKey)
    {
        return Results.Unauthorized();
    }

    using var client = new HttpClient();
    var url = "http://localhost:32677/api/CotizacionEquipoContratistaApi/GetToken";

    try
    {
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return Results.Content(content, "application/json");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al llamar al servicio GetToken: {ex.Message}", statusCode: 500);
    }
})
.WithName("GetToken")
.WithOpenApi(operation =>
{
    operation.Summary = "Obtiene un token desde la API";
    operation.Description = "Llama a la API CotizacionEquipoContratistaApi/GetToken para obtener un token válido.";
    operation.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
    {
        new()
        {
            Name = "X-API-KEY",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Required = true,
            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" },
            Description = "API Key requerida para acceder al token"
        }
    };
    return operation;
});


app.Run();



public class CotizacionEquipoContratistaModel
{
    public Cotizacion Cotizacion { get; set; }
    public EContratista EContratista { get; set; }
}

public class Cotizacion
{
    public string Asegurado { get; set; }
    public string Cuit { get; set; }
    public string Domicilio { get; set; }
    public string Organizador { get; set; }
    public string Productor { get; set; }
    public string Provincia { get; set; }
    public bool VisibleOrganizador { get; set; }
}

public class EContratista
{
    public int ActidadId { get; set; }
    public string Actividad { get; set; }
    public string Amortizacion { get; set; }
    public string Caracteristicas { get; set; }
    public string Cobertura { get; set; }
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public string HastaTxt { get; set; }
    public string Id_Tasas { get; set; }
    public string Modelo { get; set; }
    public string Moneda { get; set; }
    public string Operacion { get; set; }
    public string Pais { get; set; }
    public string PorcAsegurado { get; set; }
    public string Raltura { get; set; }
    public string Rcalle { get; set; }
    public string Rlocalidad { get; set; }
    public string Rprovincia { get; set; }
    public decimal Suma { get; set; }
    public decimal SumaPlCollder { get; set; }
    public string Tipocotizacion { get; set; }
    public int Confirmacion { get; set; }
}

public class TokenResponse
{
    public TokenResponse() { }

    public TokenResponse(object data)
    {
        Data = data;
        IsError = false;
        Message = string.Empty;
    }

    public TokenResponse(string message, bool isError = true)
    {
        Message = message;
        IsError = isError;
    }

    public object Data { get; set; }
    public string Message { get; set; }
    public bool IsError { get; set; }
}
