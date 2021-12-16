namespace AgencyApp.Models
{
    public class Agent : Human
    {
        public int DegreeId { get; set; }
        public virtual Degree? Degree { get; set; }
        public virtual User? User { get; set; }
    }
}
