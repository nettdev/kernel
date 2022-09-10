namespace Mobnet.SharedKernel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string Resource { get; }

    public AuthorizeAttribute(string resourceName) =>
        Resource = resourceName;
}