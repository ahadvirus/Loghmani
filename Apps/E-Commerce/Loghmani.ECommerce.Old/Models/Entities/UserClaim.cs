namespace Loghmani.ECommerce.Old.Models.Entities
{
    public class UserClaim
    {
        public UserClaim()
        {
            Key = string.Empty;
            Value = string.Empty;
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }
    }
}
