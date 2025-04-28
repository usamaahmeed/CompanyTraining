using System.ComponentModel.DataAnnotations;

namespace CampanyTraining.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MinLengthCustomAttribute : ValidationAttribute
    {
        private readonly int _minLength;

        public MinLengthCustomAttribute(int minLength)
        {
            _minLength = minLength;
        }

        public override bool IsValid(object? value)
        {
            if(value is string s)
            {
                if (s.Length > _minLength)
                    return true;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The prop {name} must have min length {_minLength}";
        }

    }
}
