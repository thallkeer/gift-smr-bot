using System.ComponentModel.DataAnnotations;

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

        [Key]
        public long Id { get; set; }
    }
}
