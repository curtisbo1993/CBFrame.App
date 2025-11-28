using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CBFrame.Core.Materials;

namespace CBFrame.MaterialsDb
{
    internal sealed class MaterialsJsonRoot
    {
        public UnitsInfo? Units { get; set; }
        public List<MaterialDefinition> Materials { get; set; } = new();
    }

    internal sealed class UnitsInfo
    {
        public string? Stress { get; set; }
        public string? Density { get; set; }
    }

    /// <summary>
    /// Low-level JSON loader for materials.
    /// In Phase 8 we will wrap this with MaterialLookupService in the app layer.
    /// </summary>
    public static class MaterialsJsonDatabase
    {
        public static IReadOnlyList<MaterialDefinition> LoadBasicMaterials(string? baseDirectory = null)
        {
            baseDirectory ??= AppContext.BaseDirectory;

            var path = Path.Combine(baseDirectory, "Data", "materials_basic.json");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Could not find material database JSON at: {path}");

            var json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<MaterialsJsonRoot>(json, options)
                       ?? new MaterialsJsonRoot();

            return root.Materials;
        }
    }
}
