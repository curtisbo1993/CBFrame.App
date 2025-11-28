using CBFrame.SectionsDb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using CBFrame.SectionsDb;

namespace CBFrame.SectionsDb
{
    /// <summary>
    /// Provides read-only access to the sections database (JSON-backed).
    /// Keeps all engine / DB logic outside the WPF layer.
    /// </summary>
    public sealed class SectionLookupService
    {
        private readonly Lazy<SectionDatabase> _database;

        public SectionLookupService()
        {
            _database = new Lazy<SectionDatabase>(LoadEmbeddedDatabase);
        }

        /// <summary>
        /// All sections currently in the database.
        /// </summary>
        public IReadOnlyList<SectionShape> GetAllSections()
            => _database.Value.Sections;

        /// <summary>
        /// Find a section by its Id (e.g. "W10X33").
        /// Returns null if not found.
        /// </summary>
        public SectionShape? FindById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return _database.Value.Sections
                .FirstOrDefault(s =>
                    string.Equals(s.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns all sections for a given shape family (e.g. "W", "HSS").
        /// </summary>
        public IReadOnlyList<SectionShape> FindByShapeType(string shapeType)
        {
            if (string.IsNullOrWhiteSpace(shapeType))
                return Array.Empty<SectionShape>();

            return _database.Value.Sections
                .Where(s => string.Equals(s.ShapeType, shapeType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static SectionDatabase LoadEmbeddedDatabase()
        {
            var assembly = typeof(SectionLookupService).Assembly;

            // Must match the default namespace + folder + file name.
            const string resourceName = "CBFrame.SectionsDb.Data.sections.us.json";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Embedded sections DB resource '{resourceName}' was not found. " +
                    "Check the file name, namespace, and Build Action = Embedded resource.");

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var db = JsonSerializer.Deserialize<SectionDatabase>(json, options);
            return db ?? new SectionDatabase();
        }
    }
}
