using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    class StringContainer
    {
        public static readonly string Player = "Player";
        public static readonly string TagScore = "Score";
        public static readonly string TagMap = "Map";

        #region statistics
        public static readonly string Generation = "GENERATION: ";
        public static readonly string CarsAlive = "CARS ALIVE: ";
        public static readonly string Fitness = "FITNESS: ";

        public static readonly string Population = "POPULATION: ";
        public static readonly string MutationRate = "MUTATION RATE: ";
        #endregion
    }
}
