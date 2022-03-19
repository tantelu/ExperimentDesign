using System.Collections.Generic;
using System.Drawing;
using System;

namespace ExperimentDesign.WorkList.ShowResult
{
    public class ColorBar
    {
        private List<Color> colors = new List<Color>();

        private static ColorBar _default;

        private static ColorBar _channel;

        private static ColorBar _well;

        private double min = -10;

        private double max = 10;
        public static ColorBar Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new ColorBar();
                    Color[] colors = new Color[21];
                    colors[0] = Color.FromArgb(10, 10, 152);
                    colors[1] = Color.FromArgb(10, 10, 195);
                    colors[2] = Color.FromArgb(10, 10, 247);
                    colors[3] = Color.FromArgb(10, 42, 255);
                    colors[4] = Color.FromArgb(10, 89, 255);
                    colors[5] = Color.FromArgb(10, 137, 255);
                    colors[6] = Color.FromArgb(10, 196, 247);
                    colors[7] = Color.FromArgb(10, 247, 255);
                    colors[8] = Color.FromArgb(42, 255, 247);
                    colors[9] = Color.FromArgb(89, 255, 199);
                    colors[10] = Color.FromArgb(137, 255, 152);
                    colors[11] = Color.FromArgb(191, 255, 97);
                    colors[12] = Color.FromArgb(247, 255, 42);
                    colors[13] = Color.FromArgb(255, 247, 10);
                    colors[14] = Color.FromArgb(255, 199, 10);
                    colors[15] = Color.FromArgb(255, 152, 10);
                    colors[16] = Color.FromArgb(255, 102, 10);
                    colors[17] = Color.FromArgb(255, 42, 10);
                    colors[18] = Color.FromArgb(247, 10, 10);
                    colors[19] = Color.FromArgb(199, 10, 10);
                    colors[20] = Color.FromArgb(152, 10, 10);
                    _default.colors.AddRange(colors);
                }
                return _default;
            }
        }

        public static ColorBar Well
        {
            get
            {
                if (_well == null)
                {
                    _well = new ColorBar();
                    Color[] colors = new Color[4];
                    colors[0] = Color.FromArgb(0, 0, 253);
                    colors[1] = Color.FromArgb(255, 12, 1);
                    colors[2] = Color.FromArgb(0, 102, 255);
                    colors[3] = Color.FromArgb(255, 0, 102);
                    _well.colors.AddRange(colors);
                    _well.min = 0;
                    _well.max = 3;
                }
                return _well;
            }
        }

        public static ColorBar Channel
        {
            get
            {
                if (_channel == null)
                {
                    _channel = new ColorBar();
                    Color[] colors = new Color[4];
                    colors[0] = Color.FromArgb(255, 255, 255);
                    colors[1] = Color.FromArgb(200, 200, 200);
                    colors[2] = Color.FromArgb(150, 150, 150);
                    colors[3] = Color.FromArgb(100, 100, 100);
                    _channel.colors.AddRange(colors);
                    _channel.min = 0;
                    _channel.max = 3;
                }
                return _channel;
            }
        }

        public Color GetColor(double v)
        {
            var x = (v - min) / (max - min);
            var colorindex = x * (colors.Count - 1);

            if (colorindex >= colors.Count - 1)
            {
                return colors[colors.Count - 1];
            }
            else if (colorindex < 0)
            {
                return colors[0];
            }
            else
            {
                int start = (int)(colorindex);
                double deta = colorindex - start;
                int maxindex = Math.Min(start + 1, colors.Count - 1);
                int A = (int)(colors[start].A + deta * (colors[maxindex].A - colors[start].A));
                int R = (int)(colors[start].R + deta * (colors[maxindex].R - colors[start].R));
                int G = (int)(colors[start].G + deta * (colors[maxindex].G - colors[start].G));
                int B = (int)(colors[start].B + deta * (colors[maxindex].B - colors[start].B));
                return Color.FromArgb(A,R,G,B);
            }
        }

        public void SetMinMax(double min, double max)
        {
            if (max - min > 0)
            {
                this.min = min;
                this.max = max;
            }
        }
    }
}
