using System.Collections.Generic;
namespace TETRISAI
{
    public class GameState
    {

        /// <summary>
        ///Тут несколько переменных, необходимых для работы игрового режима.Включить/отключить режим искусственного интеллекта, установить размер популяции геномов, инкремент нынешнего генома и поколения, скорость мутаций и сила мутаций, лимит шагов на 
        ///геном
        /// </summary>
        public bool AiMode = true;
        public int SizeOfPopulation = 5;
        public int NumberOfRowsCleared = 0;
        public int CurrentGenome = 0;
        public int Generation = 1;
        public double MutationRate = 0.05;
        public double MutationStep = 0.2;
        public int GameScore = 0;
        public int MovesTaken = 0;
        public List<Genomes> ListOfGenomes = new List<Genomes>();
        public int MovesLimit = 500;
        int AmountOfMoves = 0;

        //Наше игровое поле. Используется массив массивов, потому что так быстрее 
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
        
       

        public Figures CurrentFigure = new Figures();
        public Figures NextFigure = new Figures();
        //Конструктор класса GameState. Создаем нынешнюю и следующие фигуры. Все остальное - по умолчанию.
        public GameState()
        {
            CurrentFigure = new Figures();
            NextFigure = new Figures();
        }

        //Переход к следующей фигуре
        public void GenerateNextFigure()
        {
            this.CurrentFigure.CloneFigure(this.NextFigure);
            this.NextFigure = new Figures();
        }

        //Сохранить нынешнее игровое состояние  
        
        public void SaveGameState(GameState StateToSave)
        {
            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 9; j++)
                    this.GameMap[i][j] = StateToSave.GameMap[i][j];
            }
            this.CurrentFigure.CloneFigure(StateToSave.CurrentFigure);
            this.NextFigure.CloneFigure(StateToSave.NextFigure);
            this.GameScore = StateToSave.GameScore;

        }
        //Загрузить нынешнее игровое состояние 
        public void LoadGameState(GameState StateToLoad)
        {
            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 9; j++)
                    this.GameMap[i][j] = StateToLoad.GameMap[i][j];
            }
            this.CurrentFigure.CloneFigure(StateToLoad.CurrentFigure);
            this.NextFigure.CloneFigure(StateToLoad.NextFigure);
            this.GameScore = StateToLoad.GameScore;

        }

        //Установить игровое состояние по-умолчанию
        public void ResetGameState()
        {
           
            this.AmountOfMoves = 0;
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
