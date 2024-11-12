namespace CommentsApp.Api.Shared.Responses;

public class TokenResponse
{
    public required Guid UserId { get; set; }
    public required string BearerToken { get; set; }
}