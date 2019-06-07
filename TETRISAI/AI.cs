using System.Collections.Generic;
using System.Linq;
using System;

namespace TETRISAI

{



    public class AI

    {
        static GameState SavedState = new GameState();
        static Random Randomizer = new Random();

        //Загружает оптимальный геном
        public static void LoadOptimalPopulation(GameState State)
        {

            for(int i = 0; i<State.SizeOfPopulation;i++)
            {
                Genomes Genome = new Genomes();
                
                Genome.ID = 1;
                
                Genome.NumberOfRowsCleared = 0.22568649650722883;
                Genome.WeightedHeight = 0.08679520494876472;
                Genome.CumulativeHeight = 0.6152727732730796;
                Genome.RelativeHeight = 0.15842464424735841;
                Genome.NumberOfMissingBlocks = 0.15452215909537684;
                Genome.Roughness = 0.021586109522043928;
                Genome.GenomeRating = 100000;

                State.ListOfGenomes.Add(Genome);
            }

            EvaluateNextGenome(State);
        }
        
        public static void CreateFirstPopulation(GameState State)
        {
            //Мы заполняем нашу популяцию первоначальными геномами, генерируя их в конструкторе
            for (int i = 0; i < State.SizeOfPopulation; i++)
            {
                Genomes Genome = new Genomes();
                Genome.ID = Randomizer.NextDouble();
                Genome.NumberOfRowsCleared = Randomizer.NextDouble() - State.MutationRate;
                Genome.WeightedHeight = Randomizer.NextDouble() - State.MutationRate;
                Genome.CumulativeHeight = Randomizer.NextDouble() - State.MutationRate;
                Genome.RelativeHeight = Randomizer.NextDouble() - State.MutationRate;
                Genome.NumberOfMissingBlocks = Randomizer.NextDouble() * State.MutationRate;
                Genome.Roughness = Randomizer.NextDouble() - State.MutationRate;
 
                State.ListOfGenomes.Add(Genome);
            }

            //Оцениваем первый геном
            EvaluateNextGenome(State);
        }
        

        public static void EvaluateNextGenome(GameState State)
        {
            //Переходим к следующему геному
            State.CurrentGenome++;
            //Проверяем, остались ли геномы в популяции. Если не остались - нужно эволюционировать

            if (State.CurrentGenome == State.ListOfGenomes.Count)
            {
                Evlove(State);
            }

            //Поскольку мы перешли к новому геному, необходимо обнулить число сделанных шагов и сделать следующий шаг
            State.MovesTaken = 0;
            MakeNextMove(State);
        }


