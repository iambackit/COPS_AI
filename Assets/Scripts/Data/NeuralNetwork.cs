using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WeightsOfNeuron = System.Collections.Generic.List<float>;
using WeightOfSingleLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
using WeightsOfAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<float>>>;

using OutputOfNeuronsInSingleLayer = System.Collections.Generic.List<float>;
using OutputOfNeuronsInAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;

namespace Assets.Scripts.Data
{
    class NeuralNetwork
    {
        #region public
        public OutputOfNeuronsInAllLayer OutputOfNeuronsInAllLayer { get; }
        public WeightsOfAllLayer WeightsOfAllLayer { get; }
        #endregion

        #region private
        private int _NumberOfLayers = 3; //input layer, 1 hidden layer, output layer
        private int _NumberOfInputs = 5;
        private int _NumberOfOutputs = 2;
        private int _NumberOfNeuronsInHiddenLayer = 10;
        #endregion


        public NeuralNetwork()
        {
            WeightsOfAllLayer = new WeightsOfAllLayer();
            OutputOfNeuronsInAllLayer = new OutputOfNeuronsInAllLayer();

            for (int actualLayerIndex = 0; actualLayerIndex < _NumberOfLayers; actualLayerIndex++)
            {
                WeightOfSingleLayer weightsOfSingleLayer = new WeightOfSingleLayer();
                OutputOfNeuronsInSingleLayer outputOfNeuronsInSingleLayer = new OutputOfNeuronsInSingleLayer();

                int actualLayerSize = GetActualLayerSize(actualLayerIndex);

                if ( actualLayerIndex != (_NumberOfLayers-1) ) //if we are not at the last layer
                {
                    int nextLayerSize = GetActualLayerSize(actualLayerIndex + 1);

                    for (int i = 0; i<actualLayerSize; i++)
                    {
                        WeightsOfNeuron weightsOfNeuron = new WeightsOfNeuron();

                        for (int k = 0;k<nextLayerSize;k++)
                        {
                            weightsOfNeuron.Add(UnityEngine.Random.Range(-5, 5));
                        }
                        weightsOfSingleLayer.Add(weightsOfNeuron);
                    }
                    WeightsOfAllLayer.Add(weightsOfSingleLayer);
                }

                for (int i = 0;i< actualLayerSize;i++)
                {
                    outputOfNeuronsInSingleLayer.Add(0);
                }

                OutputOfNeuronsInAllLayer.Add(outputOfNeuronsInSingleLayer);
            }
        }

        private int GetActualLayerSize(int layerIndex)
        {
            if (layerIndex == 0)
                return _NumberOfInputs;
            else if (layerIndex == (_NumberOfLayers - 1))
                return _NumberOfOutputs;
            else
                return _NumberOfNeuronsInHiddenLayer;
        }
    }
}
