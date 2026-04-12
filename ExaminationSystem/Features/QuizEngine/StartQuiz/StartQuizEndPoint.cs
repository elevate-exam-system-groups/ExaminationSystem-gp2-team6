using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.CreateQuiz
{
    [ApiController]
    [Route("api/[controller]")]
    public class StartQuizEndPoint:ControllerBase
    {
        //[HttpPost]
        //public async StartQuizResponseViewModel StartQuiz([FromBody] StartQuizRequestViewModel request, [FromServices] IMediator mediator)
        //{
        //    //var quizId = await mediator.Send(command);
        //    throw new NotImplementedException();
        //    //return Ok(quizId);
        //}
    }
}
