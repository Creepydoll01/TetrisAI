using System;
using System.Threading;
using System.Diagnostics;

namespace TETRISAI
{




    class Program
    {
  
        public static void DrawGraphics(GameState State)
        {
           
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (State.GameMap[i][j] == 0)
                        Console.Write("*" + " ");
                    else
                        Console.Write("■" + " ");
                }
                Console.WriteLine();
            }

        }

        public static void DrawNextFigure(GameState State)
        {
            Console.SetCursorPosition(25,0);
            Console.Write("Следующая фигура: ");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.SetCursorPosition(i + 30, j+2);
                    if (State.NextFigure.FigureShape[i][j] == 0)
                    {
                        
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write("■");
                    }
                }
                Console.WriteLine();
            }

        }

        static void DrawInfo(GameState State)
        {
            Console.SetCursorPosition(21, 7);
            Console.WriteLine("ID генома " + State.ListOfGenomes[State.CurrentGenome].ID);
            Console.SetCursorPosition(21, 8);
            Console.WriteLine("NumberOfRowsCleared " + State.ListOfGenomes[State.CurrentGenome].NumberOfRowsCleared);
            Console.SetCursorPosition(21, 9);
            Console.WriteLine("WeightedHeight " + State.ListOfGenomes[State.CurrentGenome].WeightedHeight);
            Console.SetCursorPosition(21, 10);
            Console.WriteLine("NumberOfMissingBlocks " + State.ListOfGenomes[State.CurrentGenome].NumberOfMissingBlocks);
            Console.SetCursorPosition(21, 11);
            Console.WriteLine("Roughness " + State.ListOfGenomes[State.CurrentGenome].Roughness);
            Console.SetCursorPosition(21, 12);
            Console.WriteLine("RelativeHeight " + State.ListOfGenomes[State.CurrentGenome].RelativeHeight);
            Console.SetCursorPosition(21, 13);

            Console.WriteLine("Номер Генома: " + State.CurrentGenome);
            Console.SetCursorPosition(21, 14);
            Console.WriteLine("Номер популяции: " + State.Generation);
            Console.SetCursorPosition(21, 15);
            Console.WriteLine("Количество сделанных шагов:" + State.MovesTaken);
            Console.SetCursorPosition(21, 16);
            Console.WriteLine("Количество очков:" + State.GameScore);
       


        }

        static void Main(string[] args)
        {
            Random Randomizer = new Random();
            Stopwatch watch = new Stopwatch();
            Console.WriteLine("Welcome to Tetris. Press Enter to Load Optimal Genome. Press Backspace to start learning from scratch. Press Escape to quit.");

            GameState State = new GameState();

            Console.CursorVisible = false;
           if(Console.ReadKey().Key==ConsoleKey.Enter)
            {
                AI.LoadOptimalPopulation(State);

                while(true)
                {
                    GameMechanics.GameEngine(State);
                    Console.SetCursorPosition(0, 0);
                    DrawGraphics(State);
                    DrawNextFigure(State);
                    DrawInfo(State);

                }

            }

            else if (Console.ReadKey().Key == ConsoleKey.Backspace)
            {
                AI.CreateFirstPopulation(State);

                while (true)
                {
                    GameMechanics.GameEngine(State);
                    Console.SetCursorPosition(0, 0);
                    DrawGraphics(State);
                    DrawNextFigure(State);
                    DrawInfo(State);

                }

            }



        }

        }
    }

