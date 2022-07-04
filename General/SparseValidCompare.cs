using System;
using System.Collections.Generic;

namespace ExperimentDesign.General
{
    public class SparseValidCompare
    {
        //保证gslib文件里面对应的xyz与PointSet里面的一致  Gslib
        /// <summary>
        /// 逐网格比较 1代表相同 -1代表不同
        /// </summary>
        /// <param name="gslib"></param>
        /// <returns></returns>
        public static IList<int> GridByGridComparison(IList<PointSet> sets, string gslib)
        {
            List<int> compare = new List<int>();
            if (sets?.Count > 0)
            {
                compare.Capacity = sets.Count;
                int xcount;
                int ycount;
                int zcount;
                var vs = Gslib.ReadGslib(gslib, out xcount, out ycount, out zcount);
                for (int i = 0; i < sets.Count; i++)
                {
                    int index = sets[i].X + sets[i].Y * xcount + sets[i].Z * xcount * ycount;
                    if (Math.Abs(sets[i].V - vs[index]) < 0.0001)
                    {
                        compare.Add(1);
                    }
                    else
                    {
                        compare.Add(-1);
                    }
                }
            }
            return compare;
        }

        /// <summary>
        /// 比较相比例
        /// </summary>
        /// <param name="sets"></param>
        /// <param name="gslib"></param>
        /// <returns></returns>
        public static Tuple<Dictionary<int, int>, Dictionary<int, int>> FacieProportionComparison(IList<PointSet> sets, string gslib)
        {
            Dictionary<int, int> compare1 = new Dictionary<int, int>();
            Dictionary<int, int> compare2 = new Dictionary<int, int>();
            if (sets?.Count > 0)
            {
                int xcount;
                int ycount;
                int zcount;
                var vs = Gslib.ReadGslib(gslib, out xcount, out ycount, out zcount);
                for (int i = 0; i < sets.Count; i++)
                {
                    int index = sets[i].X + sets[i].Y * xcount + sets[i].Z * xcount * ycount;
                    if (compare1.ContainsKey(sets[i].V))
                    {
                        compare1[sets[i].V]++;
                    }
                    else
                    {
                        compare1.Add(sets[i].V, 1);
                    }
                    int facie = (int)Math.Round(vs[index]);
                    if (compare2.ContainsKey(facie))
                    {
                        compare2[facie]++;
                    }
                    else
                    {
                        compare2.Add(facie, 1);
                    }
                }
            }
            return new Tuple<Dictionary<int, int>, Dictionary<int, int>>(compare1,compare2);
        }

        /// <summary>
        /// sets只为一口井 且默认Z连续
        /// </summary>
        /// <param name="sets"></param>
        /// <param name="gslib"></param>
        /// <returns></returns>
        public static Tuple<List<int>, List<int>> ThickAndLayerComparison(IList<PointSet> sets, string gslib)
        {
            List<int> compare1 = new List<int>();
            List<int> compare2 = new List<int>();
            int last1 = -99;
            int cur1 = 0;
            int last2 = -99;
            int cur2 = 0;
            if (sets?.Count > 0)
            {
                int xcount;
                int ycount;
                int zcount;
                var vs = Gslib.ReadGslib(gslib, out xcount, out ycount, out zcount);
                for (int i = 0; i < sets.Count; i++)
                {
                    int index = sets[i].X + sets[i].Y * xcount + sets[i].Z * xcount * ycount;
                    if (sets[i].V==last1)
                    {
                        cur1++;
                    }
                    else
                    {
                        compare1.Add(cur1);
                        cur1 = 1;
                    }
                    last1 = sets[i].V;
                    int facie = (int)Math.Round(vs[index]);
                    if (facie==last2)
                    {
                        cur2++;
                    }
                    else
                    {
                        compare2.Add(cur2);
                        cur2 = 1;
                    }
                    last2 = facie;
                }
            }
            return new Tuple<List<int>, List<int>>(compare1, compare2);
        }
    }
}
