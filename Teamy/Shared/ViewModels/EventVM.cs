﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.ViewModels
{
    public class EventVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime When { get; set; }
        public string? Where { get; set; }
        public List<PollVM> Polls { get; set; }
        public List<ParticipationVM> Participants { get; set; }
        public string CreatedById { get; set; }
        public string ImageUrl { get; set; }
    }
}
