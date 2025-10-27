using ABS.Services;
using System.Security.Cryptography;
using System.Text;

namespace SERV;

/// <summary>
/// Servicio de hash usando SHA256
/// </summary>
public class EncriptacionService : IEncriptacionService
{
    /// <summary>
    /// Genera un hash SHA256 del texto proporcionado
    /// </summary>
    /// <param name="textoPlano">Texto a hashear</param>
    /// <returns>Hash en formato Base64</returns>
    public string Encriptar(string textoPlano)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(textoPlano);
        var bytes = Encoding.UTF8.GetBytes(textoPlano);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
