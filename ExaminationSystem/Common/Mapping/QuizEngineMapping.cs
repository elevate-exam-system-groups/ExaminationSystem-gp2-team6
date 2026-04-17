using AutoMapper;
using ExaminationSystem.Domain.Entities.AttemptAnswer;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Commands;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;

namespace ExaminationSystem.Common.Mapping
{
    public class QuizEngineMapping : Profile
    {
        public QuizEngineMapping()
        {
            #region Mapping StartQuiz                 
            CreateMap<Question, QuestionDto>()            
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.AnswerOptions.Select(o => new OptionDto { Id = o.Id, Text = o.Text })));   
            
            CreateMap<QuestionDto, QuestionViewModel>();

            CreateMap<QuestionDto, StartQuizResponseViewModel>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Options.Select(o => new QuestionViewModel { Id = o.Id, Text = o.Text })));
            #endregion

            #region Mapping AnswerQuiz
                
            CreateMap<QuizAttempt, QuizAttemptDto>().ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.Id));

                
            CreateMap<AnswerQuestionCommand, AttemptAnswer>().ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.SelectedOptionId, opt => opt.MapFrom(src => src.OptionId)).ReverseMap();

            #endregion
        }
    }
}
