using System;

namespace ParallelComputing_lab.Helper
{
    public class BorderInteger: IComparable<BorderInteger>
    {
        public int Value { get; set; }
        
        public int CompareTo(BorderInteger other)
        {
            return Value.CompareTo(other.Value);
        }

        public int Min() => int.MinValue;

        public int Max() => int.MaxValue;
    }
}