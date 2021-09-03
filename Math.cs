using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraLeague
{
    public static class LegMath
    {
        public static int GetBetween(this int value, int min, int max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static float GetBetween(this float value, float min, float max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static double GetBetween(this double value, double min, double max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        public static int GetIfLower(this int value, int min)
        {
            if (value < min)
                return min;
            else
                return value;
        }

        public static float GetIfLower(this float value, float min)
        {
            if (value < min)
                return min;
            else
                return value;
        }

        public static double GetIfLower(this double value, double min)
        {
            if (value < min)
                return min;
            else
                return value;
        }

        public static int GetIfHigher(this int value, int max)
        {
            if (value > max)
                return max;
            else
                return value;
        }

        public static float GetIfHigher(this float value, float max)
        {
            if (value > max)
                return max;
            else
                return value;
        }

        public static double GetIfHigher(this double value, double max)
        {
            if (value > max)
                return max;
            else
                return value;
        }
    }
}
