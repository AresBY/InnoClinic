namespace InnoClinic.Profiles.Domain.Entities
{
    public sealed class PatientProfile : UserBaseProfile
    {
        public string PhoneNumber { get; set; } = null!;
        public DateTimeOffset DateOfBirth { get; set; }
        public bool IsLinkedToAccount { get; set; }

    }
}
