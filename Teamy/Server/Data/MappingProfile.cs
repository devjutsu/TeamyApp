using System;
using AutoMapper;
using Teamy.Server.Models;
using Teamy.Shared.ViewModels;

namespace Teamy.Server.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Poll, PollVM>()
                .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices))
                .ReverseMap();

            CreateMap<PollChoice, PollChoiceVM>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ReverseMap();

            CreateMap<ImageModel, UploadVM>()
                .ReverseMap();

            CreateMap<Participation, ParticipationVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.DisplayName))
                .ReverseMap();

            CreateMap<Event, EventVM>()
                .ForMember(dest => dest.Polls, opt => opt.MapFrom(src => src.Polls))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.CoverImage.Url))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.DisplayName))
                .ReverseMap();

            CreateMap<TemplatePollChoice, PollChoiceVM>()
                .ForMember(dest => dest.Choice, opt => opt.MapFrom(src => src.Choice))
                .ReverseMap();

            CreateMap<TemplatePoll, PollVM>()
                .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices))
                .ReverseMap();

            CreateMap<List<TemplatePollChoice>, List<PollChoiceVM>>()
                .ReverseMap();

            CreateMap<Template, EventVM>()
                .ForMember(dest => dest.Polls, opt => opt.MapFrom(src => src.Polls))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.CoverImage.Url))
                .ReverseMap();

            CreateMap<TemplatePoll, Poll>()
                .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices))
                .ReverseMap();

            CreateMap<TemplatePollChoice, PollChoice>()
                .ForMember(dest => dest.Choice, opt => opt.MapFrom(src => src.Choice))
                .ReverseMap();

            CreateMap<Template, Event>()
                .ForMember(dest => dest.Polls, opt => opt.MapFrom(src => src.Polls))
                .ReverseMap();
                
        }
    }
}
