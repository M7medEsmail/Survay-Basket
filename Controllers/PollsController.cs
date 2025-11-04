using System.Threading.Tasks;

namespace SurvayBacket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // same behavior of [from body]  & Prevent requset to compelete if attribute is requird
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
            return poll is null ? NotFound() : Ok(poll.Adapt<PollResponse>());
        }

        [HttpPost("create")]
        public async Task <IActionResult> Create([FromBody] PollRequest pollrequest ,CancellationToken cancellationToken)
        {
            var NewPoll =await _pollService.CreateAsync(pollrequest.Adapt<Poll>(), cancellationToken);
            //return CreatedAtAction(nameof(GetByIdAsync), new { id = NewPoll.Id }, NewPoll);
            return Ok(NewPoll);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PollRequest pollrequest ,CancellationToken cancellationToken)
        {
            var isUpdated =await _pollService.UpdateAsync(id, pollrequest.Adapt<Poll>() , cancellationToken);
            if (!isUpdated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var IsDeleted =await _pollService.DeleteAsync(id ,cancellationToken);
            if (!IsDeleted)
                return NotFound();
            return NoContent();
        }
         
    }
}
