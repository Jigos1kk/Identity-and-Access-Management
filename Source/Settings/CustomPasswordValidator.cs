using System;
using Microsoft.AspNetCore.Identity;
using Source.Service;

namespace Source.Settings;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    private readonly IJsonLocalizer _localizer;

    public CustomIdentityErrorDescriber(IJsonLocalizer localizer)
    {
        _localizer = localizer;
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = _localizer[nameof(PasswordRequiresDigit)] 
                ?? base.PasswordRequiresDigit().Description
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = _localizer[nameof(PasswordRequiresLower)] 
                ?? base.PasswordRequiresLower().Description
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = _localizer[nameof(PasswordRequiresUpper)] 
                ?? base.PasswordRequiresUpper().Description
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = _localizer[nameof(PasswordRequiresNonAlphanumeric)] 
                ?? base.PasswordRequiresNonAlphanumeric().Description
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = _localizer[nameof(PasswordTooShort), length] 
                ?? base.PasswordTooShort(length).Description
        };
    }

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUniqueChars),
            Description = _localizer[nameof(PasswordRequiresUniqueChars), uniqueChars] 
                ?? base.PasswordRequiresUniqueChars(uniqueChars).Description
        };
    }

    public override IdentityError DefaultError()
    {
        return new IdentityError
        {
            Code = nameof(DefaultError),
            Description = _localizer[nameof(DefaultError)] 
                ?? base.DefaultError().Description
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = _localizer[nameof(DuplicateEmail), email] 
                ?? base.DuplicateEmail(email).Description
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = _localizer[nameof(DuplicateUserName), userName] 
                ?? base.DuplicateUserName(userName).Description
        };
    }

    public override IdentityError InvalidEmail(string? email)
    {
        return new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = _localizer[nameof(InvalidEmail), email ?? ""] 
                ?? base.InvalidEmail(email).Description
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = _localizer[nameof(PasswordMismatch)] 
                ?? base.PasswordMismatch().Description
        };
    }

    public override IdentityError InvalidUserName(string? userName)
    {
        return new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = _localizer[nameof(InvalidUserName), userName ?? ""] 
                ?? base.InvalidUserName(userName).Description
        };
    }
}
