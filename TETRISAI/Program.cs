using System;
using System.Threading;
using System.Diagnostics;

namespace Попыткасделатьтетрис2
{




    class Program
    {





        public static void DrawMap(int[,] GameMap)
        {



            for (int i = 0; i < GameMap.GetLength(0); i++)
            {
                for (int j = 0; j < GameMap.GetLength(1); j++)
                    Console.Write(GameMap[i, j].ToString() + ' ');
                Console.WriteLine();
            }
        }



        static void DrawGraphics(int[][] Gamemap)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Gamemap[i][j] == 0)
                        Console.Write("*" + " ");
                    else
                        Console.Write("■" + " ");
                }
                Console.WriteLine();
            }
        }





        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();


            GameState State = new GameState();
            AI.CreateFirstPopulation(State);




            ConsoleKeyInfo key = new ConsoleKeyInfo();
            Console.CursorVisible = false;
            for (int i = 0; i < 1000; i++)
            {
                watch.Start();
                Console.SetCursorPosition(0, 0);
                GameMechanics.GameEngine(State);




                DrawGraphics(State.GameMap);
                // Console.WriteLine("Количество шагов: " + State.MovesTaken);
                //Console.WriteLine("Номер генома: " + State.CurrentGenome);
                //Console.WriteLine("Номер поколения: " + State.Generation);
                //Console.WriteLine("Счёт: " + State.GameScore);
                //Console.WriteLine("Поражение " + State.Defeat);
                //Console.WriteLine("Двигалась ли " + State.FigureMoved);

                //Console.WriteLine(State.ListOfGenomes[State.CurrentGenome].GenomeRating);


            }
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);






        }
    }
}
