using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ApiStatusController : ControllerBase
{    [HttpGet]
    public ContentResult Get()
    {
        var html = @"
        <html>
        <head>
            <title>Device Management API</title>
        </head>
        <body>
            <h1>Device Management API</h1>
            <p>A API est&aacute; funcionando corretamente!</p>
            <p>Data/Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + @"</p>
            <p><a href='/docs'>Documenta&#231;&#227;o da API</a></p>
        </body>
        </html>";

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
