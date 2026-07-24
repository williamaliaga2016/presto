using Multibanca.Application.Interfaces.Common;

namespace Multibanca.Application.Implementations.Helpers;

/// <summary>
/// Métodos utilitarios para resolver descripciones de catálogos.
/// Reutilizable desde cualquier Application.
/// </summary>
public static class CatalogHelper
{
    /// <summary>
    /// Obtiene la descripción de un catálogo dado su tipo y código.
    /// Si no encuentra el catálogo, retorna el fallback proporcionado (o el código mismo).
    /// </summary>
    public static async Task<string?> GetDescFromCatalog(
        ICommonApplication commonApplication,
        string type,
        string? code,
        string? fallback = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return fallback;

        var catalogo = await commonApplication.GetCatalogoByTypeAndCode(type, code);

        return catalogo?.description ?? fallback ?? code;
    }
}
