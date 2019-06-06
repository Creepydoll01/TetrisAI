using System.Collections.Generic;
using System.Linq;
using System;
namespace Попыткасделатьтетрис2
{



    public class AI

    {
        static GameState SavedState = new GameState();
        static Random Randomizer = new Random();


        public static void CreateFirstPopulation(GameState State)
        {
            for (int i = 0; i < State.SizeOfPopulation; i++)
            {
                Genomes Genome = new Genomes(State.MutationRate);
                State.ListOfGenomes.Add(Genome);
            }

            EvaluateNextGenome(State);
        }

        public static void CreateFirstPopulationNew(GameState State)
        {
            //Мы заполняем нашу популяцию первоначальными геномами, генерируя их в конструкторе
            for (int i = 0; i < State.SizeOfPopulation; i++)
            {
                Genomes Genome = new Genomes(State.MutationRate);
                State.ListOfGenomes.Add(Genome);
            }
            //Оцениваем первый геном
            EvaluateNextGenomeNew(State);
        }
        public static void EvaluateNextGenome(GameState State) // Придумать название получше
        {

            State.CurrentGenome++;
            //Если вдруг геномов не осталось, то эволюционируем
            if (State.CurrentGenome == State.ListOfGenomes.Count)
            {

                Evolve(State);
            }


            //здесь у него реализована загрузка нынешнего игрового состояния, но мне это вроде не нужно пока
            State.MovesTaken = 0;
            MakeNextMove(State);

        }

        public static void EvaluateNextGenomeNew(GameState State)
        {
            //Переходим к след геному
            State.CurrentGenome++;
            //Проверяем, остались ли геномы в популяции. Если не остались - нужно эволюционировать
            if (State.CurrentGenome == State.ListOfGenomes.Count)
            {
                EvolveNew(State);
            }
            //Тут у него реализована загрузка состояния - не знаю, зачем
            //Поскольку мы перешли к новому геному, необходимо обнулить число сделанных шагов и сделать следующий шаг
            State.MovesTaken = 0;
            MakeNextMoveNew(State);
        }

        static void Evolve(GameState State)
        {
            State.CurrentGenome = 0;
            State.Generation++;
            //Возвращаемся к первоначальному состоянию
            State.ResetGameState();

            foreach (Genomes item in State.ListOfGenomes)
            {
                Console.WriteLine(item.GenomeRating);
            }
            // Console.ReadKey();
            //Отсортируем наш список геномов по убыванию рейтинга
            State.ListOfGenomes.OrderByDescending(x => x.GenomeRating);

            foreach (Genomes item in State.ListOfGenomes)
            {
                Console.WriteLine(item.GenomeRating);
            }
            // Console.ReadLine();


            //Добавим самый лучший геном в список лучших кандидатов на скрещивание


            State.BestCandidates.Add(State.ListOfGenomes[0]);

            //Console.WriteLine("Best candidate is" + State.ListOfGenomes[0].GenomeRating);
            // Console.ReadKey();
            //Уберем половину самых плохих геномов из списка

            while (State.ListOfGenomes.Count > (State.SizeOfPopulation / 2))
            {
                State.ListOfGenomes.RemoveAt(State.ListOfGenomes.Count - 1);
            }

            //Посчитаем сумму рейтинга всех оставшихся геномов
            int TotalRating = 0;
            foreach (Genomes item in State.ListOfGenomes)
            {
                TotalRating += item.GenomeRating;
            }
            //Создаем список геномов новго поколения и добавим туда самый хороший геном из этого
            List<Genomes> Children = new List<Genomes>();
            Children.Add(State.ListOfGenomes[0]);

            //Заполним список оставшимися геномами, создав их скрещиванием
            while (Children.Count < State.SizeOfPopulation)
            {
                Genomes Parent_1 = PickRandomGenome(State);
                Genomes Parent_2 = PickRandomGenome(State);
                Children.Add(MakeChild(Parent_1, Parent_2, State.MutationRate, State.MutationStep));
            }

            //Обьявляем новое поколение детей нашими геномами
            State.ListOfGenomes = new List<Genomes>();
            foreach (Genomes item in Children)
            {
                State.ListOfGenomes.Add(item);
            }




        }

