using AutoMapper;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.ViewModel;

namespace ExaminationSystem.Common.Mapping
{
    public class AdminMapping : Profile
    {
        public AdminMapping()
        {
            CreateMap<CreateQuizCommand, Quiz>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromMinutes(src.DurationMinutes)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => QuizStatus.Draft));

            CreateMap<UpdateQuizCommand, Quiz>()
                .ForMember(dest => dest.Duration, opt =>
                {
                    opt.Condition(src => src.DurationMinutes.HasValue);
                    opt.MapFrom(src => TimeSpan.FromMinutes(src.DurationMinutes.Value));
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Quiz, UpdateQuizResponseViewModel>()
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => (int)src.Duration.TotalMinutes))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString().ToLower()));
        }
    }
}
