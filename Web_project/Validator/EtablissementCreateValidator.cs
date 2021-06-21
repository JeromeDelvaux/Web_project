using FluentValidation;
using MesModels.Models;
using MesModels.VatLayer;
using Web_project.Models.Interfaces;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Web_project.ViewsModels;

namespace Web_project.Validator
{
    public class EtablissementCreateValidator : AbstractValidator<EtablissementCreateViewModel>
    {
        AllValidation oValidation = new AllValidation();
        private readonly IVatLayerRepository vatValidator;

        public EtablissementCreateValidator(IVatLayerRepository vatValidator)
        {
            RuleForEach(x => x.Photos).SetValidator(new PhotosValidator());
            RuleFor(x => x.Logo).SetValidator(new LogoValidator());

            this.vatValidator = vatValidator;

            RuleFor(x => x.etablissement.TypeEtab)
                .NotNull().WithMessage("Le type d'établissement est obligatoire");
            RuleFor(x => x.etablissement.Nom)
                .NotNull().WithMessage("Le nom de votre établissement est obligatoire")
                .MaximumLength(50).WithMessage("Maximum 50 caractères pour définir le nom");

            RuleFor(x => x.etablissement.NumeroTva)
                .NotNull().WithMessage("Le numéro de TVA est obligatoire")
                .MustAsync(NumTvaValide).WithMessage("Le numéro de TVA doit être valide");

            RuleFor(x => x.etablissement.AdresseEmailPro)
                .NotNull().WithMessage("L'adresse mail professionnelle est obligatoire")
                .EmailAddress().WithMessage("L'adresse mail doit être valide")
                .MaximumLength(255).WithMessage("Maximum 255 charactères pour définir l'adresse mail");

            RuleFor(x => x.etablissement.ZoneTexteLibre)
                .NotNull().WithMessage("Une déscription est obligatoire")
                .MaximumLength(2000).WithMessage("Maximum 2000 charactères pour définir la zone de texte libre");

            RuleFor(x => x.etablissement.CodePostal)
                .NotNull().WithMessage("Le code postal est obligatoire")
                .MaximumLength(20).WithMessage("Maximum 20 caractères pour définir le code postal");

            RuleFor(x => x.etablissement.Ville)
                .NotNull().WithMessage("La ville est obligatoire")
                .MaximumLength(100).WithMessage("Maximum 100 caractères pour définir le nom la ville");

            RuleFor(x => x.etablissement.Pays)
                .NotNull().WithMessage("Le pays est obligatoire");

            RuleFor(x => x.etablissement.Rue)
                .NotNull().WithMessage("La rue est obligatoire")
                .MaximumLength(100).WithMessage("Maximum 100 caractères pour définir le nom la rue");

            RuleFor(x => x.etablissement.NumeroBoite)
                .NotNull().WithMessage("Le numéro de boîte est obligatoire")
                .InclusiveBetween(1,100000).WithMessage("Maximum 20 caractères pour le numéro de boîte");

            RuleFor(x => x.etablissement.NumeroTelephone)
                .MaximumLength(25).WithMessage("Maximum 25 caractères pour le numéro de téléphone")
                .Must(oValidation.ValidationNumTel).WithMessage("Le numéro de téléphone doit être valide");

            RuleFor(x => x.etablissement.AdresseEmailEtablissement)
                .EmailAddress().WithMessage("L'adresse mail doit être valide")
                .MaximumLength(255).WithMessage("Maximum 255 caractères pour defenir l'adresse mail de létablissement");
            RuleFor(x => x.Logo)
                .NotNull().WithMessage("Le logo est obligatoire");
            RuleFor(x => x.etablissement.AdresseSiteWeb)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$").WithMessage("Le format de l'adresse Web doit être valide");

            RuleFor(x => x.etablissement.AdresseInstagram)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$").WithMessage("Le format de l'adresse Web doit être valide");

            RuleFor(x => x.etablissement.AdresseFacebook)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$").WithMessage("Le format de l'adresse Web doit être valide");

            RuleFor(x => x.etablissement.AdresseLinkedin)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$").WithMessage("Le format de l'adresse Web doit être valide");
        }

        public async Task<bool> NumTvaValide(string numTva, CancellationToken token)
        {
            VatRoot _reponse = await vatValidator.VATReponse(numTva);
            return _reponse.Valid;
        }
    }
}
