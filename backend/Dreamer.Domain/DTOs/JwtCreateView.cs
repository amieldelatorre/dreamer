using Unleash.Strategies;

namespace Dreamer.Domain.DTOs;

public class JwtCreateView
{
    public required Guid Id {  get; set; }
    public required Guid UserId { get; set; }
    public required string AccessToken { get; set; }
    public required DateTime ExpiryDate { get; set; }
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
    public Boolean IsDisabled { get; set; }
    public DateTime? DateDisabled { get; set; }
}
