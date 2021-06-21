using FluentValidation;
using IdentityServer.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IdentityServer.Validator
{
    public class UserRegisterValidator:AbstractValidator<UserRegisterViewModel>
    {
        AllValidation oValidation = new AllValidation();
        public UserRegisterValidator()
        {
            RuleFor(x => x.User.Nom)
                .NotNull().WithMessage("le champ 'Nom' est obligatoire")
                .MaximumLength(50)
                .Matches(@"^[a-zA-Z""'\s]*$");

            RuleFor(x => x.User.Prenom)
                .NotNull().WithMessage("Le champ 'Prénom'est obligatoire")
                .MaximumLength(50)
                .Matches(@"^[a-zA-Z""'\s]*$");

            RuleFor(x => x.User.Email)
                .NotNull().WithMessage("Le champ 'Email' est obligatoire")
                .MaximumLength(255)
                .EmailAddress().WithMessage("L'adresse mail doit être valide");

            RuleFor(x => x.User.PhoneNumber)
                .MaximumLength(25) 
                .Must(oValidation.ValidationNumTel).WithMessage("Le format du n° de téléphone mobile n'est pas valide ou n'est pas Belge")
                .When(x => !string.IsNullOrEmpty(x.User.PhoneNumber));

            RuleFor(x => x.User.Sexe)
                .NotNull().WithMessage("Le champ 'Genre' est obligatoire");

            RuleFor(x => x.User.DateNaissance)
                .NotNull().WithMessage("Le champ 'Date de Naissance' est obligatoire");

            RuleFor(x => x.User.Professionel)
                .NotNull().WithMessage("Le champ 'Professionel' est obligatoire");
            RuleFor(x => x.Password)
                .NotNull().WithMessage("Le mot de passe doit être rempli")
                .MinimumLength(6).WithMessage("Le mot de passe doit être composé de 6 caractères minimum")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$").WithMessage("Le mot de passe doit contenir au moins : une miniscule, une majuscule, un chiffre et un caractère spécial");

            RuleFor(x => x.ConfirmPassword)
               .NotNull().WithMessage("Le mot de passe de confirmation doit être rempli")
               .Equal(x => x.Password).WithMessage("La confirmation du mot de passe n'est pas identique");

        }
    }
}
