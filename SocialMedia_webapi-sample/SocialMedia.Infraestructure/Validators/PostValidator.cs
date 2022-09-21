using System;
using FluentValidation;
using SocialMedia.Core.Dtos;

namespace SocialMedia.Infraestructure.Validators
{
    public class PostValidator: AbstractValidator<PublicacionDto>
    {
        public PostValidator()
        {
            // dos validaciones en una sola Regla
            RuleFor(post => post.Descripcion)
                .NotNull()
                .Length(10, 150);

            RuleFor(post => post.Fecha)
                .NotNull()
                .LessThan(DateTime.Now);
        }


    }
}
