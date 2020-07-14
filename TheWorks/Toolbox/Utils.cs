using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorks.Toolbox
{
    public class Utils
    {

        public static int[] GetRandomIndexes(int amount, int max)
        {
            List<int> result = new List<int>();
            Random r = new Random();
            while (result.Count < amount)
            {
                var res = r.Next(0, max - 1);
                if (!result.Contains(res))
                    result.Add(res);
            }

            return result.ToArray();
        }

        public static List<int> GetCombinations(bool withPhone, int combinationLength)
        {
            List<int> result = new List<int>();
            if (combinationLength == 1)
                result = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            if (combinationLength == 2)
                result = new List<int> { 12, 23, 34, 45, 56, 67, 78, 13, 24, 35, 46, 57, 68, 14, 25, 36, 47, 58,
                    15, 26, 37, 48, 16, 27, 38, 17, 28, 18};
            if (combinationLength == 3)
                result = new List<int> { 123, 134, 145, 156, 167, 178, 124, 135, 146, 157, 168, 125, 136, 147, 158,
                    126, 137, 148, 127, 138, 128, 234, 245, 256, 267, 278, 235, 246, 257, 268, 236, 247, 258,
                    237, 248, 238, 345, 356, 367, 378, 346, 357, 368, 347, 358, 348, 456, 467, 478, 457, 468,
                    458, 567, 578, 568};
            if (combinationLength == 4)
                result = new List<int> { 1234, 1345, 1456, 1567, 1678, 1235, 1346, 1457, 1568, 1236, 1347, 1458,
                    1237, 1348, 1238, 2345, 2456, 2567, 2678, 2346, 2457, 2568, 2347, 2458, 2348, 3456, 3567, 3678,
                    3457, 3568, 3458, 4567, 4678, 4568};
            if (combinationLength == 5)
                result = new List<int> { 12345, 12356, 12367, 12378, 12346, 12357, 12368, 12347, 12358,
                    12348, 23456, 23467, 23478, 23457, 23468, 23458, 34567, 34678, 34568};
            if (combinationLength > 5)
                result = new List<int> { 123456, 123467, 123478, 123457, 123468, 123458, 234567, 234578, 234568};

            if (!withPhone)
                return result.Where(r => !r.ToString().Contains("5")).ToList();
            else return result.ToList();
        }
    }
}
