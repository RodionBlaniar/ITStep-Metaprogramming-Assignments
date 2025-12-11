namespace RodionBlaniarMetaFramework.Core.Validation
{
    public class RequiredAttribute : ValidationAttribute
    {
        public override string Validate(object value, string propertyName)
        {
            if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
            {
                return $"{propertyName} is required";
            }
            return null;
        }
    }
}