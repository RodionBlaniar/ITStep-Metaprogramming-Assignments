using System.Collections.Generic;
using System.Reflection;

namespace RodionBlaniarMetaFramework.Core.Validation
{
    public class Validator
    {
        public List<string> Validate(object obj)
        {
            List<string> errors = new List<string>();

            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                object value = prop.GetValue(obj);

                object[] attributes = prop.GetCustomAttributes(typeof(ValidationAttribute), true);

                foreach (object attr in attributes)
                {
                    ValidationAttribute validationAttr = attr as ValidationAttribute;
                    if (validationAttr != null)
                    {
                        string error = validationAttr.Validate(value, prop.Name);
                        if (error != null)
                        {
                            errors.Add(error);
                        }
                    }
                }
            }

            return errors;
        }
    }
}