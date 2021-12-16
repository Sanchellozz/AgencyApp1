namespace AgencyApp.Models
{
    public class Client : Human
    {
        public int? LicenseId { get; set; }
        public string Passport { get; set; }
        public virtual License? License { get; set; }
        public virtual User? User { get; set; }
    }
}
