namespace Nett.SharedKernel;

public interface IApplicationInitialiser
{
    Task InitialiseAsync();
    Task SeedAsync();
}