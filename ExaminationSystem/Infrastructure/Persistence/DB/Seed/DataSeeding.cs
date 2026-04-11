using ExaminationSystem.Contracts.Seed;
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
            // Diploma
            if (!_context.Diplomas.Any()) 
            {
                foreach (var diploma in DiplomaDataSeed.Diplomas)
                {
                    await _context.Diplomas.AddAsync(diploma);
                }
            }
            
            // Quizzez
            if (!_context.Quizzes.Any())
            {
                foreach (var quiz in QuizDataSeed.Quizzes())
                {
                    await _context.Quizzes.AddAsync(quiz);
                }
                
            }
            
            // Questions
            if (!_context.Questions.Any())
            {
                foreach (var question in QuestionDataSeed.Questions)
                {
                    await _context.Questions.AddAsync(question);
                }
            }
            
            // Answers
            if (!_context.AnswerOptions.Any())
            {
                foreach (var answer in AnswerOptionDataSeed.Answers)
                {
                    await _context.AnswerOptions.AddAsync(answer);
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
    }
}