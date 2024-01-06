

namespace SEOS.Support
{
    using System;
    using Sandbox.ModAPI;
    using Sandbox.Game.Entities;
    using VRage.Game;
    using VRageMath;
    using System.Collections.Generic;
    using SEOS.Core;
    using Sandbox.Definitions;
    using System.Linq;

    internal class RunningAverage
    {
        private readonly int _size;
        private readonly int[] _values;
        private int _valuesIndex;
        private int _valueCount;
        private int _sum;

        internal RunningAverage(int size)
        {
            _size = Math.Max(size, 1);
            _values = new int[_size];
        }

        internal int Add(int newValue)
        {
            // calculate new value to add to sum by subtracting the 
            // value that is replaced from the new value; 
            var temp = newValue - _values[_valuesIndex];
            _values[_valuesIndex] = newValue;
            _sum += temp;

            _valuesIndex++;
            _valuesIndex %= _size;

            if (_valueCount < _size)
                _valueCount++;

            return _sum / _valueCount;
        }
    }
    internal static class UtilsStatic
    {
        internal static Color White1 = new Color(255, 255, 255);
        internal static Color White2 = new Color(90, 118, 255);
        internal static Color White3 = new Color(47, 86, 255);
        internal static Color Blue1 = Color.Aquamarine;
        internal static Color Blue2 = new Color(0, 66, 255);
        internal static Color Blue3 = new Color(0, 7, 255, 255);
        internal static Color Blue4 = new Color(22, 0, 170);
        internal static Color Red1 = new Color(87, 0, 66);
        internal static Color Red2 = new Color(121, 0, 13);
        internal static Color Red3 = new Color(255, 0, 0);

        public static void UpdateTerminal(this MyCubeBlock block)
        {
            MyOwnershipShareModeEnum shareMode;
            long ownerId;
            if (block.IDModule != null)
            {
                ownerId = block.IDModule.Owner;
                shareMode = block.IDModule.ShareMode;
            }
            else
            {
                return;
            }
            block.ChangeOwner(ownerId, shareMode == MyOwnershipShareModeEnum.None ? MyOwnershipShareModeEnum.Faction : MyOwnershipShareModeEnum.None);
            block.ChangeOwner(ownerId, shareMode);
        }

        public static bool DistanceCheck(VRage.Game.ModAPI.IMyCubeBlock block, int x, double range)
        {
            if (MyAPIGateway.Session.Player.Character == null) return false;

            var pPosition = MyAPIGateway.Session.Player.Character.PositionComp.WorldVolume.Center;
            var cPosition = block.CubeGrid.PositionComp.WorldVolume.Center;
            var dist = Vector3D.DistanceSquared(cPosition, pPosition) <= (x + range) * (x + range);
            return dist;
        }

        public static Color GetEmissiveColorFromDouble(double percent)
        {
            
            return Color.Green;
            
        }

        public static Color GetShieldColorFromFloat(float percent)
        {
            if (percent > 90) return White1;
            if (percent > 80) return White2;
            if (percent > 70) return White3;
            if (percent > 60) return Blue1;
            if (percent > 50) return Blue2;
            if (percent > 40) return Blue3;
            if (percent > 30) return Blue4;
            if (percent > 20) return Red1;
            if (percent > 10) return Red2;
            return Red3;
        }

        public static List<string> GetFunctionalBlockList()
        {
            Session.SessionLog.Line($"SEOS FunctionalBlocks");
            var FunctionalBlockSubtypeIds = new List<string>();
            var allFunctionalBlockDefinitions = MyDefinitionManager.Static.GetDefinitionsOfType<MyFunctionalBlockDefinition>();

            foreach (var blockDef in allFunctionalBlockDefinitions)
            {

                FunctionalBlockSubtypeIds.Add(blockDef.Id.SubtypeName);
                Session.SessionLog.Line($"{blockDef.Id.SubtypeName} ");
            }
            return FunctionalBlockSubtypeIds;
        }

        public static List<string> GetThrusterBlockListByType()
        {
            Session.SessionLog.Line($"SEOS FunctionalBlocks");
            var FunctionalBlockSubtypeIds = new List<string>();
            var allFunctionalBlockDefinitions = MyDefinitionManager.Static.GetDefinitionsOfType<MyThrustDefinition>(); 

            foreach (var blockDef in allFunctionalBlockDefinitions)
            {

                FunctionalBlockSubtypeIds.Add(blockDef.Id.SubtypeName);
                Session.SessionLog.Line($"{blockDef.Id.SubtypeName} ");
            }
            return FunctionalBlockSubtypeIds;
        }
        public static List<string> GetCubeBlockListByType()
        {
            Session.SessionLog.Line($"SEOS FunctionalBlocks");
            var FunctionalBlockSubtypeIds = new List<string>();
            var allFunctionalBlockDefinitions = MyDefinitionManager.Static.GetDefinitionsOfType<MyContainerDefinition>();
            
            foreach (var blockDef in allFunctionalBlockDefinitions)
            {

                FunctionalBlockSubtypeIds.Add(blockDef.Id.SubtypeName);
                Session.SessionLog.Line($"{blockDef.Id.SubtypeName} ");
            }
            return FunctionalBlockSubtypeIds;
        }

        public static List<string> GetAllTypes()
        {
            List<string> blockTypes = new List<string>();
            foreach (var definition in MyDefinitionManager.Static.GetAllDefinitions())
            {
                // Use 'as' for safe casting; it will return null if the cast fails
                var blockDef = definition as MyCubeBlockDefinition;
                if (blockDef != null)
                {
                    blockTypes.Add(blockDef.Id.TypeId.ToString() + "/" + blockDef.Id.SubtypeId.ToString());
                }
            }
            return blockTypes;
        }

