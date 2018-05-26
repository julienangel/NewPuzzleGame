using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSolver {

    const int MaxSteps = 20;
    PieceType[,] board;
    List<Tuple<int, int>> goals = new List<Tuple<int, int>>();

    struct BoardStatus
    {
        public List<InputHandler.MoveDirection> moves;
        public LevelSolver board;
    }

    public LevelSolver(int w, int h, params PieceType[] pieces)
    {
        board = new PieceType[h, w];
        int k = 0;
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (k < pieces.Length)
                {
                    board[i, j] = pieces[k];
                    k++;
                }
            }
        }
    }

    public bool IsSolution()
    {
        return goals.Select(point => board[point.Item1, point.Item2] == PieceType.goal).Aggregate((a, b) => a && b);
    }

    public LevelSolver Clone()
    {
        LevelSolver n = new LevelSolver(board.GetLength(0), board.GetLength(1), board.Cast<PieceType>().ToArray());
        n.SetGoals(goals.ToArray());
        return n;
    }

    public void SetGoals(params Tuple<int, int>[] g)
    {
        goals = new List<Tuple<int, int>>(g);
    }

    public void Show()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                switch (board[i, j])
                {
                    case PieceType.empty: Console.Write("_"); break;
                    case PieceType.statice: Console.Write("X"); break;
                    case PieceType.normal: Console.Write("#"); break;
                    case PieceType.goal: Console.Write("G"); break;
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("Goals: ");
        if (goals.Count > 0)
        {
            Console.WriteLine(String.Join(" ", goals.Select(pt => pt.Item1 + "," + pt.Item2)));
        }
    }

    public LevelSolver Move(InputHandler.MoveDirection move)
    {
        switch (move)
        {
            case InputHandler.MoveDirection.Up: return Move(-1, 0);
            case InputHandler.MoveDirection.down: return Move(1, 0);
            case InputHandler.MoveDirection.left: return Move(0, -1);
            case InputHandler.MoveDirection.right: return Move(0, 1);
            default: return Move(0, 0); // never happens
        }
    }

    LevelSolver Move(int y, int x)
    {
        int xFrom, xTo;
        int yFrom, yTo;

        xFrom = x < 0 ? 1 : (x > 0) ? board.GetLength(1) - 1 : 0;
        xTo = x < 0 ? board.GetLength(1) : (x > 0) ? -1 : 0;   // Last + 1

        yFrom = y < 0 ? 1 : (y > 0) ? board.GetLength(0) - 1 : 0;
        yTo = y < 0 ? board.GetLength(0) : (y > 0) ? -1 : 0; // last + 1;

        if (x != 0)
        {
            for (int i = yFrom; i != board.GetLength(0); i++)
            {
                for (int j = xFrom; j != xTo; j -= x)
                {
                    if (board[i, j] == PieceType.normal || board[i, j] == PieceType.goal)
                    {
                        int k = j + x;
                        while (k < board.GetLength(1) && k >= 0 && board[i, k] == PieceType.empty)
                        {
                            PieceType temp = board[i, k - x];
                            board[i, k - x] = board[i, k];
                            board[i, k] = temp;
                            k += x;
                        }
                    }
                }
            }
        }

        if (y != 0)
        {
            for (int i = yFrom; i != yTo; i -= y)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == PieceType.normal || board[i, j] == PieceType.goal)
                    {
                        int l = i + y;
                        while (l >= 0 && l < board.GetLength(0) && board[l, j] == PieceType.empty)
                        {
                            PieceType temp = board[l - y, j];
                            board[l - y, j] = board[l, j];
                            board[l, j] = temp;
                            l += y;
                        }
                    }
                }
            }
        }

        return this;
    }

    public bool EqualsTo(LevelSolver other)
    {
        return other.board.Cast<PieceType>().ToArray().Zip(
          board.Cast<PieceType>().ToArray(),
            (a, b) => a == b
        ).Aggregate((a, b) => a && b);
    }

    public List<InputHandler.MoveDirection> Solve()
    {
        List<LevelSolver> visited = new List<LevelSolver>();
        List<BoardStatus> queue = new List<BoardStatus>();

        foreach (InputHandler.MoveDirection move in Enum.GetValues(typeof(InputHandler.MoveDirection)))
        {
            queue.Add(
                new BoardStatus
                {
                    moves = new List<InputHandler.MoveDirection>(new InputHandler.MoveDirection[] { move }),
                    board = Clone().Move(move)
                });
        }

        while (queue.Count > 0)
        {
            BoardStatus bs = queue[0];
            queue.RemoveAt(0);
            visited.Add(bs.board);

            if (bs.board.IsSolution()) return bs.moves;

            if (bs.moves.Count < MaxSteps)
            {
                foreach (InputHandler.MoveDirection move in Enum.GetValues(typeof(InputHandler.MoveDirection)))
                {
                    LevelSolver n = bs.board.Clone().Move(move);

                    if (!visited.Exists(b => b.EqualsTo(n)) && !queue.Exists(b => b.board.EqualsTo(n)))
                    {
                        List<InputHandler.MoveDirection> m = new List<InputHandler.MoveDirection>(bs.moves.ToArray());
                        m.Add(move);
                        queue.Add(
                            new BoardStatus
                            {
                                moves = m,
                                board = n
                            }
                        );
                    }
                }
            }
        }

        return new List<InputHandler.MoveDirection>();
    }
}
