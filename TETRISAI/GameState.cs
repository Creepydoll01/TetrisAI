using System.Collections.Generic;
namespace Попыткасделатьтетрис2
{
    public class GameState
    {
        public bool AiMode = false;
        public int SizeOfPopulation = 5;




        public int CurrentGenome = -1;

        public int Generation = 1;

        public List<Genomes> BestCandidates = new List<Genomes>();
        public double MutationRate = 0.05;
        public double MutationStep = 0.2;
        public int[][] GameMap = new int[][]
        {
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0},
            new int[] {0,0,0,0,0,0,0,0,0,0}


        };
        public bool Defeat = false;

        public bool FigureMoved = true;

        public int GameScore = 0;
        public int NumberOfRowsCleared = 0;

        public int MovesTaken = 0;
        public List<Genomes> ListOfGenomes = new List<Genomes>();
        public int MovesLimit = 500;

        int AmountOfMoves = 0;

        public Figures CurrentFigure = new Figures();
        public Figures NextFigure = new Figures();



        public Genomes AlgorithmUsed;
        public GameState()
        {
            //Убираю, потому что в явновм виде создал гейммап как поле
            // for (int i = 0; i <= 19; i++)
            //{
            //  for (int j = 0; j <= 9; j++)
            //    GameMap[i][j] = 0;
            //}
            CurrentFigure = new Figures();
            NextFigure = new Figures();

        }

        public void GenerateNextFigure()
        {
            this.CurrentFigure.CloneFigure(this.NextFigure);
            this.NextFigure = new Figures();
        }

        public void SaveGameState(GameState StateToSave)
        {
            //Добавить загрузку бэга
            this.GameScore = StateToSave.GameScore;
            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 9; j++)
                    this.GameMap[i][j] = StateToSave.GameMap[i][j];
            }

            this.CurrentFigure.CloneFigure(StateToSave.CurrentFigure);
            this.NextFigure.CloneFigure(StateToSave.NextFigure);
            this.FigureMoved = StateToSave.FigureMoved;
            this.Defeat = StateToSave.Defeat;
            this.GameScore = StateToSave.GameScore;
            this.NumberOfRowsCleared = StateToSave.NumberOfRowsCleared;
            this.AmountOfMoves = StateToSave.AmountOfMoves;
            this.CurrentGenome = StateToSave.CurrentGenome;


        }

        public void LoadGameState(GameState StateToLoad)
        {

            //Добавить загрузку бэга
            this.GameScore = StateToLoad.GameScore;
            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 9; j++)
                    this.GameMap[i][j] = StateToLoad.GameMap[i][j];
            }

            this.CurrentFigure.CloneFigure(StateToLoad.CurrentFigure);
            this.NextFigure.CloneFigure(StateToLoad.NextFigure);
            this.FigureMoved = StateToLoad.FigureMoved;
            this.Defeat = StateToLoad.Defeat;
            this.GameScore = StateToLoad.GameScore;
            this.NumberOfRowsCleared = StateToLoad.NumberOfRowsCleared;
            this.AmountOfMoves = StateToLoad.AmountOfMoves;
            this.CurrentGenome = StateToLoad.CurrentGenome;



        }
        public void ResetGameState()
        {
            this.GameScore = 0;
            this.AmountOfMoves = 0;
            this.Defeat = false;
            this.FigureMoved = true;
            this.NumberOfRowsCleared = 0;

            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 9; j++)
                    this.GameMap[i][j] = 0;
            }

            this.CurrentFigure = new Figures();
            this.NextFigure = new Figures();
        }
    }
}
