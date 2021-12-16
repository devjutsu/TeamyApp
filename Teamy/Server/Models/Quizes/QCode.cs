namespace Teamy.Server.Models.Quizes
{
    public class QCode
    {
        public string Id { get; set; }
        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
