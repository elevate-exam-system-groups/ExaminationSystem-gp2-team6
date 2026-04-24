using System.Text.Json.Serialization;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas;

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
