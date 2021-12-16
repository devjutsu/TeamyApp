namespace Teamy.Server.Models
{
    public interface IDateCreated
    {
        DateTime DateCreated { get; set; }
    }
    
    public interface IDateUpdated
    {
        DateTime DateUpdated { get; set; }
    }

    public class CreatedEntity : IDateCreated
    {
        public DateTime DateCreated { get; set; }
    }

    public class BaseEntity : IDateCreated, IDateUpdated
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
