namespace ExaminationSystem.Contracts.Seed;

public interface IDataSeeding
{
    public Task DataSeedAsync();
    
    public Task IdentityDataSeedAsync();
}