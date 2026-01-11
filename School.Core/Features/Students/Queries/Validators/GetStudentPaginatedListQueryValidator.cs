namespace School.Core.Features.Students.Queries.Validators
{
    //public class GetStudentPaginatedListQueryValidator : AbstractValidator<GetStudentPaginatedListQuery>
    //{

    //    private static readonly string[] AllowedOrderBy =
    //{
    //    "StudentID",
    //    "NameAr",
    //    "NameEn",
    //    "Address",
    //    "Phone"
    //};

    //    public GetStudentPaginatedListQueryValidator()
    //    {
    //        // PageNumber must start from 1
    //        RuleFor(x => x.PageNumber)
    //            .GreaterThan(0)
    //            .WithMessage("PageNumber must be greater than 0.");

    //        // PageSize limits (protect DB)
    //        RuleFor(x => x.PageSize)
    //            .GreaterThan(0)
    //            .WithMessage("PageSize must be greater than 0.")
    //            .LessThanOrEqualTo(50)
    //            .WithMessage("PageSize must not exceed 50.");

    //        // OrderBy validation (if provided)
    //        RuleFor(x => x.OrderBy)
    //            .Must(BeAValidOrderBy)
    //            .When(x => !string.IsNullOrWhiteSpace(x.OrderBy))
    //            .WithMessage($"OrderBy must be one of: {string.Join(", ", AllowedOrderBy)}");

    //        // Search length (optional)
    //        RuleFor(x => x.Search)
    //            .MaximumLength(100)
    //            .When(x => !string.IsNullOrWhiteSpace(x.Search))
    //            .WithMessage("Search text must not exceed 100 characters.");
    //    }

    //    private bool BeAValidOrderBy(string? orderBy)
    //    {
    //        return AllowedOrderBy.Contains(orderBy);
    //    }
    //}
}
