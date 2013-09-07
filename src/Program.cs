using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NumberGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sokoban";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            String[] levelFiles = Directory.EnumerateFiles("levels").ToArray();

            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                int currentLevel = 0;

                DrawCenteredBox(62, 14, ConsoleColor.DarkYellow);
                DrawCenteredText("SOKOBAN", 10, ConsoleColor.Blue);
                DrawCenteredText("The classic moving box puzzle game", 12, ConsoleColor.Red);
                DrawCenteredText("INSERT COIN", 17, ConsoleColor.DarkRed);

                DrawCenteredText("By Andrew & Seb Thompson", 24, ConsoleColor.White);
                Console.ReadKey(true);

                while (true)
                {

                    
                    if (currentLevel == levelFiles.Length)
                    {
                        Console.Clear();
                        DrawCenteredBox(62, 14, ConsoleColor.DarkYellow);
                        DrawCenteredText(string.Format("SOKOBAN MASTER", currentLevel + 1), 8, ConsoleColor.Magenta);
                        DrawCenteredText("You have completed the game!", 12, ConsoleColor.White);
                        DrawCenteredText("Push any key to return to the title secreen", 17, ConsoleColor.DarkRed);
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    }
                    int[,] grid = LoadLevel(levelFiles[currentLevel]);


                    int xSize = grid.GetUpperBound(0) + 1;
                    int ySize = grid.GetUpperBound(1) + 1;

                    Console.Clear();
                    DrawCenteredBox(62, 14, ConsoleColor.DarkYellow);
                    DrawCenteredText(string.Format("LEVEL {0}",currentLevel+1), 10, ConsoleColor.Blue);
              
                    DrawCenteredText("Push any key to continue", 15, ConsoleColor.DarkRed);
                    Console.ReadKey(true);
                    Console.Clear();

                    int moveCount = 0;
                    while (true)
                    {
                        Draw(grid, xSize, ySize,currentLevel,moveCount);
                        if (!HandleInput(grid, xSize, ySize))
                        {
                            break;

                        }
                 
                        moveCount++;

                        bool goalFound = false;
                        for (int x = 0; x < xSize; x++)
                        {
                            for (int y = 0; y < ySize; y++)
                            {
                                if (grid[x, y] == 5 || grid[x, y] == 6)
                                {
                                    goalFound = true;
                                }
                            }
                        }
                        if (goalFound == false)
                        {

                            Console.Clear();
                            DrawCenteredBox(62, 14, ConsoleColor.DarkYellow);
                            DrawCenteredText(string.Format("LEVEL {0} COMPLETED", currentLevel + 1), 8, ConsoleColor.Magenta);
                            DrawCenteredText(string.Format("You did the level in {0} moves", moveCount), 12, ConsoleColor.White);
                            DrawCenteredText("Push any key to continue", 17, ConsoleColor.DarkRed);
                            Console.ReadKey(true);
                            Console.Clear();
                            currentLevel++;
                            break;
                        }
                    }
                }
            }
        }

        public static int[,] LoadLevel(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string[] lines = sr.ReadToEnd().Replace("\r\n","\n").Split('\n');
                
                int xSize = lines[0].Length;
                int ySize = lines.Length;

                int[,] level = new int[xSize,ySize];

                for (int y = 0; y < ySize; y++)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        if (lines[y][x] == '#')
                        {
                            level[x,y] = 0;
                        }

                        if (lines[y][x] == ' ')
                        {
                            level[x, y] = 1;
                        }

                        if (lines[y][x] == '@')
                        {
                            level[x, y] = 2;
                        }

                        if (lines[y][x] == '$')
                        {
                            level[x, y] = 3;
                        }

                        if (lines[y][x] == '*')
                        {
                            level[x, y] = 4;
                        }

                        if (lines[y][x] == '.')
                        {
                            level[x, y] = 5;
                        }

                        if (lines[y][x] == '+')
                        {
                            level[x, y] = 6;
                        }
                    }
                }
                return level;
            }
        }

        public static bool HandleInput(int[,] level, int xSize, int ySize)
        {
            ConsoleKey keyPressed = Console.ReadKey(true).Key;

            if (keyPressed == ConsoleKey.Escape)
                return false;

            int pX = 0;
            int pY = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (level[x, y] == 2 || level[x,y] == 6)
                    {
                        pX = x;
                        pY = y;
                    }
                }
            }

            if (keyPressed == ConsoleKey.UpArrow)
            {
                HandleDirection(level, pX, pY, 0, -1);
            }

            if (keyPressed == ConsoleKey.DownArrow)
            {
                HandleDirection(level, pX, pY, 0, +1);
            }
            if (keyPressed == ConsoleKey.LeftArrow)
            {
                HandleDirection(level, pX, pY, -1, 0);
            }
            if (keyPressed == ConsoleKey.RightArrow)
            {
                HandleDirection(level, pX, pY, +1, 0);
            }


            return true;
        }


        public static void HandleDirection(int[,] level,int pX,int pY,int xMod,int yMod)
        {
            if (level[pX + xMod, pY + yMod] == 1)
            {
                level[pX + xMod, pY + yMod] = 2;
                if (level[pX, pY] == 2) { level[pX, pY] = 1; }
                if (level[pX, pY] == 6) { level[pX, pY] = 5; }
            }

            if (level[pX + xMod, pY + yMod] == 5)
            {
                level[pX + xMod, pY + yMod] = 6;
                if (level[pX, pY] == 2) { level[pX, pY] = 1; }
                if (level[pX, pY] == 6) { level[pX, pY] = 5; }
            }

            if (level[pX + xMod, pY + yMod] == 3)
            {
                if (level[pX + (xMod * 2), pY + (yMod * 2)] == 1) 
                {
                    level[pX + (xMod * 2), pY + (yMod * 2)] = 3;
                    level[pX + xMod, pY + yMod] = 2;

                    if (level[pX, pY] == 2) { level[pX, pY] = 1; }
                    if (level[pX, pY] == 6) { level[pX, pY] = 5; }
                }

                if (level[pX + (xMod * 2), pY + (yMod * 2)] == 5)
                {
                    level[pX + (xMod * 2), pY + (yMod * 2)] = 4;
                    level[pX + xMod, pY + yMod] = 2;

                    if (level[pX, pY] == 2) { level[pX, pY] = 1; }
                    if (level[pX, pY] == 6) { level[pX, pY] = 5; }
                }  
            }

            if (level[pX + xMod, pY + yMod] == 4)
            {
                if (level[pX + (xMod * 2), pY + (yMod * 2)] == 1)
                {
                    level[pX + (xMod * 2), pY + (yMod * 2)] = 3;
                    level[pX + xMod, pY + yMod] = 6;

                    if (level[pX, pY] == 2) { level[pX, pY] = 1; }
                    if (level[pX, pY] == 6) { level[pX, pY] = 5; }
                }

                if (level[pX + (xMod * 2), pY + (yMod * 2)] == 5)
                {
                    level[pX + (xMod * 2), pY + (yMod * 2)] = 4;
                    level[pX + xMod, pY + yMod] = 6;

                    if (level[pX, pY] == 2) { level[pX, pY] = 1; }
                    if (level[pX, pY] == 6) { level[pX, pY] = 5; }
                }
            }
        }

        public static void DrawCenteredBox(int boxWidth,int boxHeight,ConsoleColor color)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            int boxOffsetX = (Console.WindowWidth / 2) - (boxWidth / 2);
            int boxOffsetY = (Console.WindowHeight / 2) - (boxHeight / 2);

            for (int x = 0; x < boxWidth; x++)
            {
                for (int y = 0; y < boxHeight; y++)
                {
                    if (x == 0 || x == boxWidth-1 || y== 0 || y == boxHeight-1)
                    {
                        Console.SetCursorPosition(x + boxOffsetX, y + boxOffsetY );
                        Console.Write("*");
                    }
                    
                }
            }

            Console.ForegroundColor = temp;
        }
        public static void DrawCenteredText(string text, int yPos, ConsoleColor color)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            int xPos = (Console.WindowWidth / 2) - (text.Length / 2);
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(text);
            Console.ForegroundColor = temp;
        }

        public static void Draw(int[,] level, int xSize, int ySize,int currentLevel,int moveCount)
        {



            DrawCenteredText(string.Format("LEVEL {0}", currentLevel + 1), 4, ConsoleColor.Blue);
            DrawCenteredBox(62, 14, ConsoleColor.DarkYellow);

            Console.SetCursorPosition(41, 19);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Press ESC to restart the level");

            Console.SetCursorPosition(9, 19);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Moves: {0}",moveCount);

            Console.ForegroundColor = ConsoleColor.White;


            int startX = 0;
            int startY = 0;

            int offsetX = (Console.WindowWidth / 2) - (xSize / 2);
            int offsetY = (Console.WindowHeight / 2) - (ySize / 2);
            for (int x = startX; x < xSize; x++)
            {
                for (int y = startY; y < ySize; y++)
                {
                    Console.SetCursorPosition(x+offsetX, y+offsetY);

                    if (level[x, y] == 0)
                    {
                        Console.Write("#");
                    }

                    if (level[x, y] == 1)
                    {
                        Console.Write(" ");
                    }

                    if (level[x, y] == 2 || level[x,y] == 6)
                    { 
                        ConsoleColor temp = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("@");
                        Console.ForegroundColor = temp;
                    }

                    if (level[x, y] == 3)
                    {
                        ConsoleColor temp = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("O");
                        Console.ForegroundColor = temp;
                    }

                    if (level[x, y] == 4)
                    {
                        ConsoleColor temp = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("O");
                        Console.ForegroundColor = temp;
                    }

                    if (level[x, y] == 5)
                    {
                        Console.Write("*");
                    }
                }

            }


        }
    }
}
