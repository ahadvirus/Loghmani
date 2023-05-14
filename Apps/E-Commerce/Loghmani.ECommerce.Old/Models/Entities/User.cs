namespace Loghmani.ECommerce.Old.Models.Entities;

public class User
{

    public User()
    {
        Username = string.Empty;
        Password = string.Empty;
    }
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }

}