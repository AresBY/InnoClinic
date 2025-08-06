namespace InnoClinic.Profiles.Domain.Entities
{
    public abstract class UserBaseProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OwnerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
    }
}
