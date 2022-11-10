namespace Nett.Kernel;

public interface IApplicationInitialiser
{
    Task InitialiseAsync();
    Task SeedAsync();
}