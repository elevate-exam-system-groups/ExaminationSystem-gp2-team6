namespace ExaminationSystem.Features.QuizEngine.ViewResults
{
    public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PerPage,
    int TotalPages
);
}
