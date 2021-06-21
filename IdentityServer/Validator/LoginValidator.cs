using FluentValidation;
using IdentityServer.ViewsModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Validator
{
    public class LoginValidator : AbstractValidator<UserLoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().WithMessage("Un Username doit être rempli");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Un mot de passe doit être rempli");
        }
    }
}
