using System.ComponentModel.DataAnnotations;

namespace CommentsApp.Api.Shared.Requests;

public class RegisterRequest
{
    [EmailAddress] public required string Email { get; set; }
    [StringLength(25)] public required string Username { get; set; }
    [StringLength(64, MinimumLength = 64)] public required string PasswordHash { get; set; }
    [StringLength(25)] public required string VisibleName { get; set; }
    [Url, StringLength(100)] public string? HomePageUrl { get; set; }
}