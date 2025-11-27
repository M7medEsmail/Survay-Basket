using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Question;
using System.Threading;
using System.Threading.Tasks;

namespace SurvayBacket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken) 
        {
            var question =await _questionService.GetByIdAsync(pollId, id, cancellationToken);
    
            return question.IsSuccess ? Ok(question.Value) : question.ToProblem(StatusCodes.Status400BadRequest);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromRoute]int pollId, CancellationToken cancellationToken) {
           
            var result =await _questionService.GetAll(pollId ,cancellationToken); 

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status400BadRequest);

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute]int pollId,[FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(pollId, questionRequest, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.ToProblem(StatusCodes.Status400BadRequest);
        }

        [HttpPut("{id}/toggleStatus")]
            public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);
            if (result.IsSuccess)
                return NoContent();
            return result.ToProblem(StatusCodes.Status400BadRequest);
        }

    }
}
