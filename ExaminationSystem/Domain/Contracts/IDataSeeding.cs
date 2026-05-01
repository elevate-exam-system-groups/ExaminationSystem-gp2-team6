namespace ExaminationSystem.Domain.Contracts;

public interface IDataSeeding
{
    public Task DataSeedAsync();
    
    public Task IdentityDataSeedAsync();
}