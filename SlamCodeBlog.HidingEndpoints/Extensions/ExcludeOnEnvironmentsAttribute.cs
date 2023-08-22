namespace SlamCodeBlog.HidingEndpoints.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ExcludeOnEnvironmentsAttribute : Attribute
    {
        public ExcludeOnEnvironmentsAttribute(params string[] environments)
        {
            Environments = environments;
        }

        public string[] Environments { get; }
    }
}