        public static void Evlove(GameState State)
        {
            // Мы прошли все геномы - пора возвращаться к нулевому значению.

            State.CurrentGenome = 0;
            State.Generation++;
            State.ResetGameState();

            GameState StateOfRound = new GameState();
            StateOfRound.SaveGameState(State);
            //Сортируем наши геномы по убыванию рейтинга
            State.ListOfGenomes=State.ListOfGenomes.OrderByDescending(x => x.GenomeRating).ToList();

            //Добавим самый хороший геном в список кандидатов на скрещивание
            

            //Удалим половину самых плохих геномов

            while(State.ListOfGenomes.Count>State.SizeOfPopulation/2)
            {
                State.ListOfGenomes.RemoveAt(State.ListOfGenomes.Count - 1);
            }

            int TotalGenomeRating = 0;

            //Считаем сумму всех хороших геномов
            for(int i = 0; i<State.ListOfGenomes.Count;i++)
            {
                TotalGenomeRating = State.ListOfGenomes[i].GenomeRating;
            }

            //Создаем список дочерних геномов

            List<Genomes> Children = new List<Genomes>();

            Children.Add(State.ListOfGenomes[0]);
            while(Children.Count< State.SizeOfPopulation)
            {
                Children.Add(MakeChild(PickRandomGenome(State), PickRandomGenome(State), State));
            }

            State.ListOfGenomes = new List<Genomes>();

            foreach(Genomes item in Children)
            {
                State.ListOfGenomes.Add(item);
            }


        }
           
        
            //Скрещиваем два старых генома для создания нового
        private static Genomes MakeChild(Genomes Parent1, Genomes Parent2, GameState State)
        {

            //Создаем новый экземпляр класса Genomes, заполняем его значениями, зависящими от родительских геномов
            Genomes Child = new Genomes();

            Child.ID = Randomizer.NextDouble();
            Child.NumberOfRowsCleared = GetRandomParent(Parent1, Parent2).NumberOfRowsCleared;
            Child.WeightedHeight = GetRandomParent(Parent1, Parent2).WeightedHeight;
            Child.CumulativeHeight = GetRandomParent(Parent1, Parent2).CumulativeHeight;
            Child.RelativeHeight = GetRandomParent(Parent1, Parent2).RelativeHeight;
            Child.NumberOfMissingBlocks = GetRandomParent(Parent1, Parent2).NumberOfMissingBlocks;
            Child.Roughness = GetRandomParent(Parent1, Parent2).Roughness;
            Child.GenomeRating = -1;



            if (Randomizer.NextDouble() < State.MutationRate)
            {
                Child.WeightedHeight += Randomizer.NextDouble() * State.MutationStep * 2 - State.MutationStep;
            }

            if (Randomizer.NextDouble() < State.MutationRate)
            {
                Child.CumulativeHeight += Randomizer.NextDouble() * State.MutationStep * 2 - State.MutationStep;
            }

            if (Randomizer.NextDouble() < State.MutationRate)
            {
                Child.RelativeHeight += Randomizer.NextDouble() * State.MutationStep * 2 - State.MutationStep;
            }

            if (Randomizer.NextDouble() < State.MutationRate)
            {
                Child.NumberOfMissingBlocks += Randomizer.NextDouble() * State.MutationStep * 2 - State.MutationStep;
            }

            if (Randomizer.NextDouble() < State.MutationRate)
            {
                Child.NumberOfRowsCleared += Randomizer.NextDouble() * State.MutationStep * 2 - State.MutationStep;
            }

            if (Randomizer.NextDouble() < State.MutationRate)
            {
                Child.Roughness += Randomizer.NextDouble() * State.MutationStep * 2 - State.MutationStep;
            }


            return Child;

        }
        

        //Возвращает случайный родительский геном, один из двух
        public static Genomes GetRandomParent(Genomes Parent_1, Genomes Parent_2)
        {
            if (Randomizer.NextDouble() > 0.5)
            {
                return Parent_1;
            }

            else
            {
                return Parent_2;
            }
        }

        //Возвращает случайный геном, один из массива 
       public static Genomes PickRandomGenome(GameState State)
        {

            return State.ListOfGenomes[Randomizer.Next(State.ListOfGenomes.Count - 1)];
        }

        //Функция, которая выбирает и совершает следующий игровой шаг
        static public void MakeNextMove(GameState State)
        {
            
            //Увеличим количество сделанных шагов
            State.MovesTaken++;
            //Сохраним наше состояние, чтобы можно было безбоязненно определить список возможных шагов
            SavedState.SaveGameState(State);
            List<Moves> ListOfMoves = new List<Moves>();

            //Смотрим, какие шаги мы можем сделать с нынешней фигурой
             ListOfMoves = GetAllPossibleMoves(State);
            //Теперь перейдем к следующей фигуре, чтобы понять, какой из нынешних ходов приведет к наиболее оптимальному результату при учете, что и следующим ходом будет наиболее оптимальный
            State.GenerateNextFigure();

            for (int i = 0; i < ListOfMoves.Count; i++)
            {
                Moves NextMove = new Moves();
               NextMove = CalculateHighestRatedMoveNew(GetAllPossibleMoves(State));
                ListOfMoves[i].Rating += NextMove.Rating;
            }
            //Теперь найдем самый выгодный ход из всех возможных
            Moves MoveToMake = new Moves();
            MoveToMake = CalculateHighestRatedMoveNew(ListOfMoves);

            //Самый выгодный ход найден. Теперь можно загрузить первоначальное состояние и сделать наконец этот ход

           State.LoadGameState(SavedState);

            //Выполняем выбранный ход

            for (int Rotations = 0; Rotations < MoveToMake.NumberOfRotations; Rotations++)
            {
                GameMechanics.RotateShape(State);
            }
            // Двигаем влево
            if (MoveToMake.Shift < 0)
            {
                for (int Left = 0; Left < Math.Abs(MoveToMake.Shift); Left++)
                {
                    GameMechanics.MoveLeft(State);
                }
            }
            // Двигаем вправо
            else if (MoveToMake.Shift > 0)
            {
                for (int Right = 0; Right < MoveToMake.Shift; Right++)
                {
                    GameMechanics.MoveRight(State);
                }
            }

        }
       

