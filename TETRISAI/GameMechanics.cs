using System;
using System.Collections.Generic;

namespace TETRISAI
{
    //Класс функций, отвечающий непосредственно за игровую механику - движение фигур, коллизию, подсчет очков и так далее. 
    public class GameMechanics
    {


        static public GameState RemoveShape(GameState State) // Данный метод убирает фигуру с игрового поля. Это может быть полезно например в том случае, если нам надо подсчитать характеристики поля
        {
  
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (State.CurrentFigure.FigureShape[i][j] != 0)
                    {
                        State.GameMap[State.CurrentFigure.Y + i][State.CurrentFigure.X + j] = 0;
                    }
                }
            }
            return State;
        }

        //Данный метод поворачивает фигуру. Если поворот вызывает коллизию, то происходит отмена изменений
        public static void RotateShape(GameState State)
        {

            RemoveShape(State);
            State.CurrentFigure.Rotate(1);

            if (Collision(State))
            {
                State.CurrentFigure.Rotate(3);
            }

            DrawShape(State);

        }

        static public void DrawShape(GameState State) // Этот метод как-бы впечатывает фигуру на игровое поле
        {

            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (State.CurrentFigure.FigureShape[i][j] != 0)
                    {
                        State.GameMap[State.CurrentFigure.Y + i][State.CurrentFigure.X + j] = State.CurrentFigure.FigureShape[i][j];
                    }
                }
            }

        }

        //Метод, отвечающий за проверку коллизий
        static public bool Collision(GameState State)
        {
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (State.CurrentFigure.FigureShape[i][j] != 0)
                    {


                        if (State.CurrentFigure.Y + i > 19 || State.CurrentFigure.Y + i < 0 || State.CurrentFigure.X + j > 9 || State.CurrentFigure.X + j < 0 || State.GameMap[State.CurrentFigure.Y + i][State.CurrentFigure.X + j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //Метод, сдвигающий фигуру влево
        static public GameState MoveLeft(GameState State)

        {
            RemoveShape(State);
            State.CurrentFigure.X--;
            if (Collision(State))
            {
                State.CurrentFigure.X++;
            }
            DrawShape(State);

            return State;
        }
        //Метод, сдвигающий фигуру вправо
        static public GameState MoveRight(GameState State)

        {
            RemoveShape(State);
            State.CurrentFigure.X++;
            if (Collision(State))
            {
                State.CurrentFigure.X--;
            }
            DrawShape(State);

            return State;
        }


        //Метод, сдвигающий фигуру вниз. Одновременно происходит проверка на проигрыш
        static public MovementResults MoveDown(GameState State)
        {
            MovementResults MoveResults= new MovementResults();
            RemoveShape(State);
            State.CurrentFigure.Y++;


            if (Collision(State))
            {

                State.CurrentFigure.Y--;
                DrawShape(State);
                State.GenerateNextFigure();
                ClearRows(State);

                if (Collision(State))
                {

                    MoveResults.Defeat = true;


                    if (State.AiMode)
                    {
                        
                        State.ResetGameState();
                        MoveResults.Defeat = true;
                        
                    }

                    else
                    {
                        State.ResetGameState();
                    }
                }


                MoveResults.MoveMade = false;
            }

            DrawShape(State);

            State.GameScore++;
            return MoveResults;

        }


        //Метод, инициализирующий игровую сессию
        public static void GameEngine(GameState State)
        {
            if (State.AiMode)
            {
                MovementResults MoveResult = new MovementResults();
                MoveResult = MoveDown(State);
                
                
                if (!MoveResult.MoveMade)
                {
                    //Если мы проиграли либо вышли за лимит шагов на геном надо обновить геном
                    if (MoveResult.Defeat || State.MovesTaken > State.MovesLimit)
                    {
                        State.ListOfGenomes[State.CurrentGenome].GenomeRating = State.GameScore;

                        
                        //Мы сейчас начнем играть заново новым геномом, поэтому тут необходимо занулить рейтинг
                        State.GameScore = 0;
                        AI.EvaluateNextGenome(State);

                    }
                    //Если же мы не проиграли, то можно делать следующий шаг
                    else
                    {
                        AI.MakeNextMove(State);
                    }
                }
            }
        }
      
        //Метод, отчищающий заполненные ряды
        static void ClearRows(GameState State)
        {
            List<int> RowsToClear = new List<int>();

            for (int row = 0; row < 20; row++)
            {
                bool HasEmptySpace = false;

                for (int column = 0; column < 10; column++)
                {
                    if (State.GameMap[row][column] == 0)
                    {
                        HasEmptySpace = true;
                    }
                }

                //Если нет пустых клеток

                if (!HasEmptySpace)
                {
                    RowsToClear.Add(row);
                }


            }
            // Добавляем очки в зависимости от количества заполненных строк
            if (RowsToClear.Count == 1)
            {
                State.GameScore += 200;
            }

            if (RowsToClear.Count == 2)
            {
                State.GameScore += 800;
            }

            if (RowsToClear.Count == 3)
            {
                State.GameScore += 2400;
            }

            if (RowsToClear.Count == 4)
            {
                State.GameScore += 3200;
            }

            foreach (int item in RowsToClear)
            {
                for (int j = 0; j < 10; j++)
                {
                    State.GameMap[item][j] = 0;

                    for (int k = item; k >= 0; k--)
                    {
                        if (State.GameMap[k][j] == 1)
                        {
                            State.GameMap[k][j] = 0;
                            State.GameMap[k + 1][j] = 1;
                        }
                    }
                }
            }

            State.NumberOfRowsCleared = RowsToClear.Count;


        }


    }
}

