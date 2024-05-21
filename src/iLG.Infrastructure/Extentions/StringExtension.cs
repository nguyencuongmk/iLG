namespace iLG.Infrastructure.Extentions
{
    public static class StringExtension
    {
        public static T ToEnum<T>(this string? value) where T : struct
        {
            ArgumentException.ThrowIfNullOrEmpty(value);

            if (Enum.TryParse(value, out T result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"{value} is not type of {typeof(T)}");
            }
        }
    }
}