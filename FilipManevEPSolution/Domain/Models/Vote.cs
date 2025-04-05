namespace WebApplication1.Domain.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string UserId { get; set; } 
        public int OptionVoted { get; set; } 
    }
}
