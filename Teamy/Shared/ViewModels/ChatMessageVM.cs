using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.ViewModels
{
    public record ChatMessageVM
    {
        public Guid? Id { get; set; }
        public string? SentBy { get; set; }
        public DateTime? SentAt { get; set; }
        public string Text { get; set; }
        public Guid EventId { get; set; }
    }
}
