using System;
using System.IO;
using System.Text.Json;

namespace CBFrame.IO.Serialization;

/// <summary>
/// Deserializes a .cbf JSON string/file back into a CbfProjectRoot.
/// </summary>
public sealed class CbfDeserializer
{
    private readonly JsonSerializerOptions _options;

    public CbfDeserializer()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// Deserialize from a JSON string.
    /// </summary>
    public CbfProjectRoot DeserializeFromString(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON string must not be null or empty.", nameof(json));

        var root = JsonSerializer.Deserialize<CbfProjectRoot>(json, _options);
        if (root == null)
        {
            throw new InvalidOperationException("Failed to deserialize CBF project.");
        }

        return root;
    }

    /// <summary>
    /// Deserialize directly from a .cbf file on disk.
    /// </summary>
    public CbfProjectRoot DeserializeFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path must not be null or empty.", nameof(filePath));

        var json = File.ReadAllText(filePath);
        return DeserializeFromString(json);
    }
}
