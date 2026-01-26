
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SurvayBacket.Api.Helper;
using System;

namespace SurvayBacket.Api.Services
{
    public class NotificationService(
        ApplicationDbContext context ,
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor


        ) : INotificationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task SendNewPollNotification(int? poolId = null)
        {
            IEnumerable<Poll> polls = [];

            if (poolId.HasValue)
            {
                var poll =await _context.Polls.SingleAsync(p=> p.Id == poolId && p.IsPublished);

                polls = [poll];
            }
            else
            {
                polls = await _context.Polls
                    .Where(p => p.IsPublished)
                    .AsNoTracking()
                    .ToListAsync();
            }

            var users = await _userManager.Users.ToListAsync();

            foreach (var polll in polls)
            {
                foreach (var user in users)
                {
                    var placeHolder = new Dictionary<string, string>
                    {
                        { "{{name}}",user.FirstName},
                        { "{{pollTitle}}",polll.Title},
                        { "{{endDate}}",polll.EndAt.ToString()}
                    };


                    var body = EmailBodyBuilder.GenerateEmailBody("NewPollNotification", placeHolder);
                    await _emailSender.SendEmailAsync(user.Email, "New Poll Published!", body);

                   }

            }
        }
    }
}
