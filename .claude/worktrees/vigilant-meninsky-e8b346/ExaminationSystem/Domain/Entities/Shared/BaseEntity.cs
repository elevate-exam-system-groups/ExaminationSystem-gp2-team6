
namespace ExaminationSystem.Domain.Entities.Shared;

public class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    
    public bool IsDeleted { get; set; }
}