using System;
namespace Попыткасделатьтетрис2
{
    public class Genomes
    {
        Random Randomizer = new Random();
        //Уникальный айди
        public double ID;
        //Важность отчистки рядов, которая происходит благодаря очередному игровому действию. Чем больше было отчищено - тем больше это значение
        public double NumberOfRowsCleared;

        // Высота рядов, чтобы алгоритм понимал, что мы строим слишком высоко
        public double WeightedHeight;
        //Сумма всех высот 
        public double CumulativeHeight;
        // Самая большая высота - самая маленькая
        public double RelativeHeight;
        // Количество всех пустых клеток, которые уже никак нельзя заполнить
        public double NumberOfMissingBlocks;
        //сумма всех разниц между высотами каждой колонки ??????????????
        public double Roughness; // Надо бы придумать название получше

        public int GenomeRating;




        //Создаем конструктор этого класса, который будет создавать нам случайные геномы

        public Genomes(double MutationRate)
        {
            this.ID = Randomizer.NextDouble();
            this.NumberOfRowsCleared = Randomizer.NextDouble() - MutationRate;
            this.WeightedHeight = Randomizer.NextDouble() - MutationRate;
            this.CumulativeHeight = Randomizer.NextDouble() - MutationRate;
            this.RelativeHeight = Randomizer.NextDouble() - MutationRate;
            this.NumberOfMissingBlocks = Randomizer.NextDouble() * MutationRate;
            this.Roughness = Randomizer.NextDouble() - MutationRate;


        }

    }
}