using System;
using UnityEngine;

public static class TicTacToeAI
{
    public static (int row, int col)? GetBestMove(Constants.PlayerType[,] board)
    {
        float bestScore = float.MinValue;
        (int row, int col) bestMove = (-1, -1);

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for ( int col= 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == Constants.PlayerType.None)
                {
                    board[row, col] = Constants.PlayerType.Player2; // Turn AI
                    float score = DoMinimax(board, 0, false);
                    board[row, col] = Constants.PlayerType.None;    // return
                    
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }
        }
        if (bestMove != (-1,- 1)) return bestMove;

        return null;
    }

    private static float DoMinimax(Constants.PlayerType[,] board, int depth, bool isMaximizing)
    {
        if(CheckGameWin(Constants.PlayerType.Player1, board)) return -10 + depth;
        if(CheckGameWin(Constants.PlayerType.Player2, board)) return 10 - depth;
        if(CheckGameDraw(board)) return 0;

        if (isMaximizing)
        {
            float bestScore = float.MinValue;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == Constants.PlayerType.None)
                    {
                        board[row, col] = Constants.PlayerType.Player2; // Turn AI
                        float score = DoMinimax(board, depth + 1, false);
                        board[row, col] = Constants.PlayerType.None;
                        bestScore = MathF.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            float bestScore = float.MaxValue;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == Constants.PlayerType.None)
                    {
                        board[row, col] = Constants.PlayerType.Player1; // Turn Player
                        float score = DoMinimax(board, depth + 1, true);
                        board[row, col] = Constants.PlayerType.None;    // return
                        bestScore = MathF.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    // Method for checking game winning conditions
    public static bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == playerType &&
                board[row, 1] == playerType &&
                board[row, 2] == playerType)
            {
                return true;
            }
        }

        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == playerType &&
                board[1, col] == playerType &&
                board[2, col] == playerType)
            {
                return true;
            }
        }

        if (board[0,0] == playerType &&
            board[1,1] == playerType &&
            board[2,2] == playerType)
        {
            return true;
        }

        if (board[0,2] == playerType &&
            board[1,1] == playerType &&
            board[2,0] == playerType)
        {
            return true;
        }

        return false;
    }

    public static bool CheckGameDraw(Constants.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == Constants.PlayerType.None) return false;
            }
        }

        return true;
    }
}
