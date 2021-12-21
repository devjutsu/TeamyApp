using System;
using AutoMapper;
using Teamy.Server.Models;
using Teamy.Server.Models.Quizes;
using Teamy.Server.Models.Polls;
using Teamy.Server.Models.Templates;
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

            CreateMap<PollAnswer, PollAnswerVM>()
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
                .ForMember(dest => dest.InviteCode, opt => opt.MapFrom(src => src.Invites.First(o => o.Public).InviteCode))
                .ForMember(dest => dest.ProposedDates, opt => opt.MapFrom(src => src.ProposedDates))
                .ReverseMap();

            CreateMap<TemplatePollChoice, PollChoiceVM>()
                .ForMember(dest => dest.Choice, opt => opt.MapFrom(src => src.Choice));

            CreateMap<TemplatePoll, PollVM>()
                .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

            CreateMap<List<TemplatePollChoice>, List<PollChoiceVM>>();

            CreateMap<Template, EventVM>()
                .ForMember(dest => dest.Polls, opt => opt.MapFrom(src => src.Polls))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.CoverImage.Url));

            CreateMap<TemplatePoll, Poll>()
                .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

            CreateMap<TemplatePollChoice, PollChoice>()
                .ForMember(dest => dest.Choice, opt => opt.MapFrom(src => src.Choice));

            CreateMap<Template, Event>()
                .ForMember(dest => dest.Polls, opt => opt.MapFrom(src => src.Polls));

            CreateMap<ProposedDate, ProposedDateVM>()
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes))
                .ReverseMap();

            CreateMap<DateVote, DateVoteVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.DisplayName))
                .ReverseMap();

            CreateMap<ChatMessage, ChatMessageVM>()
                .ForMember(dest => dest.SentBy, opt => opt.MapFrom(src => src.SentBy.DisplayName))
                .ReverseMap();

            CreateMap<Quiz, QuizVM>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ReverseMap();
            CreateMap<QuizQuestion, QuizQuestionVM>().ReverseMap();
            CreateMap<QuizChoice, QuizChoiceVM>().ReverseMap();
            CreateMap<QuizAnswer, QuizAnswerVM>().ReverseMap();
            CreateMap<QCode, QCodeVM>().ReverseMap();

        }
    }
}
