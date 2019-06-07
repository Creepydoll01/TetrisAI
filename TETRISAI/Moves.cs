namespace TETRISAI
{

    //Класс, представителями которого являются игровые ходы
    public class Moves
    {
        //Количество поворотов
        public int NumberOfRotations;
        //Направление движения
        public int Shift;
        //Рейтинг кода
        public double Rating = 0;

        //Переменные, необходимые для работы алгоритма
        public double NumberOfRowsCleared;
        public double WeightedHeight;
        public double CumulativeHeight;
        public double RelativeHeight;
        public double NumberOfMissingBlocks;
        public double Roughness;


    }



}