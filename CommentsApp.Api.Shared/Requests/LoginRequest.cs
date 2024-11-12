using System.ComponentModel.DataAnnotations;

namespace CommentsApp.Api.Shared.Requests;

public class LoginRequest
{
    [StringLength(25)] public required string Username { get; set; }
    [StringLength(64, MinimumLength = 64)] public required string PasswordHash { get; set; }
}