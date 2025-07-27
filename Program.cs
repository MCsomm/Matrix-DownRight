using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string filePath = args.Length > 0 ? args[0] : "matrix.txt";

        try
        {
            int[,] matrix = ReadMatrixFromFile(filePath);

            var result = FindMinimumPath(matrix);
            int minSum = result.minSum;
            List<(int, int)> path = result.path;

            Console.WriteLine($"Minimum path sum: {minSum}");
            Console.WriteLine("Path (row,col) → value:");

            foreach (var cell in path)
            {
                int row = cell.Item1;
                int col = cell.Item2;
                Console.WriteLine($"({row},{col}) → {matrix[row, col]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // 1️. Matrix Loader 
    static int[,] ReadMatrixFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        // Case 1: Single-line flat matrix
        if (lines.Length == 1)
        {
            string[] parts = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int total = parts.Length;
            double sqrt = Math.Sqrt(total);
            int size = (int)sqrt;

            if (size * size != total)
                throw new Exception($"Invalid matrix: {total} values can't form a square.");

            int[,] matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = int.Parse(parts[i * size + j]);
                }
            }

            return matrix;
        }

        // Case 2: Multi-line matrix
        int rowCount = lines.Length;
        int colCount = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
        int[,] matrixMulti = new int[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] values = lines[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length != colCount)
                throw new Exception($"Inconsistent column count on line {i + 1}.");

            for (int j = 0; j < colCount; j++)
            {
                matrixMulti[i, j] = int.Parse(values[j]);
            }
        }

        return matrixMulti;
    }

    // 2️ Prog
    static (int minSum, List<(int, int)> path) FindMinimumPath(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        int[,] cost = new int[rows, cols];
        (int, int)[,] parent = new (int, int)[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == 0 && j == 0)
                {
                    cost[i, j] = matrix[i, j];
                    parent[i, j] = (-1, -1);
                }
                else
                {
                    int fromTop = i > 0 ? cost[i - 1, j] : int.MaxValue;
                    int fromLeft = j > 0 ? cost[i, j - 1] : int.MaxValue;

                    if (fromTop < fromLeft)
                    {
                        cost[i, j] = fromTop + matrix[i, j];
                        parent[i, j] = (i - 1, j);
                    }
                    else
                    {
                        cost[i, j] = fromLeft + matrix[i, j];
                        parent[i, j] = (i, j - 1);
                    }
                }
            }
        }

        // 3️ Backtrack
        List<(int, int)> path = new List<(int, int)>();
        int r = rows - 1;
        int c = cols - 1;

        while (r >= 0 && c >= 0)
        {
            path.Add((r, c));
            var prev = parent[r, c];
            r = prev.Item1;
            c = prev.Item2;

            if (r == -1 && c == -1)
                break;
        }

        path.Reverse();
        return (cost[rows - 1, cols - 1], path);
    }
}
