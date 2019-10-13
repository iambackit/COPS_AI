﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WeightsOfNeuron = System.Collections.Generic.List<float>;
using WeightOfSingleLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
using WeightsOfAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<float>>>;

using OutputOfNeuronsInSingleLayer = System.Collections.Generic.List<float>;
using OutputOfNeuronsInAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
using UnityEngine;

namespace Assets.Scripts.Data
{
    class NeuralNetwork
    {
        #region public
        public OutputOfNeuronsInAllLayer OutputOfNeuronsInAllLayer { get; set; }
        public WeightsOfAllLayer WeightsOfAllLayer { get; set; }
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
                            weightsOfNeuron.Add(UnityEngine.Random.Range(-5f, 5f));
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

        public List<float> FeedForward(List<float> distances)
        {
            for (int i = 0; i < distances.Count; i++)
                OutputOfNeuronsInAllLayer[0][i] = distances[i];


            for (int indexOfLayer = 0; indexOfLayer < (OutputOfNeuronsInAllLayer.Count - 1); indexOfLayer++)
            {
                WeightOfSingleLayer weightOfActulLayer = WeightsOfAllLayer[indexOfLayer];

                WeightsOfNeuron actualLayerWeightsOfNeuron = OutputOfNeuronsInAllLayer[indexOfLayer];
                WeightsOfNeuron nextLayerWeightsOfNeuron = OutputOfNeuronsInAllLayer[indexOfLayer + 1];

                for (int i = 0;i<nextLayerWeightsOfNeuron.Count;i++)
                {
                    float sum = 0;
                    for (int j = 0;j<actualLayerWeightsOfNeuron.Count;j++)
                    {
                        sum += weightOfActulLayer[j][i] * actualLayerWeightsOfNeuron[j];
                    }

                    nextLayerWeightsOfNeuron[i] = Sigmoid(sum);
                }
            }

            List<float> output = OutputOfNeuronsInAllLayer[OutputOfNeuronsInAllLayer.Count - 1];
            return output;
        }

        public NeuralNetwork(DNA dna)
        {
            WeightsOfAllLayer = dna.WeightsOfAllLayer;
            OutputOfNeuronsInAllLayer = new OutputOfNeuronsInAllLayer();

            for (int actualLayerIndex = 0; actualLayerIndex < _NumberOfLayers; actualLayerIndex++)
            {
                OutputOfNeuronsInSingleLayer outputOfNeuronsInSingleLayer = new OutputOfNeuronsInSingleLayer();
                int actualLayerSize = GetActualLayerSize(actualLayerIndex);

                for (int i = 0; i < actualLayerSize; i++)
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

        private float Sigmoid(float input)
        {
            return 1 / (float)(1 + Mathf.Pow(2.71828f, -input));
        }
    }
}