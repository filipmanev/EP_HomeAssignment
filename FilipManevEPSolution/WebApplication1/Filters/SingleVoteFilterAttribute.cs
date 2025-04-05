using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.DataAccess.Interfaces;

namespace WebApplication1.Presentation.Filters
{
    public class SingleVoteFilterAttribute : ActionFilterAttribute
    {
        private readonly IPollRepository _pollRepository;

        public SingleVoteFilterAttribute(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("pollId") && context.HttpContext.User.Identity.IsAuthenticated)
            {
                int pollId = (int)context.ActionArguments["pollId"];
                string userId = context.HttpContext.User.Identity.Name;

                if (_pollRepository.HasUserVoted(pollId, userId))
                {
                    context.Result = new RedirectToActionResult("Details", "Poll", new { id = pollId });
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
