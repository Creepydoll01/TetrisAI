using System;

namespace TETRISAI
{ 
    public class Figures // Здесь мы создаем наши фигуры
    {
        Random Randomizer = new Random();
        int[][] _figureShape = new int[4][];
        public int[][] FigureShape
        {
            get
            {
                return _figureShape;
            }
            set
            {
                _figureShape = value;
            }
        }
        //Координаты фигур
        public int X;
        public int Y;

        //Здесь хранятся наши фигуры. В нашем случае есть перечень шести стандартных фигур, а также одна необычная - для демонстрации эффективности алгоритма
        int[][] _ShapeType_t = new int[4][] { new int[] { 0, 1, 0, 0 }, new int[] { 1, 1, 1, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_o = new int[4][] { new int[] { 1, 1, 0, 0 }, new int[] { 1, 1, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_l = new int[4][] { new int[] { 1, 1, 1, 1 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_r = new int[4][] { new int[] { 1, 0, 0, 0 }, new int[] { 1, 1, 1, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_j = new int[4][] { new int[] { 1, 1, 1, 0 }, new int[] { 1, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_z = new int[4][] { new int[] { 1, 1, 0, 0 }, new int[] { 0, 1, 1, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_s = new int[4][] { new int[] { 0, 1, 1, 0 }, new int[] { 1, 1, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        int[][] _ShapeType_w = new int[4][] { new int[] { 1, 0, 1, 0 }, new int[] { 1, 1, 1, 0 }, new int[] { 0, 1, 0, 0 }, new int[] { 0, 0, 0, 0 } };


        public Figures() //Конструктор класса создает случайную фигуру на основе сгенерированного числа
        {
            int Seed = Randomizer.Next(8) ;
            if (Seed == 1)
            {
                _figureShape = _ShapeType_t;
            }
            else if (Seed == 2)
            {
                _figureShape = _ShapeType_o;
            }

            else if (Seed == 3)
            {
                _figureShape = _ShapeType_l;
            }
            else if(Seed ==4)

            {
                _figureShape = _ShapeType_r;
            }
            else if (Seed == 5)

            {
                _figureShape = _ShapeType_j;
            }
            else if (Seed == 6)

            {
                _figureShape = _ShapeType_s;
            }
            else if (Seed == 7)

            {
                _figureShape = _ShapeType_z;
            }
            else 

            {
                _figureShape = _ShapeType_w;
            }

            this.X = 5;
            this.Y = 0;
        }

        //Функция, отвечающая за поворот фигуры
        public void Rotate(int TimestoRotate)
        {

            for (int t = 0; t < TimestoRotate; t++)
            {
                int[][] OldFigure_1 = new int[4][];

                OldFigure_1[0] = new int[4];
                OldFigure_1[1] = new int[4];
                OldFigure_1[2] = new int[4];
                OldFigure_1[3] = new int[4];

                for (int i = 0; i <= 3; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        OldFigure_1[i][j] = this._figureShape[i][j];
                    }
                }

                for (int i = 0; i <= 3; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        this.FigureShape[i][j] = OldFigure_1[j][i];
                    }
                }

                int[] OldFigure_2 = new int[4];

                for (int i = 0; i <= 3; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        OldFigure_2[j] = FigureShape[i][j];

                    }

                    Array.Reverse(OldFigure_2);

                    for (int j = 0; j <= 3; j++)
                    {
                        FigureShape[i][j] = OldFigure_2[j];

                    }

                }
            }

        }

        //Функция, позволяющая клонировать фигуру, чтобы избежать ссылки на объект 
        public void CloneFigure(Figures FiguretoClone)
        {
            this.X = FiguretoClone.X;
            this.Y = FiguretoClone.Y;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.FigureShape[i][j] = FiguretoClone.FigureShape[i][j];
                }
            }
        }


    }

}