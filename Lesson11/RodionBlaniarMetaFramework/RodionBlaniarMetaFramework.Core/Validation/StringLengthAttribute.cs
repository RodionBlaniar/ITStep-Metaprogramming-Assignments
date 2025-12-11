namespace RodionBlaniarMetaFramework.Core.Validation
{
    public class StringLengthAttribute : ValidationAttribute
    {
        public int Min { get; }
        public int Max { get; }

        public StringLengthAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override string Validate(object value, string propertyName)
        {
            if (value == null)
                return null;

            if (value is string s)
            {
                if (s.Length < Min || s.Length > Max)
                {
                    return $"{propertyName} must be between {Min} and {Max} characters";
                }
            }
            return null;
        }
    }
}