using System.Collections.Generic;

namespace CBFrame.Core.Loads
{
    public static class LoadDefaults
    {
        public static List<LoadCase> CreateBasicLoadCases()
        {
            var cases = new List<LoadCase>();

            int id = 1;

            var dead = new LoadCase(id++, "DL - Dead Load", LoadCaseType.Dead)
            {
                IncludeSelfWeight = true,
                SelfWeightFactor = 1.0
            };
            cases.Add(dead);

            cases.Add(new LoadCase(id++, "LL - Live Load", LoadCaseType.Live));
            cases.Add(new LoadCase(id++, "RL - Roof Live", LoadCaseType.RoofLive));
            cases.Add(new LoadCase(id++, "SL - Snow Load", LoadCaseType.Snow));

            cases.Add(new LoadCase(id++, "WX+ - Wind +X", LoadCaseType.Wind));
            cases.Add(new LoadCase(id++, "WX- - Wind -X", LoadCaseType.Wind));
            cases.Add(new LoadCase(id++, "WY+ - Wind +Y", LoadCaseType.Wind));
            cases.Add(new LoadCase(id++, "WY- - Wind -Y", LoadCaseType.Wind));

            cases.Add(new LoadCase(id++, "EX+ - EQ +X", LoadCaseType.Seismic));
            cases.Add(new LoadCase(id++, "EX- - EQ -X", LoadCaseType.Seismic));
            cases.Add(new LoadCase(id++, "EY+ - EQ +Y", LoadCaseType.Seismic));
            cases.Add(new LoadCase(id++, "EY- - EQ -Y", LoadCaseType.Seismic));

            return cases;
        }

        public static List<LoadCombination> CreateBasicLoadCombinations()
        {
            var combos = new List<LoadCombination>();

            int id = 1;

            // 1.4D
            var c1 = new LoadCombination(id++, "1.4D", LoadCombinationType.Ultimate);
            c1.Terms.Add(new LoadCombinationTerm(1, 1.4));
            combos.Add(c1);

            // 1.2D + 1.6L
            var c2 = new LoadCombination(id++, "1.2D + 1.6L", LoadCombinationType.Ultimate);
            c2.Terms.Add(new LoadCombinationTerm(1, 1.2)); // DL
            c2.Terms.Add(new LoadCombinationTerm(2, 1.6)); // LL
            combos.Add(c2);

            // 1.2D + 1.0L + 1.0Wx
            var c3 = new LoadCombination(id++, "1.2D + 1.0L + 1.0Wx", LoadCombinationType.Ultimate);
            c3.Terms.Add(new LoadCombinationTerm(1, 1.2));
            c3.Terms.Add(new LoadCombinationTerm(2, 1.0));
            c3.Terms.Add(new LoadCombinationTerm(5, 1.0)); // WX+
            combos.Add(c3);

            // Serviceability: 1.0D + 1.0L
            var c4 = new LoadCombination(id++, "SVC 1.0D + 1.0L", LoadCombinationType.Serviceability);
            c4.Terms.Add(new LoadCombinationTerm(1, 1.0));
            c4.Terms.Add(new LoadCombinationTerm(2, 1.0));
            combos.Add(c4);

            return combos;
        }
    }
}
