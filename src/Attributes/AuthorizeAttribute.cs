namespace Mobnet.SharedKernel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute(string permission, string group, string label) 
    {
        Permission = permission;
        Group = group;
        Label = label;
    }

    public string Permission { get; }
    public string Group { get; }
    public string Label { get; }
}