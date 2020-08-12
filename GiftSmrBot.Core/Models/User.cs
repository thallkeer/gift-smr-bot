namespace GiftSmrBot.Core.Models
{
    public class User
    {
        public User()
        {

        }
        public User(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