        public static List<string> GetAllTypeCategories<T>() where T : MyCubeBlockDefinition
        {
            List<string> typeCategories = new List<string>();

            // Get all definitions of a given base type
            var allBlockDefinitions = MyDefinitionManager.Static.GetDefinitionsOfType<T>();

            foreach (var blockDef in allBlockDefinitions)
            {
                string typeCategory = blockDef.Id.TypeId.ToString();

                if (!typeCategories.Contains(typeCategory))
                {

                    string originalString = typeCategory;
                    string toRemove = "MyObjectBuilder_";
                    string result = originalString.Replace(toRemove, "");


                    typeCategories.Sort();
                    typeCategories.Add(result);
                }
            }

            return typeCategories;
        }

        public static List<string> GetAllSubTypeCategories<T>() where T : MyCubeBlockDefinition
        {
            List<string> typeCategories = new List<string>();

            // Get all definitions of a given base type
            var allBlockDefinitions = MyDefinitionManager.Static.GetDefinitionsOfType<T>();

            foreach (var blockDef in allBlockDefinitions)
            {
                string typeCategory = blockDef.Id.SubtypeId.ToString();

                if (!typeCategories.Contains(typeCategory))
                {

                    string originalString = typeCategory;
                    string toRemove = "MyObjectBuilder_";
                    string result = originalString.Replace(toRemove, "");


                    typeCategories.Sort();
                    typeCategories.Add(result);
                }
            }

            return typeCategories;
        }
 
        public static Dictionary<string, List<string>> PopulateTypeSubtypeLibrary()
        {
            Dictionary<string, List<string>> typeSubtypeLibrary = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> ModifiedLibrary = new Dictionary<string, List<string>>();

            foreach (var definition in MyDefinitionManager.Static.GetAllDefinitions())
            {
                // Use 'as' for safe casting; it will return null if the cast fails
                MyCubeBlockDefinition blockDef = definition as MyCubeBlockDefinition;

                if (blockDef != null)
                {
                    string typeId = blockDef.Id.TypeId.ToString();
                    string subtypeId = blockDef.Id.SubtypeId.ToString();

                    // If the type is not yet in the dictionary, add it with an empty list as the value.
                    if (!typeSubtypeLibrary.ContainsKey(typeId))
                    {

                        typeSubtypeLibrary[typeId] = new List<string>();
                        //Session.SessionLog.Line($"SEOS: Type added to dict: {typeId}");
                    }


                    typeSubtypeLibrary[typeId].Add(subtypeId);

                    //Session.SessionLog.Line($"SEOS: Type added to dict: {subtypeId}");
                }
            }

            foreach(var name in typeSubtypeLibrary)
            {

                string originalString = name.Key;
                string toRemove = "MyObjectBuilder_";
                string result = originalString.Replace(toRemove, "");
                ModifiedLibrary.Add(result,name.Value);
            }

            return ModifiedLibrary;
        }



        public static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Initialize the distance matrix
            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            // Compute the Levenshtein distance
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        public static string SearchClosestMatch(string query, List<string> items)
        {
            int minDistance = int.MaxValue;
            string closestItem = null;

            foreach (string item in items)
            {
                int distance = LevenshteinDistance(query, item);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            return closestItem;
        }

        public static List<string> SearchClosestMatches(string query, List<string> items, int topN = 5)
        {
            // Create a dictionary to store each item with its distance to the query
            Dictionary<string, int> itemDistances = new Dictionary<string, int>();

            foreach (string item in items)
            {
                int distance = LevenshteinDistance(query, item);
                itemDistances[item] = distance;
            }

            // Sort the items by their distance to the query and take the top N
            var sortedItems = itemDistances.OrderBy(x => x.Value).Take(topN).Select(x => x.Key).ToList();

            return sortedItems;
        }

        public static List<string> FuzzySearch(string query, List<string> items, int maxDistance = 2)
        {
            List<string> closelyRelatedItems = new List<string>();

            foreach (string item in items)
            {
                int distance = LevenshteinDistanceMod(query, item);
                if (distance <= maxDistance)
                {
                    closelyRelatedItems.Add(item);
                }
            }

            return closelyRelatedItems;
        }

        public static int LevenshteinDistanceMod(string a, string b)
        {
            int lenA = a.Length;
            int lenB = b.Length;
            int[,] d = new int[lenA + 1, lenB + 1];

            for (int i = 0; i <= lenA; i++)
                d[i, 0] = i;

            for (int j = 0; j <= lenB; j++)
                d[0, j] = j;

            for (int i = 1; i <= lenA; i++)
            {
                for (int j = 1; j <= lenB; j++)
                {
                    int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[lenA, lenB];
        }

        public static List<string> NGramSearch(string query, List<string> items, int nGramSize)
        {
            List<string> closelyRelatedItems = new List<string>();

            foreach (string item in items)
            {
                double similarity = ComputeNGramSimilarity(query, item, nGramSize);

                if (similarity > 0.2)  // Set your own threshold
                {
                    closelyRelatedItems.Add(item);
                }
            }

            return closelyRelatedItems;
        }

        public static double ComputeNGramSimilarity(string a, string b, int n)
        {
            HashSet<string> aGrams = new HashSet<string>(GetNGrams(a, n));
            HashSet<string> bGrams = new HashSet<string>(GetNGrams(b, n));

            int intersection = aGrams.Intersect(bGrams).Count();

            return (double)intersection / (aGrams.Count + bGrams.Count - intersection);
        }

        public static List<string> GetNGrams(string input, int n)
        {
            List<string> nGrams = new List<string>();

            for (int i = 0; i <= input.Length - n; i++)
            {
                nGrams.Add(input.Substring(i, n));
            }

            return nGrams;
        }

    }

}









