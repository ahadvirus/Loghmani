using System.Collections.Generic;

namespace Loghmani.ECommerce.Old.Areas.Auth.Models.DataTransfers.Login;

public class UserDTO
{
    public UserDTO()
    {
        Name = string.Empty;
        Family = string.Empty;
        Roles = new List<string>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public List<string> Roles { get; set; }
}