using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CBFrame.Core.Sections;

namespace CBFrame.SectionsDb
{
    internal sealed class SectionsJsonRoot
    {
        public string? Database { get; set; }
        public UnitsInfo? Units { get; set; }
        public List<SectionShape> Shapes { get; set; } = new();
    }

    internal sealed class UnitsInfo
    {
        public string? Length { get; set; }
        public string? Area { get; set; }
        public string? Inertia { get; set; }
        public string? Modulus { get; set; }
        public string? WeightPerLength { get; set; }
    }

    /// <summary>
    /// Low-level JSON loader for section shapes.
    /// In Phase 8 we will wrap this with SectionLookupService in the app layer.
    /// </summary>
    public static class SectionsJsonDatabase
    {
        public static IReadOnlyList<SectionShape> LoadSteelAiscShapes(string? baseDirectory = null)
        {
            baseDirectory ??= AppContext.BaseDirectory;

            var path = Path.Combine(baseDirectory, "Data", "steel_aisc_shapes.json");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Could not find section database JSON at: {path}");

            var json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<SectionsJsonRoot>(json, options)
                       ?? new SectionsJsonRoot();

            return root.Shapes;
        }
    }
}
