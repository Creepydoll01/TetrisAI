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

            DrowInfo(State);
        }

        static void DrowInfo(GameState State)
        {
            Console.SetCursorPosition(30, 1);
            Console.WriteLine("ID генома " + State.ListOfGenomes[State.CurrentGenome].ID);
            Console.SetCursorPosition(30, 2);
            Console.WriteLine("NumberOfRowsCleared " + State.ListOfGenomes[State.CurrentGenome].NumberOfRowsCleared);
            Console.SetCursorPosition(30, 3);
            Console.WriteLine("WeightedHeight " + State.ListOfGenomes[State.CurrentGenome].WeightedHeight);
            Console.SetCursorPosition(30, 4);
            Console.WriteLine("NumberOfMissingBlocks " + State.ListOfGenomes[State.CurrentGenome].NumberOfMissingBlocks);
            Console.SetCursorPosition(30, 5);
            Console.WriteLine("Roughness " + State.ListOfGenomes[State.CurrentGenome].Roughness);
            Console.SetCursorPosition(30, 6);
            Console.WriteLine("RelativeHeight " + State.ListOfGenomes[State.CurrentGenome].RelativeHeight);
            Console.SetCursorPosition(30, 7);

            Console.WriteLine("Номер генома: " + State.CurrentGenome);
            Console.SetCursorPosition(30, 8);
            Console.WriteLine("Номер понуляции: " + State.Generation);
            Console.SetCursorPosition(30, 9);
            Console.WriteLine("Количество сделанных шагов" + State.MovesTaken);
            Console.SetCursorPosition(30, 10);
            Console.WriteLine("Количество очков " + State.GameScore);
            Console.SetCursorPosition(30,11);
            Console.WriteLine("Максимальное колво очков  " + maxgamescore);


        }

        static void DrawInfoAboutGenomes(GameState State)
        {
            foreach (Genomes item in State.ListOfGenomes)
            {
                Console.WriteLine(item.NumberOfRowsCleared);
                Console.WriteLine(item.WeightedHeight);
                Console.WriteLine(item.NumberOfMissingBlocks);
                Console.WriteLine(item.Roughness);
                Console.WriteLine(item.RelativeHeight);
                Console.WriteLine(item.ID);
                Console.WriteLine(State.CurrentGenome);
            }

        }





        static void Main(string[] args)
        {
            Random Randomizer = new Random();
            Stopwatch watch = new Stopwatch();


            GameState State = new GameState();
             //AI.CreateFirstPopulation(State);
            AI.LoadOptimalPopulation(State);

            foreach(Genomes item in State.ListOfGenomes)
            {
                Console.Write(item.NumberOfRowsCleared);
                Console.Write(item.WeightedHeight);
                Console.Write(item.NumberOfMissingBlocks);
                Console.Write(item.Roughness);
                Console.Write(item.RelativeHeight);
                Console.Write(item.ID);
                Console.WriteLine(State.CurrentGenome);
            }
            Console.ReadKey();



            ConsoleKeyInfo key = new ConsoleKeyInfo();
            Console.CursorVisible = false;
            DrawGraphics(State);
            Console.ReadKey();
            Console.Clear();
          
            while(true){
                Console.SetCursorPosition(0, 0);
                //Console.Clear();


                
                if (State.GameScore > maxgamescore)
                    maxgamescore = State.GameScore;

                GameMechanics.GameEngine(State);

                DrawGraphics(State);

            }

        }
    }
}
