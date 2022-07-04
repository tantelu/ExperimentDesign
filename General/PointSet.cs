using System;

namespace ExperimentDesign.General
{
    public class PointSet
    {
        public PointSet(int x, int y, int z, int v)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.V = v;
        }

        public PointSet(Grid3D grid, double xd, double yd, double zd, int v)
        {
            if (xd >= grid.Xmin && xd < grid.Xmax && yd >= grid.Ymin && xd < grid.Ymax && zd >= grid.Zmin && zd < grid.Zmax)
            {
                this.X = (int)((xd - grid.Xmin) / grid.Xsize);
                this.Y = (int)((yd - grid.Ymin) / grid.Ysize);
                this.Z = (int)((zd - grid.Zmin) / grid.Zsize);
                this.V = v;
            }
            else
            {
                throw new ArgumentOutOfRangeException("超出网格范围的坐标无效");
            }
        }

        public int Id { get; set; } = -1;

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Z { get; private set; }

        public int V { get; set; }
    }
}
