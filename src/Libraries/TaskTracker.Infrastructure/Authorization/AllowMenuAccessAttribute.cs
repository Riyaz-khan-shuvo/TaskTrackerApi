namespace TaskTracker.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowMenuAccessAttribute : Attribute
    {
    }
}
