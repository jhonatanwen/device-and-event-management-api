using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("docs")]
public sealed class DocsController(IWebHostEnvironment environment) : ControllerBase
{
    private readonly IWebHostEnvironment _environment = environment;

    [HttpGet]
    public async Task<IActionResult> GetDocs()
    {
        try
        {
            // Carregar o conteúdo YAML diretamente
            var swaggerYaml = await LoadSwaggerYamlContent();

            if (swaggerYaml == null)
            {
                return NotFound("Arquivo swagger.yaml não encontrado");
            }

            var formattedSwaggerYaml = swaggerYaml.Replace("\\", "\\\\").Replace("`", "\\`").Replace("$", "\\$");

            var html = $@"
            <!DOCTYPE html>
            <html lang='pt-br'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Device Management API - Documentação</title>
                <link rel='stylesheet' type='text/css' href='https://unpkg.com/swagger-ui-dist@4.15.5/swagger-ui.css' />
            </head>
            <body>
                <div id='swagger-ui'></div>

                <script src='https://unpkg.com/swagger-ui-dist@4.15.5/swagger-ui-bundle.js'></script>
                <script src='https://unpkg.com/swagger-ui-dist@4.15.5/swagger-ui-standalone-preset.js'></script>
                <script>
                    window.onload = function() {{
                        const yamlSpec = `{formattedSwaggerYaml}`;

                        const ui = SwaggerUIBundle({{
                            spec: jsyaml.load(yamlSpec),
                            dom_id: '#swagger-ui',
                            deepLinking: true,
                            presets: [
                                SwaggerUIBundle.presets.apis,
                                SwaggerUIStandalonePreset
                            ],
                            plugins: [
                                SwaggerUIBundle.plugins.DownloadUrl
                            ],
                            layout: 'StandaloneLayout'
                        }});
                    }};
                </script>
                <script src='https://unpkg.com/js-yaml@4.1.0/dist/js-yaml.min.js'></script>
            </body>
            </html>";

            return Content(html, "text/html");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao carregar documentação: {ex.Message}");
        }
    }

    private async Task<string?> LoadSwaggerYamlContent()
    {
        // Tentar primeiro no caminho relativo à raiz do projeto
        var yamlPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "docs", "swagger.yaml");

        if (!System.IO.File.Exists(yamlPath))
        {
            // Tentar no caminho absoluto baseado no ambiente
            yamlPath = Path.Combine(_environment.ContentRootPath, "..", "docs", "swagger.yaml");

            if (!System.IO.File.Exists(yamlPath))
            {
                return null;
            }
        }

        return await System.IO.File.ReadAllTextAsync(yamlPath);
    }
}