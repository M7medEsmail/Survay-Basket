using Microsoft.AspNetCore.Authorization;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Polls;
using SurvayBacket.Api.Entities;
using System.Threading.Tasks;

namespace SurvayBacket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // same behavior of [from body]  & Prevent requset to compelete if attribute is requird
    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService; //Using Primary Constractor
      
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            var reponse = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(reponse);
        }   

        [HttpGet("{id:int}")]
        public async Task <IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var poll =await _pollService.GetByIdAsync(id, cancellationToken);
            return poll.IsFailure? NotFound(poll.Error) : Ok(poll.Value);
        }

        [HttpPost("create")]
        public async Task <IActionResult> Create([FromBody] PollRequest pollrequest ,CancellationToken cancellationToken)
        {
            //var poll = new Poll
            //{
            //    Title = pollrequest.Title,
            //    Summary = pollrequest.Summary,
            //    StartAt = pollrequest.StartAt,
            //    EndAt = pollrequest.EndAt,
            //    IsPublished = pollrequest.IsPublished
            //};

            //var NewPoll =await _pollService.CreateAsync(poll, cancellationToken);
            var NewPoll =await _pollService.CreateAsync(pollrequest, cancellationToken);
            return NewPoll.IsSuccess? Ok(NewPoll.Value)
                : NewPoll.ToProblem(StatusCodes.Status409Conflict);  

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PollRequest pollrequest ,CancellationToken cancellationToken)
        {
            var result =await _pollService.UpdateAsync(id, pollrequest , cancellationToken);
           
            if (result.IsFailure)
                return result.ToProblem(StatusCodes.Status409Conflict);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result =await _pollService.DeleteAsync(id ,cancellationToken);
            if (result.IsFailure)
                return NotFound(result.Error);
            return NoContent();
        }
         
    }
}
