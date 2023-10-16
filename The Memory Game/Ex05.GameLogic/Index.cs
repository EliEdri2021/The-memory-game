#pragma warning disable CS0659
#pragma warning disable CS0661
namespace Ex05.GameLogic
{
    internal readonly struct Index
    {
        private readonly int r_Column;
        private readonly int r_Row;

        public Index(int i_Column, int i_Row)
        {
            this.r_Column = i_Column;
            this.r_Row = i_Row;
        }

        public static bool operator ==(Index i_IndexA, Index i_IndexB)
        {
            return i_IndexA.GetRow() == i_IndexB.GetRow() && i_IndexA.GetColumn() == i_IndexB.GetColumn();
        }

        public static bool operator !=(Index i_IndexA, Index i_IndexB)
        {
            return !(i_IndexA == i_IndexB);
        }

        public int GetColumn()
        {
            return this.r_Column;
        }

        public int GetRow()
        {
            return this.r_Row;
        }

        public override bool Equals(object i_ObjectOther)
        {
            return i_ObjectOther is Index indexOther && this == indexOther;
        }

        public override string ToString()
        {
            return $"Index | column (x) : {this.GetColumn()} , row (y) : {this.GetRow()}";
        }
    }
}
