namespace Mobnet.SharedKernel;

public interface IApplicationInitialiser
{
    Task InitialiseAsync();
    Task SeedAsync();
}