using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Validator
{
    public class LogoValidator: AbstractValidator<IFormFile>
    {
        public LogoValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(1000000)
                .WithMessage(x => x.FileName+ " est trop volumineux...La taille souhaitée du logo est de : Max. 1000000 bytes. Taille du fichier : {PropertyValue} bytes");

            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png") || x.Equals("image/ico"))
                .WithMessage("Ce type de ficier n'est pas supporté");
        }
    }
}
