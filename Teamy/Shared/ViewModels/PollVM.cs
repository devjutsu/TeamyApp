using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.ViewModels
{
    public class PollVM
    {
        public Guid? Id { get; set; }
        public Guid? EventId { get; set; }
        public string Question { get; set; }
        public List<PollChoiceVM> Choices { get; set; }
        public bool MultiChoice { get; set; }
        public bool FreeTextOption { get; set; }
    }

    public class PollChoiceVM
    {
        public Guid? Id { get; set; }
        public string Choice { get; set; }
        public Guid? PollId { get; set; }
        public List<PollAnswerVM>? Answers { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class PollAnswerVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
