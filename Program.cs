using CotizadorEquipoContratista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using System.Net.Http.Headers;
using Microsoft.OpenApi.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var apiBase = builder.Configuration["apiBase"] ?? throw new InvalidOperationException("Error ApiBase");
var apiKey = builder.Configuration["ApiKey"] ?? throw new InvalidOperationException("Error ApiKey");

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Cotización Equipos Contratistas", Version = "v1" });
    
    c.CustomSchemaIds(type => type.Name);
    
    c.UseAllOfToExtendReferenceSchemas();
    
    c.DocumentFilter<SchemaRegistrationFilter>();
    
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
    async (HttpRequest request, [FromBody] CotizacionEquipoContratistaModel paramItem) =>
    {
        try
        {
            using var client = new HttpClient();
            var url = $"{apiBase}CotizacionEquipoContratistaApi/RegistrarMacro";

            if (request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var authHeaderValue = authHeader.ToString();
                
                if (AuthenticationHeaderValue.TryParse(authHeaderValue, out var parsedHeader))
                {
                    if (!string.IsNullOrEmpty(parsedHeader.Parameter))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedHeader.Parameter);
                    }
                    else if (!string.IsNullOrEmpty(parsedHeader.Scheme) && 
                             parsedHeader.Scheme != "Bearer" && parsedHeader.Scheme != "Basic")
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedHeader.Scheme);
                    }
                }
                else if (authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) || 
                         authHeaderValue.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeaderValue.Substring(7).Trim();
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
            }

            var response = await client.PostAsJsonAsync(url, paramItem);
            var content = await response.Content.ReadAsStringAsync();

            // Reenviar el código de estado HTTP y el contenido de la respuesta
            return Results.Content(
                content,
                "application/json",
                statusCode: (int)response.StatusCode
            );
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
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(CotizacionResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["draw"] = new OpenApiInteger(0),
                    ["recordsFiltered"] = new OpenApiInteger(0),
                    ["recordsTotal"] = new OpenApiInteger(0),
                    ["data"] = new OpenApiObject
                    {
                        ["Prima"] = new OpenApiString("0"),
                        ["Total"] = new OpenApiString("0"),
                        ["Couta"] = new OpenApiDouble(0),
                        ["UrlFile"] = new OpenApiString("")
                    },
                    ["message"] = new OpenApiString(""),
                    ["isError"] = new OpenApiBoolean(false),
                    ["showMessage"] = new OpenApiBoolean(false)
                }
            }
        }
    };
    
    operation.Responses["401"] = new OpenApiResponse
    {
        Description = "No autorizado - Token ausente o inválido.",
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(TokenResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["Data"] = new OpenApiNull(),
                    ["Message"] = new OpenApiString("Acceso no autorizado: token inválido"),
                    ["IsError"] = new OpenApiBoolean(true)
                }
            }
        }
    };
    
    operation.Responses["400"] = new OpenApiResponse
    {
        Description = "Solicitud incorrecta - No se pudo registrar la cotización.",
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(TokenResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["Data"] = new OpenApiNull(),
                    ["Message"] = new OpenApiString("No se pudo registrar la cotización"),
                    ["IsError"] = new OpenApiBoolean(true)
                }
            }
        }
    };
    
    operation.Responses["404"] = new OpenApiResponse
    {
        Description = "No encontrado - No se encontró la cotización registrada.",
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(TokenResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["Data"] = new OpenApiNull(),
                    ["Message"] = new OpenApiString("No se encontró la cotización registrada"),
                    ["IsError"] = new OpenApiBoolean(true)
                }
            }
        }
    };
    
    operation.Responses["500"] = new OpenApiResponse
    {
        Description = "Error interno del servidor.",
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(TokenResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["Data"] = new OpenApiNull(),
                    ["Message"] = new OpenApiString("Error interno del servidor"),
                    ["IsError"] = new OpenApiBoolean(true)
                }
            }
        }
    };

    return operation;
});



app.MapGet("/api/Cotizacion/GetToken", async (HttpContext context) =>
{
    // Intentar obtener el header X-API-KEY (case-insensitive)
    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var providedKey))
    {
        // Intentar con minúsculas también
        if (!context.Request.Headers.TryGetValue("x-api-key", out providedKey))
        {
            return Results.Unauthorized();
        }
    }

    var apiKeyValue = providedKey.ToString().Trim();
    if (apiKeyValue != apiKey)
    {
        return Results.Unauthorized();
    }

    using var client = new HttpClient();
    // Asegurar que el header se envíe correctamente
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("X-API-KEY", apiKeyValue);
    var url = $"{apiBase}CotizacionEquipoContratistaApi/GetToken";

    try
    {
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Verificar el código de estado HTTP
        if (!response.IsSuccessStatusCode)
        {
            // Si la respuesta no es exitosa, devolver el error con el mismo código de estado
            return Results.Content(
                content,
                "application/json",
                statusCode: (int)response.StatusCode
            );
        }

        // Si el status es 200 pero el contenido indica un error, devolverlo también
        // Esto maneja el caso donde el servidor devuelve 200 pero con un error en el JSON
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
    operation.Responses["200"] = new OpenApiResponse
    {
        Description = "Token obtenido exitosamente.",
        Content =
        {
            ["application/json"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = nameof(TokenResponse)
                    }
                },
                Example = new OpenApiObject
                {
                    ["Data"] = new OpenApiObject
                    {
                        ["token"] = new OpenApiString("279e01c8-9d90-42f4-83ed-b558ed31342c")
                    },
                    ["Message"] = new OpenApiString(""),
                    ["IsError"] = new OpenApiBoolean(false)
                }
            }
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

public class SchemaRegistrationFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (!swaggerDoc.Components.Schemas.ContainsKey(nameof(TokenResponse)))
        {
            swaggerDoc.Components.Schemas.Add(nameof(TokenResponse), new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["Data"] = new OpenApiSchema { Type = "object", Nullable = true },
                    ["Message"] = new OpenApiSchema { Type = "string", Nullable = true },
                    ["IsError"] = new OpenApiSchema { Type = "boolean" }
                }
            });
        }

        if (!swaggerDoc.Components.Schemas.ContainsKey(nameof(CotizacionResponse)))
        {
            swaggerDoc.Components.Schemas.Add(nameof(CotizacionResponse), new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["Draw"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                    ["RecordsFiltered"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                    ["RecordsTotal"] = new OpenApiSchema { Type = "integer", Format = "int32" },
                    ["Data"] = new OpenApiSchema 
                    { 
                        Type = "object", 
                        Nullable = true,
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["Prima"] = new OpenApiSchema { Type = "string", Nullable = true },
                            ["Total"] = new OpenApiSchema { Type = "string", Nullable = true },
                            ["Couta"] = new OpenApiSchema { Type = "number", Format = "decimal", Nullable = true },
                            ["UrlFile"] = new OpenApiSchema { Type = "string", Nullable = true }
                        }
                    },
                    ["Message"] = new OpenApiSchema { Type = "string", Nullable = true },
                    ["IsError"] = new OpenApiSchema { Type = "boolean" },
                    ["ShowMessage"] = new OpenApiSchema { Type = "boolean" }
                }
            });
        }

        if (!swaggerDoc.Components.Schemas.ContainsKey(nameof(CotizacionEquipoContratistaModel)))
        {
            swaggerDoc.Components.Schemas.Add(nameof(CotizacionEquipoContratistaModel), new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["Cotizacion"] = new OpenApiSchema { Type = "object", Nullable = true },
                    ["EContratista"] = new OpenApiSchema { Type = "object", Nullable = true }
                }
            });
        }
    }
}
