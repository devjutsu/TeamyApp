using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teamy.Shared.Common;

namespace Teamy.Shared.ViewModels
{
    public class EventVM : ICloneable
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public string? Where { get; set; }
        public List<PollVM>? Polls { get; set; }
        public List<ParticipationVM>? Participants { get; set; }
        public string? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public string? ImageUrl { get; set; }
        public string? InviteCode { get; set; }
        public List<ProposedDateVM>? ProposedDates { get; set; }
        public EventDateStatus DateStatus { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? EventDateTo { get; set; }

        public object Clone()
        {
            var item = (EventVM)this.MemberwiseClone();
            item.ProposedDates = this.ProposedDates?.ToList() ?? new List<ProposedDateVM>();
            item.Polls = this.Polls?.ToList() ?? new List<PollVM>();
            item.Participants = this.Participants?.ToList() ?? new List<ParticipationVM>();
            return item;
        }
    }
}
