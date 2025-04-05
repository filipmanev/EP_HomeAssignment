using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DataAccess.DataContext;
using WebApplication1.DataAccess.Interfaces;
using WebApplication1.Domain.Models;

namespace WebApplication1.DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = DateTime.UtcNow;
            _context.Polls.Add(poll);
            _context.SaveChanges();
        }

        public IEnumerable<Poll> GetPolls()
        {
            return _context.Polls.ToList();
        }

        public void Vote(int pollId, int optionVoted, string userId)
        {
            var poll = _context.Polls.Find(pollId);
            if (poll == null)
                throw new Exception("poll not found");

            switch (optionVoted)
            {
                case 1:
                    poll.Option1VotesCount++;
                    break;
                case 2:
                    poll.Option2VotesCount++;
                    break;
                case 3:
                    poll.Option3VotesCount++;
                    break;
                default:
                    throw new Exception("nvalid option");
            }

            var vote = new Vote { PollId = pollId, OptionVoted = optionVoted, UserId = userId };
            _context.Votes.Add(vote);

            _context.SaveChanges();
        }

        public bool HasUserVoted(int pollId, string userId)
        {
            return _context.Votes.Any(v => v.PollId == pollId && v.UserId == userId);
        }
    }
}
