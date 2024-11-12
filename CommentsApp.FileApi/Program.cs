using System.Net.Mime;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddResponseCaching(options =>
{
    options.SizeLimit = 500 * 1024 * 1024;
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = [
        MediaTypeNames.Application.Octet,
        MediaTypeNames.Image.Png,
        MediaTypeNames.Image.Jpeg,
        MediaTypeNames.Image.Gif,
        MediaTypeNames.Text.Plain
    ];
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithMethods("GET").AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseResponseCompression();
app.UseResponseCaching();

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors();
app.MapControllers();

app.Run();
