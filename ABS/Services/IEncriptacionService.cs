namespace ABS.Services;

/// <summary>
/// Interfaz para servicios de hash de claves
/// </summary>
public interface IEncriptacionService
{
    /// <summary>
    /// Genera un hash SHA256 del texto proporcionado
    /// </summary>
    /// <param name="textoPlano">Texto a hashear</param>
    /// <returns>Hash en formato Base64</returns>
    string Encriptar(string textoPlano);
}
