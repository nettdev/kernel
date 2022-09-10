namespace Mobnet.SharedKernel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string Permission { get; }
    public string Group { get; }
    public string Label { get; }
    public bool VisibleInResourceList { get; }

    public AuthorizeAttribute(string permission, string group, string label, bool visibleInResourceList = true) 
    {
        Permission = permission;
        Group = group;
        Label = label;
        VisibleInResourceList = visibleInResourceList;
    }
}