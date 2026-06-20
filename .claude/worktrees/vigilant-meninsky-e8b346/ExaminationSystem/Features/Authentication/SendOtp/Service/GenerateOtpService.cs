using System.Security.Cryptography;
using ExaminationSystem.Domain.Contracts;

namespace ExaminationSystem.Features.Authentication.SendOtp.Service;

public class GenerateOtpService : IGenerateOtpService
{
    public string GenerateOtp()
    {
        var bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        var number = BitConverter.ToUInt32(bytes, 0) % 900000 + 100000;

        return number.ToString();
    }
}