        private static Genomes MakeChild(Genomes Parent_1, Genomes Parent_2, double MutationRate, double MutationStep)
        {


            Genomes Child = new Genomes(MutationRate);

            //Выбираем для нашего ребенка случайные параметры от каждого родителя
            Child.NumberOfRowsCleared = GetRandomParent(Parent_1, Parent_2).NumberOfRowsCleared;
            Child.WeightedHeight = GetRandomParent(Parent_1, Parent_2).WeightedHeight;
            Child.CumulativeHeight = GetRandomParent(Parent_1, Parent_2).CumulativeHeight;
            Child.RelativeHeight = GetRandomParent(Parent_1, Parent_2).RelativeHeight;
            Child.NumberOfMissingBlocks = GetRandomParent(Parent_1, Parent_2).NumberOfMissingBlocks;
            Child.Roughness = GetRandomParent(Parent_1, Parent_2).Roughness;
            Child.GenomeRating = -1;

            // Теперь начинаем мутировать нашего ребенка :)

            if (Randomizer.NextDouble() < MutationRate)
            {
                Child.WeightedHeight += Randomizer.NextDouble() * MutationStep * 2 - MutationStep;
            }

            if (Randomizer.NextDouble() < MutationRate)
            {
                Child.CumulativeHeight += Randomizer.NextDouble() * MutationStep * 2 - MutationStep;
            }

            if (Randomizer.NextDouble() < MutationRate)
            {
                Child.RelativeHeight += Randomizer.NextDouble() * MutationStep * 2 - MutationStep;
            }

            if (Randomizer.NextDouble() < MutationRate)
            {
                Child.NumberOfMissingBlocks += Randomizer.NextDouble() * MutationStep * 2 - MutationStep;
            }

            if (Randomizer.NextDouble() < MutationRate)
            {
                Child.NumberOfRowsCleared += Randomizer.NextDouble() * MutationStep * 2 - MutationStep;
            }

            if (Randomizer.NextDouble() < MutationRate)
            {
                Child.Roughness += Randomizer.NextDouble() * MutationStep * 2 - MutationStep;
            }

            return Child;

        }

        private static Genomes GetRandomParent(Genomes Parent_1, Genomes Parent_2)
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

