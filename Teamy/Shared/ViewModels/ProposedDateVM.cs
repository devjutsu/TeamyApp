using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.ViewModels
{
    public record ProposedDateVM
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateTo { get; set; }
        public Guid? EventId { get; set; }
        public List<DateVoteVM> Votes { get; set; } = new List<DateVoteVM>();
    }

    public class DateVoteVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
    }
}
