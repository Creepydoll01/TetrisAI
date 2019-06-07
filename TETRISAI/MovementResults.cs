using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TETRISAI
{
    //Результаты движения вниз. Необходимы для упрощения работы алгоритма и детекта ситуации, когда игра проиграна. 
    public class MovementResults
    {

        public bool Defeat = false;

        public bool MoveMade = true;

        public int RowsCleared = 0;
    }
}
