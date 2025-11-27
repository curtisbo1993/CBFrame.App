namespace CBFrame.Core.Loads
{
    /// <summary>
    /// One term in a load combination: Factor * LoadCase.
    /// </summary>
    public sealed class LoadCombinationTerm
    {
        public int LoadCaseId { get; }
        public double Factor { get; set; }

        public LoadCombinationTerm(int loadCaseId, double factor)
        {
            LoadCaseId = loadCaseId;
            Factor = factor;
        }
    }
}
