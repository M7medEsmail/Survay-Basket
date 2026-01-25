using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;

        [HttpGet("row-data")]
        public async Task<IActionResult> GetResults([FromRoute]int pollId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetPollVoteAsync(pollId, cancellationToken);
            return results.IsSuccess ? Ok(results.Value) : results.ToProblem();
        }

        [HttpGet("Vote-per-day")]
        public async Task<IActionResult> GetVotePerDay([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetVotePerDay(pollId, cancellationToken);
            return results.IsSuccess ? Ok(results.Value) : results.ToProblem();
        }

        [HttpGet("Vote-per-Question")]
        public async Task<IActionResult> GetVotePerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var results = await _resultService.GetVotePerQuestion(pollId, cancellationToken);
            return results.IsSuccess ? Ok(results.Value) : results.ToProblem();
        }
    }
}
 