using System.Security.Cryptography;
using System.Text;

namespace InvestCloud.MatrixCalculator.Application.Extensions;

internal static class StringExtensions
{
    public static string InterpolateConvert(this string input, object parameters)
    {
        var result = input;
        var properties = parameters.GetType().GetProperties();

        foreach (var property in properties)
        {
            result = result.Replace($"{{{{{property.Name}}}}}", property.GetValue(parameters).ToString());
        }

        return result;
    }

    public static string ToMD5(this string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString());
            }
            return sb.ToString();
        }
    }
}
