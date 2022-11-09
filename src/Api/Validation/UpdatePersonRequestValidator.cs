﻿using DockerTestsSample.Api.Contracts.Requests;
using FluentValidation;
using JetBrains.Annotations;

namespace DockerTestsSample.Api.Validation;

[UsedImplicitly]
public sealed class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
{
    public UpdatePersonRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Person).NotNull();
        RuleFor(x => x.Person.Name).NotEmpty();
        RuleFor(x => x.Person.LastName).NotEmpty();
        RuleFor(x => x.Person.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Person.BirthDate).NotEmpty();
    }
}
