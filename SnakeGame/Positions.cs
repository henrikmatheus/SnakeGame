using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Positions
    {
        public int Row { get; }
        public int Col { get; }

        public Positions(int row, int col)
        {
            Row = row;
            Col = col;
        }
        //for create a position on the grid with plus Direction class
        public Positions Translate(Direction dir)
        {
            return new Positions(Row + dir.RowOffset, Col + dir.ColOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Positions positions &&
                   Row == positions.Row &&
                   Col == positions.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(Positions left, Positions right)
        {
            return EqualityComparer<Positions>.Default.Equals(left, right);
        }

        public static bool operator !=(Positions left, Positions right)
        {
            return !(left == right);
        }
    }
}
