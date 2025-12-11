namespace RodionBlaniarMetaFramework.Core.Validation
{
    public class RangeAttribute : ValidationAttribute
    {
        public int Min { get; }
        public int Max { get; }

        public RangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override string Validate(object value, string propertyName)
        {
            if (value == null)
                return null;

            if (value is int num)
            {
                if (num < Min || num > Max)
                {
                    return $"{propertyName} must be between {Min} and {Max}";
                }
            }
            return null;
        }
    }
}