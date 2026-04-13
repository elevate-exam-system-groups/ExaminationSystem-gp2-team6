using System.Text.Json.Serialization;

namespace ExaminationSystem.Features.Diplomas;

public sealed class DiplomaQuizItemDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("duration_minutes")]
    public int DurationMinutes { get; init; }

    [JsonPropertyName("attempt_count")]
    public int AttemptCount { get; init; }

    [JsonPropertyName("last_score")]
    public int? LastScore { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = null!;
}
