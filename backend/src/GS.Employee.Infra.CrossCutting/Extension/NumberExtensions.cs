namespace GS.Employee.Infra.CrossCutting.Extension
{
    public static class NumberExtensions
    {
        public static string ToDecimalString(this decimal input)
        {
            return input.ToString("F5", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
