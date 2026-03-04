using AlumniNetwork.Application.DTOs;
using FluentValidation;

namespace AlumniNetwork.Application.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostRequestDto>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.ImageUrl).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));
    }
}

public class AddCommentValidator : AbstractValidator<AddCommentRequestDto>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.CommentText).NotEmpty().MaximumLength(1000);
    }
}
