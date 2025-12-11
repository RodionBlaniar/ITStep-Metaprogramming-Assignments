using System;

namespace RodionBlaniarMetaFramework.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        public abstract string Validate(object value, string propertyName);
    }
}