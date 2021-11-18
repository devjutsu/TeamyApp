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
        public Guid? EventId { get; set; }
    }
}
