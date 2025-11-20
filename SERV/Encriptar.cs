using System.Security.Cryptography;
using System.Text;

namespace SERV;

/// <summary>
/// Servicio de encriptación MD5
/// </summary>
public class Encriptar()
{
    /// <summary>
    /// Genera un hash MD5 del texto proporcionado
    /// </summary>
    /// <param name="input">Texto a hashear</param>
    /// <returns>Hash en formato hexadecimal</returns>
    public static string CreateMD5(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        StringBuilder sb = new();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}
