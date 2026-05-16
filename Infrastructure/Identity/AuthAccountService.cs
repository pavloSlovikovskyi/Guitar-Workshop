using Application.Common.Interfaces;
using Domain.Customers;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class AuthAccountService : IAuthAccountService
{
    public const string CustomerRoleName = "Customer";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AuthAccountService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthRegistrationResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser is not null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new AuthRegistrationResult(false, null, ["Email is already registered."]);
                }

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new AuthRegistrationResult(
                        false,
                        null,
                        createResult.Errors.Select(e => e.Description).ToList());
                }

                var roleResult = await _userManager.AddToRoleAsync(user, CustomerRoleName);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new AuthRegistrationResult(
                        false,
                        null,
                        roleResult.Errors.Select(e => e.Description).ToList());
                }

                var customer = Customer.New(
                    CustomerId.New(),
                    firstName,
                    lastName,
                    phoneNumber: "-",
                    email);

                customer.IdentityId = user.Id;

                await _context.Customers.AddAsync(customer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new AuthRegistrationResult(true, customer.Id.Value, []);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public async Task<AuthenticatedUser?> ValidateLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return null;

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new AuthenticatedUser(user.Id, user.Email ?? email, roles.ToList());
    }
}
