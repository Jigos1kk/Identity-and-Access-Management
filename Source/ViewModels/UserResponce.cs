using System;

namespace Source.ViewModels;

public class UserResponce
{
    public UserResponce(Guid uuid, string? firstName, string? lastName)
    {
        Uuid = uuid;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Uuid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
