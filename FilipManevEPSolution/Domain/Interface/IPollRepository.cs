using System.Collections.Generic;
using WebApplication1.Domain.Models;

namespace WebApplication1.DataAccess.Interfaces
{
    public interface IPollRepository
    {
        void CreatePoll(Poll poll);
        IEnumerable<Poll> GetPolls();
        void Vote(int pollId, int optionVoted, string userId);
        bool HasUserVoted(int pollId, string userId);
    }
}
