using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CommentsApp.Api.Data.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [EmailAddress, StringLength(100)] public required string Email { get; set; }
    [StringLength(25)] public required string Username { get; set; }
    [StringLength(64, MinimumLength = 64)] public required string PasswordHash { get; set; }
    [StringLength(25)] public required string VisibleName { get; set; }
    [Url, StringLength(100)] public string? HomePageUrl { get; set; }
}