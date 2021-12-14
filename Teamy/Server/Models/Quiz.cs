namespace Teamy.Server.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public List<string> Questions { get; set; }
        public string? UserId { get; set; }
        public string QCode { get; set; }
    }
}