        //Функция создает список всех возможных ходов
        public static List<Moves> GetAllPossibleMoves(GameState State)
        {
            
            //Перед выполнением всех процедур нужно сохранить игровое состояние. Далее все действия будем выполнять с ним

            GameState SavedState = new GameState();
            SavedState.SaveGameState(State);

            //Создаем списки движений и соответствующих им рейтингов

            List<Moves> ListOfPossibleMoves = new List<Moves>();
            List<double> PossibleMoveRatings = new List<double>();

            //Для каждого возможного количества вращений...
            for(int Rotations = 0; Rotations<4;Rotations++)
            {
                List<int> OldX = new List<int>();

                    for(int x = -5; x<=5;x++)
                     {
                           //Загружаем сохраненное состояние
                           State.LoadGameState(SavedState);
                            
                            //Вращаем
                            for(int j = 0; j<Rotations;j++)
                              {
                                    GameMechanics.RotateShape(State);
                              }

                            //Двигаем влево
                             if(x<0)
                             {        
                                for(int l = 0; l<Math.Abs(x);l++)
                                 {
                                      GameMechanics.MoveLeft(State);
                                   }
                             }
                             //Или вправо
                             else if(x>0)
                            {
                               for(int r = 0; r<x;r++)
                                {
                                     GameMechanics.MoveRight(State);
                                }
                            }

                    //Если фигура двигалась, то надо еще подвинуть, пока перестанет двигаться (встанет на свое место)

                    if (!OldX.Contains(State.CurrentFigure.X))
                    {
                        MovementResults MoveResult = new MovementResults();

                        MoveResult = GameMechanics.MoveDown(State);

                        while (MoveResult.MoveMade)
                        {
                            MoveResult = GameMechanics.MoveDown(State);
                        }

                        // Создаем новый экземпляр класса Move - наш новый ход

                        Moves Move = new Moves();
                        //Заполняем его значения
                        Move.NumberOfRowsCleared = MoveResult.RowsCleared;
                        Move.WeightedHeight = Math.Pow(CalculateHeight(State), 1.5);
                        Move.CumulativeHeight = CalculateCumulativeHeight(State);
                        Move.RelativeHeight = CalculateRelativeHeight(State);
                        Move.NumberOfMissingBlocks = CountMissingBlocks(State);
                        Move.Roughness = CalculateRoughness(State);

                        
                        //Производим оценку

                        Move.Rating += Move.NumberOfRowsCleared * State.ListOfGenomes[State.CurrentGenome].NumberOfRowsCleared;
                        Move.Rating -= Move.WeightedHeight * State.ListOfGenomes[State.CurrentGenome].WeightedHeight;
                        Move.Rating -= Move.CumulativeHeight * State.ListOfGenomes[State.CurrentGenome].CumulativeHeight;
                        Move.Rating -= Move.RelativeHeight * State.ListOfGenomes[State.CurrentGenome].RelativeHeight;
                        Move.Rating -= Move.NumberOfMissingBlocks * State.ListOfGenomes[State.CurrentGenome].NumberOfMissingBlocks;
                        Move.Rating -= Move.Roughness * State.ListOfGenomes[State.CurrentGenome].Roughness;

                        //Если шаг проиграл игру, то он очень плохой. Уменьшаем его рейтинг. 
                        if (MoveResult.Defeat)
                        {
                            Move.Rating -= 500;
                        }

                        Move.NumberOfRotations = Rotations;
                        Move.Shift = x;

                        ListOfPossibleMoves.Add(Move);
                   
                        OldX.Add(State.CurrentFigure.X);

                    }

                }

            }

                   //Загружаем сохраненное состояние и возвращаем список шагов
                 State.LoadGameState(SavedState);         
                 return ListOfPossibleMoves;
            }




        

