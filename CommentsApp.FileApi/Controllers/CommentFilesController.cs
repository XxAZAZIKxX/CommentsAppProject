using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace CommentsApp.FileApi.Controllers;

[ApiController, Route("files/comments/{commentId:guid}/{fileId:guid}.{extension}")]
public class CommentFilesController : ControllerBase
{
    [HttpGet, ResponseCache(Duration = 60*60*24*31)]
    public async Task<IActionResult> GetFileAsunc(Guid commentId, Guid fileId, string extension)
    {
        return await Task.Run<IActionResult>(() =>
        {
            var fileInfo = new FileInfo($"files/comments/{commentId}/{fileId}.{extension}");
            if (fileInfo.Exists is false) return NotFound("File not found");
            var fileStream = fileInfo.OpenRead();

            var contentType = extension switch
            {
                "png" => MediaTypeNames.Image.Png,
                "jpg" => MediaTypeNames.Image.Jpeg,
                "jpeg" => MediaTypeNames.Image.Jpeg,
                "gif" => MediaTypeNames.Image.Gif,
                "txt" => MediaTypeNames.Text.Plain,
                _ => MediaTypeNames.Application.Octet
            };

            return File(fileStream, contentType);
        });
    }
}