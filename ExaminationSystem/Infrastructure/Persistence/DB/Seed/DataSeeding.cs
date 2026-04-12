using ExaminationSystem.Contracts.Seed;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.AnswerOption;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.Diploma;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.Question;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.Quiz;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.Roles;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed.Data.User;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Infrastructure.Persistence.DB.Seed;

public class DataSeeding : IDataSeeding
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    
    public DataSeeding(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task DataSeedAsync()
    {
        try
        {
            if (!_context.Diplomas.Any())
            {
                await _context.Diplomas.AddRangeAsync(DiplomaDataSeed.Diplomas);
                await _context.SaveChangesAsync();
            }

            if (!_context.Quizzes.Any())
            {
                await _context.Quizzes.AddRangeAsync(QuizDataSeed.Quizzes());
                await _context.SaveChangesAsync();
            }

            if (!_context.Questions.Any())
            {
                await _context.Questions.AddRangeAsync(QuestionDataSeed.Questions);
                await _context.SaveChangesAsync();
            }

            if (!_context.AnswerOptions.Any())
            {
                await _context.AnswerOptions.AddRangeAsync(AnswerOptionDataSeed.Answers);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while seeding: {ex.Message}");
            throw;
        }
    }

    public async Task IdentityDataSeedAsync()
    {
        try
        {
            // Roles
            if (!_roleManager.Roles.Any())
            {
                foreach (var role in RoleDataSeed.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    }
                }
            }

            // Users
            if (!_userManager.Users.Any())
            {
                foreach (var user in UserDataSeed.Users)
                {
                    var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                    if (result.Succeeded)
                    {
                        var role = user.UserType.ToString();

                        if (await _roleManager.RoleExistsAsync(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine(error.Description);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while saving seeded data: {ex.Message}");
            throw;
        }

        await SeedStudentDashboardSampleDataAsync();
    }

    private async Task SeedStudentDashboardSampleDataAsync()
    {
        if (_context.StudentDiplomaEnrollments.Any())
            return;

        var student = await _userManager.FindByEmailAsync("test@gmail.com");
        if (student is null)
            return;

        await _context.StudentDiplomaEnrollments.AddRangeAsync(
            new StudentDiplomaEnrollment
            {
                StudentId = student.Id,
                DiplomaId = 1,
                EnrolledAt = DateTime.UtcNow
            },
            new StudentDiplomaEnrollment
            {
                StudentId = student.Id,
                DiplomaId = 2,
                EnrolledAt = DateTime.UtcNow
            });

        await _context.QuizAttempts.AddRangeAsync(
            new QuizAttempt
            {
                StudentId = student.Id,
                QuizId = 1,
                Score = 75,
                SubmittedAt = DateTime.UtcNow.AddDays(-2)
            },
            new QuizAttempt
            {
                StudentId = student.Id,
                QuizId = 2,
                Score = 55,
                SubmittedAt = DateTime.UtcNow.AddDays(-1)
            });

        await _context.SaveChangesAsync();
    }
}