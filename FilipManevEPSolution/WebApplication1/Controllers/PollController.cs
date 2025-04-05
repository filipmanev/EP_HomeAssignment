using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication1.DataAccess.Interfaces;
using WebApplication1.Domain.Models;
using WebApplication1.Presentation.Filters;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class PollController : Controller
    {
        private readonly IPollRepository _pollRepository;

        public PollController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var polls = _pollRepository.GetPolls();
            var sortedPolls = polls.OrderByDescending(p => p.DateCreated);
            return View(sortedPolls);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Poll poll)
        {
            if (ModelState.IsValid)
            {
                _pollRepository.CreatePoll(poll);
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var polls = _pollRepository.GetPolls();
            var poll = polls.FirstOrDefault(p => p.Id == id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SingleVoteFilterAttribute))]
        public IActionResult Vote(int pollId, int optionVoted)
        {
            var userId = User.Identity?.Name;
            _pollRepository.Vote(pollId, optionVoted, userId);
            return RedirectToAction(nameof(Details), new { id = pollId });
        }
    }
}
