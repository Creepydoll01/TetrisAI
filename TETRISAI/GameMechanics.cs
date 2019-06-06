using System;
using System.Collections.Generic;

namespace Попыткасделатьтетрис2
{
    public class GameMechanics
    {


        static public GameState RemoveShape(GameState State) //Tries to make a desired move given Gamefield called GameMap and a Tetris Figure 
        {

            //Эта часть когда должна быть в методе обновления а не действия, надо перенексти
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
            // Эта часть кода закончена

            return State;
        }

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

        static public void DrawShape(GameState State) //Tries to make a desired move given Gamefield called GameMap and a Tetris Figure 
        {

            //Эта часть когда должна быть в методе обновления а не действия, надо перенексти
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
            // Эта часть кода закончена


        }
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



        static public GameState MoveDown(GameState State)
        {
            State.FigureMoved = true;
            State.Defeat = false;
            State.NumberOfRowsCleared = 0;
            RemoveShape(State);
            State.CurrentFigure.Y++;


            if (Collision(State))
            {

                State.CurrentFigure.Y--;
                DrawShape(State);
                State.GenerateNextFigure();
                //State.CurrentFigure.CloneFigure(State.NextFigure);
                //State.NextFigure = new Figures();
                ClearRows(State);



                //Console.WriteLine("Поражение достигнуто ли:" + State.Defeat.ToString());

                if (Collision(State))
                {

                    State.Defeat = true;

                    if (State.AiMode)
                    {
                        //Ничего не делаем НОВОВВЕДЕНИЕ
                        State.ResetGameState();
                        State.Defeat = true;
                        //State.CurrentGenome++;
                    }

                    else
                    {
                        State.ResetGameState();
                    }
                }


                State.FigureMoved = false;
            }

            DrawShape(State);

            State.GameScore++;

            return State;

        }



        public static void GameEngineNew(GameState State)
        {
            if (State.AiMode)
            {

                if (!State.FigureMoved)
                {
                    //Если мы проиграли либо вышли за лимит шагов на геном надо обновить геном
                    if (State.Defeat || State.MovesTaken > State.MovesLimit)
                    {
                        State.ListOfGenomes[State.CurrentGenome].GenomeRating = State.GameScore;
                        //Мы сейчас начнем играть заново новым геномом, поэтому тут необходимо занулить рейтинг
                        State.GameScore = 0;
                        AI.EvaluateNextGenomeNew(State);

                    }
                    //Если же мы не проиграли, то можно делать следующий шаг
                    else
                    {
                        AI.MakeNextMoveNew(State);
                    }
                }
            }
        }
        public static void GameEngine(GameState State)
        {


            if (State.AiMode && State.CurrentGenome != -1)
            {
                MoveDown(State);
                //Если фигура не подвинулась (встала на свое место)
                if (!State.FigureMoved)
                {
                    //Console.WriteLine("встала");

                    {

                        if (State.Defeat)
                        {

                            State.ListOfGenomes[State.CurrentGenome].GenomeRating = State.GameScore;
                            AI.EvaluateNextGenome(State);
                        }
                        //Если же мы не проиграли - надо делать следующий ход
                        else
                        {
                            AI.MakeNextMove(State);
                        }
                    }
                }
            }
            // Если же мы играем сами, то нужно просто опустить фигуру вниз
            else
            {
                MoveDown(State);
            }
        }

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

            // Здесь должен создаваться отдельный массив типа тех рядов, что мы убрали, но я не знаю, нафиг это нужно, поэтому пока не делаю



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

