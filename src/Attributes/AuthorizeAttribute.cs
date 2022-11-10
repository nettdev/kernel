namespace Mobnet.SharedKernel;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizeAttribute : Attribute
{
    public string Resource { get; }
    public bool CheckTentantAccess { get; }

    public AuthorizeAttribute(string resource, bool checkTentantAccess = true)
    {
        Resource = resource;
        CheckTentantAccess = checkTentantAccess;
    }
}