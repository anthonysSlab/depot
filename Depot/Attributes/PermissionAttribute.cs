namespace Depot.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(Type type, string commandName)
        {
            PermissionString = $"{type.Name}.{commandName}";
        }

        public string PermissionString { get; }
    }
}