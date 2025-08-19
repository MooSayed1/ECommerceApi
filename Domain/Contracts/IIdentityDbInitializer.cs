namespace Domain.Contracts;

public interface IIdentityDbInitializer
{
    Task InitializeAsync();
}