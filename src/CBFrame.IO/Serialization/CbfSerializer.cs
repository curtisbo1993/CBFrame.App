using System;
using System.IO;
using System.Text.Json;

namespace CBFrame.IO.Serialization;

/// <summary>
/// Serializes a CbfProjectRoot into JSON (.cbf file content).
/// </summary>
public sealed class CbfSerializer
{
    private readonly JsonSerializerOptions _options;

    public CbfSerializer()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// Serialize to a JSON string.
    /// </summary>
    public string SerializeToString(CbfProjectRoot root)
    {
        if (root is null) throw new ArgumentNullException(nameof(root));
        return JsonSerializer.Serialize(root, _options);
    }

    /// <summary>
    /// Serialize directly to a file on disk.
    /// </summary>
    public void SerializeToFile(CbfProjectRoot root, string filePath)
    {
        if (root is null) throw new ArgumentNullException(nameof(root));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path must not be null or empty.", nameof(filePath));

        var json = SerializeToString(root);
        File.WriteAllText(filePath, json);
    }
}
