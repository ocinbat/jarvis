namespace Jarvis.Extensions
{
    public static class IntegerExtensions
    {
        public static int ToInt(this long value)
        {
            return (int) value;
        }
    }
}