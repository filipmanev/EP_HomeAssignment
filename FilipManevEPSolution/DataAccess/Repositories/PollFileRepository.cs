using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WebApplication1.DataAccess.Interfaces;
using WebApplication1.Domain.Models;

namespace WebApplication1.DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filePath = "polls.json";

        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = DateTime.UtcNow;
            var polls = GetPolls().ToList();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            polls.Add(poll);
            var json = JsonSerializer.Serialize(polls);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Poll> GetPolls()
        {
            if (!File.Exists(_filePath))
                return new List<Poll>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Poll>>(json) ?? new List<Poll>();
        }

        public void Vote(int pollId, int optionVoted, string userId)
        {
            var polls = GetPolls().ToList();
            var poll = polls.FirstOrDefault(p => p.Id == pollId);
            if (poll == null)
                throw new Exception("Poll not found");

            if (poll.VotedUserIds != null && poll.VotedUserIds.Contains(userId))
                throw new Exception("usser has already voted on this poll");

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
                    throw new Exception("invalid option.");
            }

            poll.VotedUserIds.Add(userId);

            var json = JsonSerializer.Serialize(polls);
            File.WriteAllText(_filePath, json);
        }

        public bool HasUserVoted(int pollId, string userId)
        {
            var polls = GetPolls();
            var poll = polls.FirstOrDefault(p => p.Id == pollId);
            if (poll == null)
                throw new Exception("poll not found.");

            return poll.VotedUserIds?.Contains(userId) ?? false;
        }
    }
}
