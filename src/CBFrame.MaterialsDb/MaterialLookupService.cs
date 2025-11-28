using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using CBFrame.MaterialsDb.Models;

namespace CBFrame.MaterialsDb
{
    /// <summary>
    /// Provides read-only access to the materials database (JSON-backed).
    /// </summary>
    public sealed class MaterialLookupService
    {
        private readonly Lazy<MaterialDatabase> _database;

        public MaterialLookupService()
        {
            _database = new Lazy<MaterialDatabase>(LoadEmbeddedDatabase);
        }

        public IReadOnlyList<MaterialRecord> GetAllMaterials()
            => _database.Value.Materials;

        public MaterialRecord? FindById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return _database.Value.Materials
                .FirstOrDefault(m =>
                    string.Equals(m.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<MaterialRecord> FindByType(string materialType)
        {
            if (string.IsNullOrWhiteSpace(materialType))
                return Array.Empty<MaterialRecord>();

            return _database.Value.Materials
                .Where(m =>
                    string.Equals(m.MaterialType, materialType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static MaterialDatabase LoadEmbeddedDatabase()
        {
            var assembly = typeof(MaterialLookupService).Assembly;

            const string resourceName = "CBFrame.MaterialsDb.Data.materials.us.json";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Embedded materials DB resource '{resourceName}' was not found. " +
                    "Check the file name, namespace, and Build Action = Embedded resource.");

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var db = JsonSerializer.Deserialize<MaterialDatabase>(json, options);
            return db ?? new MaterialDatabase();
        }
    }
}
