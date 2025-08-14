namespace InnoClinic.Profiles.Domain.Entities
{
    public class ReceptionistProfile : UserBaseProfile
    {
        public Guid? OwnerId { get; set; }
        public Guid OfficeId { get; set; }
    }
}
