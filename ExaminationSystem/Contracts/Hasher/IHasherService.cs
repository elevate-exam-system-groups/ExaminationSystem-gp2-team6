namespace ExaminationSystem.Contracts.Hasher;

public interface IHasherService
{
    string Hash(string token);
}