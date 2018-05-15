using TokenAPI.Models;

namespace TokenAPI.Models
{
    public class UserToken
    {
        public string ApplicationUserId { get; set; }
        public int TokenId { get; set; }

        public virtual Token Token { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
