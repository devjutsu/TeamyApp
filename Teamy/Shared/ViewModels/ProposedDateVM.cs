using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.ViewModels
{
    public class ProposedDateVM : ICloneable
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateTo { get; set; }
        public Guid? EventId { get; set; }
        public List<DateVoteVM> Votes { get; set; } = new List<DateVoteVM>();

        public object Clone()
        {
            var item = (ProposedDateVM)this.MemberwiseClone();
            item.Votes = this.Votes?.ToList() ?? new List<DateVoteVM>();
            return item;
        }
    }

    public class DateVoteVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
    }
}
