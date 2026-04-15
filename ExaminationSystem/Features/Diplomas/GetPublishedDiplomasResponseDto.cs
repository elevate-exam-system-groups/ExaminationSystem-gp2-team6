using System.Text.Json.Serialization;

namespace ExaminationSystem.Features.Diplomas;

public sealed class GetPublishedDiplomasResponseDto
{
    [JsonPropertyName("page")]
    public int Page { get; init; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; init; }

    [JsonPropertyName("total")]
    public int Total { get; init; }

    [JsonPropertyName("items")]
    public IReadOnlyList<PublishedDiplomaItemDto> Items { get; init; } = Array.Empty<PublishedDiplomaItemDto>();
}

public sealed class PublishedDiplomaItemDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("quiz_count")]
    public int QuizCount { get; init; }

    [JsonPropertyName("student_progress")]
    public double StudentProgress { get; init; }
}
