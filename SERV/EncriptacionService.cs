using ABS.Services;
using System.Security.Cryptography;
using System.Text;

namespace SERV;

/// <summary>
/// Servicio de hash usando MD5
/// </summary>
public class EncriptacionService : IEncriptacionService
{
    /// <summary>
    /// Genera un hash MD5 del texto proporcionado
    /// </summary>
    /// <param name="textoPlano">Texto a hashear</param>
    /// <returns>Hash en formato hexadecimal</returns>
    public string Encriptar(string textoPlano)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(textoPlano);
        
        using MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(textoPlano);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}
