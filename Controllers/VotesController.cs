using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SurvayBacket.Api.Abstractions;
using SurvayBacket.Api.Contracts.Votes;
using SurvayBacket.Api.Entities;
using SurvayBacket.Api.Errors;
using SurvayBacket.Api.Extensions;

namespace SurvayBacket.Api.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionService QuestionService , IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _QuestionService = QuestionService;
        private readonly IVoteService _voteService = voteService;

        [HttpGet]
        [OutputCache(PolicyName = "OutPutCache")]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var votes = await _QuestionService.GetAvailable(pollId, userId!, cancellationToken);
            return votes.IsSuccess ? Ok(votes.Value) : votes.ToProblem();


        }

        [HttpPost]

        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest voteRequests, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _voteService.VoteAsync(pollId, userId!, voteRequests, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();

        }
    }
}
