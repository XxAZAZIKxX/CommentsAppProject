using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommentsApp.Api.Data.Models;

[Index(nameof(ParentCommentId))]
public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public User Author { get; set; }
    [ForeignKey(nameof(Author))] public Guid AuthorId { get; set; }
    public string Text { get; set; }
    public Comment? ParentComment { get; set; }
    [ForeignKey(nameof(ParentComment))] public Guid? ParentCommentId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? FileUrl { get; set; }
}