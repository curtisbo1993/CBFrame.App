namespace CBFrame.Core.Enums
{
    /// <summary>
    /// Type of structural element. This will be used across the model.
    /// </summary>
    public enum ElementType
    {
        Unknown = 0,
        Line = 1,
        Plate = 2,
        Wall = 3,
        Solid = 4,
        Spring = 5,
        Link = 6
    }

    /// <summary>
    /// Degree of freedom types for nodes and elements.
    /// </summary>
    public enum DofType
    {
        Ux = 0,
        Uy = 1,
        Uz = 2,
        Rx = 3,
        Ry = 4,
        Rz = 5
    }

    /// <summary>
    /// High-level analysis type selection (for later phases).
    /// </summary>
    public enum AnalysisType
    {
        LinearStatic = 0,
        Modal = 1,
        ResponseSpectrum = 2,
        PDelta = 3,
        NonlinearStatic = 4
    }

    /// <summary>
    /// Unit system used by the model.
    /// </summary>
    public enum UnitSystem
    {
        Imperial = 0,
        Metric = 1
    }

    /// <summary>
    /// Simple support classification used by nodes.
    /// This is descriptive only in Phase 2 (no stiffness logic).
    /// </summary>
    public enum SupportType
    {
        Custom = 0,
        Fixed = 1,
        Pinned = 2,
        Roller = 3,
        Spring = 4
    }
    /// <summary>
    /// Describes how a line element is used in the structure.
    /// </summary>
    public enum LineElementUsage
    {
        Generic = 0,
        Beam = 1,
        Column = 2,
        Brace = 3,
        Joist = 4,
        TrussChord = 5,
        TrussWeb = 6
    }
    /// <summary>
    /// General material classification.
    /// </summary>
    public enum MaterialType
    {
        Unknown = 0,
        Steel = 1,
        Concrete = 2,
        Wood = 3,
        Masonry = 4,
        Aluminum = 5
    }

    /// <summary>
    /// Steel shape production type.
    /// </summary>
    public enum SteelShapeType
    {
        Unknown = 0,
        Rolled = 1,
        BuiltUp = 2,
        WeldedPlate = 3,
        ColdFormed = 4
    }

    /// <summary>
    /// Concrete weight category.
    /// </summary>
    public enum ConcreteWeightClass
    {
        NormalWeight = 0,
        Lightweight = 1,
        Heavyweight = 2
    }
    /// <summary>
    /// Material category for sections (ties back to materials).
    /// </summary>
    public enum SectionMaterialType
    {
        Unknown = 0,
        Steel = 1,
        Concrete = 2,
        Wood = 3,
        Masonry = 4,
        Aluminum = 5
    }

    /// <summary>
    /// Shape family for a section.
    /// </summary>
    public enum SectionShapeFamily
    {
        Unknown = 0,
        WideFlange = 1,
        Channel = 2,
        Angle = 3,
        Tee = 4,
        RectTube = 5,
        RoundTube = 6,
        SolidRectangle = 7,
        SolidCircle = 8,
        Slab = 9,
        Wall = 10
    }

    /// <summary>
    /// Basic load case types.
    /// </summary>
    public enum LoadCaseType
    {
        Dead = 0,
        Live = 1,
        RoofLive = 2,
        Wind = 3,
        Seismic = 4,
        Snow = 5,
        Temperature = 6,
        Other = 99
    }

    /// <summary>
    /// Direction for loads (global or local DOFs).
    /// </summary>
    public enum LoadDirection
    {
        GlobalX = 0,
        GlobalY = 1,
        GlobalZ = 2,
        LocalX = 3,
        LocalY = 4,
        LocalZ = 5,
        RotationX = 6,
        RotationY = 7,
        RotationZ = 8
    }

    /// <summary>
    /// Distribution shape for element loads.
    /// </summary>
    public enum LoadDistributionType
    {
        Uniform = 0,
        Trapezoidal = 1,
        Point = 2
    }

    /// <summary>
    /// Indicates whether a result set comes from
    /// a single load case or from a load combination.
    /// </summary>
    public enum ResultSourceType
    {
        LoadCase = 0,
        LoadCombination = 1
    }
}
