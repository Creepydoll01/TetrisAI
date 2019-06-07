using System;
using System.Threading;
using System.Diagnostics;

namespace TETRISAI
{




    class Program
    {
        static int maxgamescore = 0;

        public static void DrawMap(int[][] GameMap)
        {

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                    Console.Write(GameMap[i][ j].ToString() + ' ');
                Console.WriteLine();
            }
        }



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

            Console.WriteLine("Номер генома: " + State.CurrentGenome);
            Console.SetCursorPosition(21, 14);
            Console.WriteLine("Номер понуляции: " + State.Generation);
            Console.SetCursorPosition(21, 15);
            Console.WriteLine("Количество сделанных шагов" + State.MovesTaken);
            Console.SetCursorPosition(21, 16);
            Console.WriteLine("Количество очков " + State.GameScore);
            Console.SetCursorPosition(21, 17);
            Console.WriteLine("Максимальное колво очков  " + maxgamescore);


        }

       


        static void Main(string[] args)
        {
            Random Randomizer = new Random();
            Stopwatch watch = new Stopwatch();


            GameState State = new GameState();
             //AI.CreateFirstPopulation(State);
            AI.LoadOptimalPopulation(State);

            ConsoleKeyInfo key = new ConsoleKeyInfo();
            Console.CursorVisible = false;
            DrawGraphics(State);
            Console.ReadKey();
            Console.Clear();
          
            while(true){
                Console.SetCursorPosition(0, 0);
                //Console.Clear();

                
                GameMechanics.GameEngine(State);
                
                DrawGraphics(State);
                DrawNextFigure(State);
                DrawInfo(State);
                Thread.Sleep(50);


            }

        }
    }
}
