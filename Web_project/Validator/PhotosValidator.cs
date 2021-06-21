using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Validator
{
    public class PhotosValidator : AbstractValidator<IFormFile>
    {
        public PhotosValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(3000000)
                .WithMessage(x => x.FileName+ " est trop volumineux...La taille souhaitée d'une photo est de : Max. 3000000 bytes. Taille du fichier : {PropertyValue} bytes");

            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage("File type is larger than allowed");
        }

    }
}
