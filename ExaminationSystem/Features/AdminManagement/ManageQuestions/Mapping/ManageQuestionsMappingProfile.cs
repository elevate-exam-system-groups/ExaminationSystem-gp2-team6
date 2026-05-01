using AutoMapper;
using ExaminationSystem.Domain.Entities.AnswerOption;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Dtos;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Mapping
{
    public class ManageQuestionsMappingProfile : Profile
    {
        public ManageQuestionsMappingProfile()
        {
            CreateMap<OptionDto, AnswerOption>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionId, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore());

            CreateMap<AddQuestionCommand, Question>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.AnswerOptions, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.OrderIndex, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Quiz, opt => opt.Ignore());
        }
    }
}