        static Genomes PickRandomGenome(GameState State)
        {

            return State.ListOfGenomes[Randomizer.Next(State.ListOfGenomes.Count - 1)];
        }
        static public void MakeNextMoveNew(GameState State)
        {
            //Увеличим количество сделанных шагов
            State.MovesTaken++;
            //Сохраним наше состояние, чтобы можно было безбоязненно определить список возможных шагов
            SavedState.SaveGameStateNew(State);
            List<Moves> ListOfMoves = new List<Moves>();

            //Смотрим, какие шаги мы можем сделать с нынешней фигурой
            ListOfMoves = GetAllPossibleMovesNew(State);
            //Теперь перейдем к следующей фигуре, чтобы понять, какой из нынешних ходов привет к наиболее оптимальному результату при учете, что и следующим ходом будет наиболее оптимальный
            State.GenerateNextFigureNew();

            for (int i = 0; i < ListOfMoves.Count; i++)
            {
                Moves NextMove = new Moves();
                NextMove = CalculateHighestRatedMoveNew(GetAllPossibleMovesNew(State));
                ListOfMoves[i].Rating += NextMove.Rating;
            }
            //Теперь найдем самый выгодный мув из всех возможных
            Moves MoveToMake = new Moves();
            MoveToMake = CalculateHighestRatedMoveNew(ListOfMoves);

            //Самый выгодный мув найден. Теперь можно загрузить первоначальное состояние и сделать наконец этот мув

            State.LoadGameStateNew(SavedState);

            //Выполняем мув. ВРОДЕ РАБОТАЕТ КАК И ДОЛЖНО

            for (int Rotations = 0; Rotations < MoveToMake.NumberOfRotations; Rotations++)
            {
                GameMechanics.RotateShape(State);
            }
            // двигаем влево
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
        static public void MakeNextMove(GameState State)
        {

            State.MovesTaken++;
            //Смотрим, а не привысили ли мы лимит количество действий. Если превысили - надо выбирать новый геном
            if (State.MovesTaken > State.MovesLimit)
            {
                State.ListOfGenomes[State.CurrentGenome].GenomeRating = State.GameScore;

                EvaluateNextGenome(State);

            }

            else
            {
                //Нужно сделать ход

                List<Moves> ListOfMoves = GetAllPossibleMoves(State);

                //удалить
                //foreach(Moves item in ListOfMoves)
                //{
                //  Console.WriteLine(item.Shift);
                //}
                //Console.ReadKey();
                //

                GameState SavedState = new GameState();
                SavedState.SaveGameState(State);

                //Для каждого возможного шага с нынешней фигурой посчитаем все возможные шаги с будущей фигурой и выберем наиболее оптимальный
                State.GenerateNextFigure();



                for (int i = 0; i < ListOfMoves.Count; i++)
                {
                    Moves Nextmove = CalculateHighestRatedMove(GetAllPossibleMoves(State));
                    ListOfMoves[i].Rating += Nextmove.Rating;
                }

                // Найдя наиболее выгодный ход, мы можем его совершить. Для этого загрузим предыдущее состояние

                State.LoadGameState(SavedState);

                Moves MoveToMake = CalculateHighestRatedMove(ListOfMoves);

                for (int Rotations = 0; Rotations < MoveToMake.NumberOfRotations; Rotations++)
                {
                    GameMechanics.RotateShape(State);
                }
                // двигаем влево
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

        }

        public static List<Moves> GetAllPossibleMoves(GameState State)
        {
            //Прежде чем смотреть на варианты разивития событий, сохраним нынешнее состояние игры

            GameState SavedState = new GameState();
            SavedState.SaveGameState(State);

            List<Moves> PossibleMoves = new List<Moves>();
            List<double> MovesRatings = new List<double>();
            int NumberOfIterations = 0;
            // Проверяем каждый возможный поворот фигуры
            for (int Rotations = 0; Rotations < 4; Rotations++)
            {
                List<int> PreviousX = new List<int>();
                // Проверяем каждую итерацию (что это?)
                for (int t = -5; t <= 5; t++)
                {
                    NumberOfIterations++;

                    //осуществляем вращение
                    for (int i = 0; i < Rotations; i++)
                    {
                        GameMechanics.RotateShape(State);
                    }

                    //Move Left

                    if (t < 0)
                    {
                        int Abs_t = Math.Abs(t);
                        for (var left = 0; left < Abs_t; left++)
                        {
                            GameMechanics.MoveLeft(State);
                        }
                    }

                    //Move Right

                    if (t > 0)
                    {

                        for (var right = 0; right < t; right++)
                        {
                            GameMechanics.MoveRight(State);
                        }
                    }

                    //если фигура вообще двигалась 
                    if (!PreviousX.Contains(State.CurrentFigure.X))
                    {
                        //Двигаем вниз
                        GameMechanics.MoveDown(State);
                        while (State.FigureMoved)
                        {
                            GameMechanics.MoveDown(State);
                        }

                        Moves Move = new Moves(State);
                        Move.NumberOfRotations = Rotations;
                        Move.Shift = t;

                        Move.Algorithm.NumberOfRowsCleared = State.NumberOfRowsCleared;
                        Move.Algorithm.WeightedHeight = Math.Pow(CalculateHeight(State), 1.5);
                        Move.Algorithm.CumulativeHeight = CalculateCumulativeHeight(State);
                        Move.Algorithm.RelativeHeight = CalculateRelativeHeight(State);
                        Move.Algorithm.NumberOfMissingBlocks = CountMissingBlocks(State);
                        Move.Algorithm.Roughness = CalculateRoughness(State);

                        Move.Rating += Move.Algorithm.NumberOfRowsCleared * State.ListOfGenomes[State.CurrentGenome].NumberOfRowsCleared;
                        Move.Rating += Move.Algorithm.WeightedHeight * State.ListOfGenomes[State.CurrentGenome].WeightedHeight;
                        Move.Rating += Move.Algorithm.CumulativeHeight * State.ListOfGenomes[State.CurrentGenome].CumulativeHeight;
                        Move.Rating += Move.Algorithm.RelativeHeight * State.ListOfGenomes[State.CurrentGenome].RelativeHeight;
                        Move.Rating += Move.Algorithm.NumberOfMissingBlocks * State.ListOfGenomes[State.CurrentGenome].NumberOfMissingBlocks;
                        Move.Rating += Move.Algorithm.Roughness * State.ListOfGenomes[State.CurrentGenome].Roughness;

                        if (State.Defeat)
                        {
                            Move.Rating -= 500;
                        }

                        PossibleMoves.Add(Move);
                        PreviousX.Add(State.CurrentFigure.X);


                    }
                }
            }

            //Мы рассмотрели все варианты развития событий. Теперь загрузим ранее сохраненное состояние

            State.LoadGameState(SavedState);

            return PossibleMoves;

        }


        // Далее идут вспомогательные функции, использующиеся в методе GetAllPossibleMoves
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

        public static Moves CalculateHighestRatedMove(List<Moves> ListOfMoves) // Сильно переписал эту функцию, так что надо быть аккуратным
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