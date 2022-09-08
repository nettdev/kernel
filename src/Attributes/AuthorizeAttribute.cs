namespace Mobnet.SharedKernel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute(string permission, string description) 
    {
        Permission = permission;
        Description = description;
    }

    public string Permission { get; private set; }
    public string Description { get; private set; }
}