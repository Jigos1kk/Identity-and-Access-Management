using System;
using Source.Models.Entities;

namespace Source.ViewModels;

public class UserResponce
{
    public UserResponce(User user)
    {
        Uuid = user.Uuid;
        UserName = user.UserName;
        FirstName = user.FirstName;
        LastName = user.FirstName;
        Email = user.Email;
    }

    public Guid Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

}
