using Teamy.Shared.ViewModels;

namespace Teamy.Client.Services
{
    public interface IManagePolls
    {
        public PollVM NewPoll();
    }

    public class PollService : IManagePolls
    {
        public PollVM NewPoll()
            => new PollVM()
            {
                Choices = new List<PollChoiceVM>()
                {
                    new PollChoiceVM()
                    {
                        Choice = "Type poll choice text",
                        Answers = new List<PollAnswerVM>()
                    }
                },
                Question = "Type Question",
                MultiChoice = true,
                FreeTextOption = true
            };
    }
}
