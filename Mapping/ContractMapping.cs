using SurvayBacket.Api.Contracts.Polls;

namespace SurvayBacket.Api.Mapping
{
    public static class ContractMapping
    {
        #region Manual Mapping
        //public static PollResponse MapToRespones(this Poll poll)
        //{
        //    return new()
        //    {
        //        Id = poll.Id,
        //        Title = poll.Title,
        //        Summary = poll.Summary,
        //        EndAt  = poll.EndAt,
        //        StartAt= poll.StartAt,
        //        IsPublished=poll.IsPublished
        //    };
        //}

        //public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> polls)
        //{
        //    return polls.Select(MapToRespones);
        //}
        public static Poll MapToPoll(this PollRequest request)
        {
            return new()
            {
                Title = request.Title,
                Summary = request.Summary,
            };
        }
        #endregion
    }
}
