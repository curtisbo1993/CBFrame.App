using CBFrame.MaterialsDb;
using CBFrame.SectionsDb;

namespace CBFrame.App.Wpf.Infrastructure
{
    /// <summary>
    /// Central registry for read-only lookup services used across the app.
    /// For now it's a simple static holder so dialogs and panels can access
    /// sections and materials without complex DI.
    /// </summary>
    public static class AppLookupServices
    {
        /// <summary>
        /// Global sections lookup (JSON / embedded DB-backed).
        /// </summary>
        public static SectionLookupService Sections { get; } = new SectionLookupService();

        /// <summary>
        /// Global materials lookup (JSON / embedded DB-backed).
        /// </summary>
        public static MaterialLookupService Materials { get; } = new MaterialLookupService();
    }
}
