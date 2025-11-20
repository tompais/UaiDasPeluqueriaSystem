namespace ABS.Services;

/// <summary>
/// Interfaz para servicios de hash de claves
/// </summary>
public interface IEncriptacionService
{
    /// <summary>
    /// Genera un hash MD5 del texto proporcionado
    /// </summary>
    /// <param name="textoPlano">Texto a hashear</param>
    /// <returns>Hash en formato hexadecimal</returns>
    string Encriptar(string textoPlano);
}