        // Далее идут вспомогательные функции, использующиеся в методе GetAllPossibleMoves. Они нужны для расчетов рейтинга шага
        private static int CalculateCumulativeHeight(GameState State)
        {
            GameMechanics.RemoveShape(State);

            int[] MaxHeights = new int[] { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
            for (int rows = 0; rows < 20; rows++)
            {
                for (int columns = 0; columns < 10; columns++)
                {
                    if (State.GameMap[rows][columns] != 0 && MaxHeights[columns] == 20)
                    {
                        MaxHeights[columns] = rows;
                    }
                }
            }

            int TotalHeight = 0;
            for (int i = 0; i < MaxHeights.GetLength(0); i++)
            {
                TotalHeight += 20 - MaxHeights[i];
            }

            GameMechanics.DrawShape(State);

            return TotalHeight;
        }

        private static int CountMissingBlocks(GameState State)
        {
            GameMechanics.RemoveShape(State);

            int[] MaxHeights = new int[] { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };

            for (int rows = 0; rows < 20; rows++)
            {
                for (int columns = 0; columns < 10; columns++)
                {
                    if (State.GameMap[rows][columns] != 0 && MaxHeights[columns] == 20)
                    {
                        MaxHeights[columns] = rows;
                    }
                }
            }

            int AmountOfHoles = 0;

            for (int i = 0; i < MaxHeights.GetLength(0); i++)
            {
                for (int j = MaxHeights[i]; j < 20; j++)
                {
                    if (State.GameMap[j][i] == 0)
                    {
                        AmountOfHoles++;
                    }
                }
            }

            GameMechanics.DrawShape(State);
            return AmountOfHoles;

        }

        private static double CalculateRoughness(GameState State)
        {

            GameMechanics.RemoveShape(State);

            int[] MaxHeights = new int[] { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
            for (int rows = 0; rows < 20; rows++)
            {
                for (int columns = 0; columns < 10; columns++)
                {
                    if (State.GameMap[rows][columns] != 0 && MaxHeights[columns] == 20)
                    {
                        MaxHeights[columns] = rows;
                    }
                }
            }

            double Roughness = 0;

            for (int i = 0; i < MaxHeights.GetLength(0) - 1; i++)
            {
                Roughness += Math.Abs(MaxHeights[i] - MaxHeights[i + 1]);
            }

            GameMechanics.DrawShape(State);

            return Roughness;

        }

        private static int CalculateRelativeHeight(GameState State)
        {
            GameMechanics.RemoveShape(State);

            int[] MaxHeights = new int[] { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
            for (int rows = 0; rows < 20; rows++)
            {
                for (int columns = 0; columns < 10; columns++)
                {
                    if (State.GameMap[rows][columns] != 0 && MaxHeights[columns] == 20)
                    {
                        MaxHeights[columns] = rows;
                    }
                }
            }

            GameMechanics.DrawShape(State);

            return (MaxHeights.Max() - MaxHeights.Min());
        }

        private static int CalculateHeight(GameState State)
        {
            GameMechanics.RemoveShape(State);

            int[] MaxHeights = new int[] { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
            for (int rows = 0; rows < 20; rows++)
            {
                for (int columns = 0; columns < 10; columns++)
                {
                    if (State.GameMap[rows][columns] != 0 && MaxHeights[columns] == 20)
                    {
                        MaxHeights[columns] = rows;
                    }
                }
            }
            GameMechanics.DrawShape(State);

            return (20 - MaxHeights.Min());

        }
        public static Moves CalculateHighestRatedMove(List<Moves> ListOfMoves) 
        {
            double HighestRating = -1000000000;

            int BestMoveID = 0;

            foreach (Moves Move in ListOfMoves)
            {
                if (Move.Rating > HighestRating)
                {
                    HighestRating = Move.Rating;
                    BestMoveID = ListOfMoves.IndexOf(Move);

                }
            }

            return ListOfMoves[BestMoveID];

        }

        public static Moves CalculateHighestRatedMoveNew(List<Moves> ListOfMoves)
        {
            double HighestRating = -1000000000;

            int BestMoveID = 0;

            foreach (Moves Move in ListOfMoves)
            {
                if (Move.Rating > HighestRating)
                {
                    HighestRating = Move.Rating;
                    BestMoveID = ListOfMoves.IndexOf(Move);

                }
            }
            return ListOfMoves[BestMoveID];
        }

    }
}