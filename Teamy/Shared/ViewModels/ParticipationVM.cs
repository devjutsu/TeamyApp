using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teamy.Shared.Common;

namespace Teamy.Shared.ViewModels
{
    public class ParticipationVM
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public string UserId { get; set; }
        public ParticipationStatus Status { get; set; }
        public string Name { get; set; }
    }
}
