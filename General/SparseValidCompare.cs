using System;
using System.Collections.Generic;

namespace ExperimentDesign.General
{
    public class SparseValidCompare
    {
        //保证gslib文件里面对应的xyz与PointSet里面的一致  Gslib
        /// <summary>
        /// 逐网格比较 1代表相同 0代表不同
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
                var vs = Gslib.ReadSgems(gslib, out xcount, out ycount, out zcount);
                for (int i = 0; i < sets.Count; i++)
                {
                    int index = sets[i].X + sets[i].Y * xcount + sets[i].Z * xcount * ycount;
                    if (Math.Abs(sets[i].V - vs[index]) < 0.0001)
                    {
                        compare.Add(1);
                    }
                    else
                    {
                        compare.Add(0);
                    }
                }
            }
            return compare;
        }

        /// <summary>
        /// 比较相比例 Item1井的相比例 Item2模型的相比例
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
                var vs = Gslib.ReadSgems(gslib, out xcount, out ycount, out zcount);
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
            return new Tuple<Dictionary<int, int>, Dictionary<int, int>>(compare1, compare2);
        }

        /// <summary>
        /// sets只为一口井 且默认Z连续  layergridnum多少个网格作为一层(gslib文件没有层的概念)
        /// </summary>
        /// <param name="sets"></param>
        /// <param name="gslib"></param>
        /// <returns>List.Count对应层数  Item1为井上厚度 Item2为模型厚度</returns>
        public static List<Tuple<int, int>> ThickAndLayerComparison(IList<PointSet> sets, string gslib, int layergridnum, int code)
        {
            var res = new List<Tuple<int, int>>();
            if (sets?.Count > 0)
            {
                int xcount;
                int ycount;
                int zcount;
                var vs = Gslib.ReadSgems(gslib, out xcount, out ycount, out zcount);
                int cur = 0;
                int thick_well = 0;
                int thick_model = 0;
                for (int i = 0; i < sets.Count; i++)
                {
                    cur++;
                    if (cur > layergridnum)
                    {
                        res.Add(new Tuple<int, int>(thick_well, thick_model));
                        cur = 0;
                        thick_well = 0;
                        thick_model = 0;
                    }
                    int index = sets[i].X + sets[i].Y * xcount + sets[i].Z * xcount * ycount;
                    if (sets[i].V == code)
                    {
                        thick_well++;
                    }
                    int facie = (int)Math.Round(vs[index]);
                    if (facie == code)
                    {
                        thick_model++;
                    }
                }
                if (cur > 0)
                {
                    res.Add(new Tuple<int, int>(thick_well, thick_model));
                }
            }
            return res;
        }
    }
}
