namespace Dreamer.Domain.DTOs
{
    public class UserView
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required DateTime DateCreated { get; set; }
        public required DateTime DateModified { get; set; }
    }
}
