using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }
        private readonly LinkedList<Positions> snakePositions = new LinkedList<Positions>();
        private readonly Random random = new Random();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            AddSnake();
            AddFood();
            
        }

        public void AddSnake()
        {
            int r = Rows / 2;
            for(int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.snake;
                snakePositions.AddFirst(new Positions(r, c));
            }
        }
        //for food (start)
        private IEnumerable<Positions> EmptyPositions()
        {
            for(int r = 0; r < Rows; r++)
            {
                for(int c = 0; c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Positions(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Positions> empty = new List<Positions>(EmptyPositions());

            if(empty.Count == 0)
            {
                return; 
            }

            Positions pos = empty[random.Next(empty.Count)]; //here the pos variable generates
            // a random number when gets the empty.Count value.
            Grid[pos.Row, pos.Col] = GridValue.Food;
        }//for food (finish)

        public Positions HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Positions TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Positions> SnakePositions()
        {
            return snakePositions;
        }

        private void AddHead(Positions pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.snake;
        }

        private void RemoveTail()
        {
            Positions tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        public void ChangeDirection(Direction dir)
        {
            Dir = dir;
        }

        private bool OutsideGrid(Positions pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue WillHit(Positions newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if(newHeadPos == TailPosition()) //head in the same square of tail
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void Move()
        {
            Positions newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);
            if(hit == GridValue.Outside || hit == GridValue.snake)
            {
                GameOver = true;
            }
            else if(hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if(hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();

            }
        }
    }
}
