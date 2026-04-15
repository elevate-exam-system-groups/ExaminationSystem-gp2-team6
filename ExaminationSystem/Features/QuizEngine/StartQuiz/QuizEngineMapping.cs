using AutoMapper;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz
{
    public class QuizEngineMapping : Profile
    {
        public QuizEngineMapping()
        {
            #region Mapping StartQuiz 
            CreateMap<Question, QuestionDto>().ForMember(dest => dest.Options,opt => opt.MapFrom(src => src.AnswerOptions.Select(o => new OptionDto{Id = o.Id,Text = o.Text})));
            CreateMap<QuestionDto,QuestionViewModel>();
            CreateMap<QuestionDto, StartQuizResponseViewModel>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Options.Select(o => new QuestionViewModel { Id = o.Id, Text = o.Text })))
            #endregion
        }
    }
}
