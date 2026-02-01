using System.IO.Compression;
using System.Text;

namespace BackofficeCore.Shared.Utils;

public static class ZipHelper
{
    /// <summary>
    /// Crea un archivo ZIP en memoria con la estructura de carpetas y archivos proporcionada
    /// </summary>
    /// <param name="files">Diccionario donde la clave es la ruta relativa (ej: "Sala/PH-001-qr.svg") y el valor es el contenido del archivo</param>
    /// <returns>Stream del archivo ZIP</returns>
    public static MemoryStream CreateZipFile(Dictionary<string, string> files)
    {
        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var file in files)
            {
                var entry = archive.CreateEntry(file.Key, CompressionLevel.Optimal);

                using var entryStream = entry.Open();
                using var writer = new StreamWriter(entryStream, Encoding.UTF8);
                writer.Write(file.Value);
            }
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    /// <summary>
    /// Crea un archivo ZIP con archivos de texto y binarios
    /// </summary>
    /// <param name="textFiles">Diccionario de archivos de texto</param>
    /// <param name="binaryFiles">Diccionario de archivos binarios</param>
    /// <returns>Stream del archivo ZIP</returns>
    public static MemoryStream CreateZipFileWithBinary(Dictionary<string, string> textFiles, Dictionary<string, byte[]> binaryFiles)
    {
        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            // Agregar archivos de texto
            foreach (var file in textFiles)
            {
                var entry = archive.CreateEntry(file.Key, CompressionLevel.Optimal);

                using var entryStream = entry.Open();
                using var writer = new StreamWriter(entryStream, Encoding.UTF8);
                writer.Write(file.Value);
            }

            // Agregar archivos binarios
            foreach (var file in binaryFiles)
            {
                var entry = archive.CreateEntry(file.Key, CompressionLevel.Optimal);

                using var entryStream = entry.Open();
                entryStream.Write(file.Value, 0, file.Value.Length);
            }
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    /// <summary>
    /// Agrega un archivo SVG a la estructura de archivos para el ZIP
    /// </summary>
    /// <param name="files">Diccionario de archivos</param>
    /// <param name="folderPath">Ruta de la carpeta (ej: "Sala")</param>
    /// <param name="fileName">Nombre del archivo (ej: "PH-001-qr.svg")</param>
    /// <param name="svgContent">Contenido SVG del archivo</param>
    public static void AddSvgFile(Dictionary<string, string> files, string folderPath, string fileName, string svgContent)
    {
        var filePath = string.IsNullOrEmpty(folderPath) 
            ? fileName 
            : $"{folderPath}/{fileName}";

        files[filePath] = svgContent;
    }
}
