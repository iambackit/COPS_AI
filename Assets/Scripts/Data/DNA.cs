using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WeightsOfNeuron = System.Collections.Generic.List<float>;
using WeightOfSingleLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
using WeightsOfAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<float>>>;

namespace Assets.Scripts.Data
{
    public class DNA
    {
        #region public
        public WeightsOfAllLayer WeightsOfAllLayer { get; private set; }
        #endregion

        #region private
        private float _Probability = 0.025f;
        #endregion

        public DNA(WeightsOfAllLayer weights)
        {
            this.WeightsOfAllLayer = weights;
        }

        public DNA CrossOver(DNA parent0, DNA parent1)
        {
            WeightsOfAllLayer crossOveredWeights = new WeightsOfAllLayer();

            for (int i = 0;i< WeightsOfAllLayer.Count; i++)
            {
                WeightOfSingleLayer parentWeights0 = parent0.WeightsOfAllLayer[i];
                WeightOfSingleLayer parentWeights1 = parent1.WeightsOfAllLayer[i];

                WeightOfSingleLayer newWeight = new WeightOfSingleLayer();

                for (int j = 0;j<parentWeights0.Count;j++)
                {
                    WeightsOfNeuron actualWeights = new WeightsOfNeuron();
                    newWeight.Add(actualWeights);
                    for (int k = 0;k<parentWeights0[j].Count;k++)
                    {
                        float value = Random.Range(0f, 1f) < 0.5f ? parentWeights0[j][k] : parentWeights1[j][k];
                        newWeight[j].Add(value);
                    }
                }
                crossOveredWeights.Add(newWeight);
            }

            return new DNA(crossOveredWeights);
        }

        public void Mutate()
        {
            WeightsOfAllLayer crossOveredWeights = new WeightsOfAllLayer();

            for (int i = 0; i < WeightsOfAllLayer.Count; i++)
            {
                WeightOfSingleLayer newWeight = new WeightOfSingleLayer();

                for (int j = 0;j<WeightsOfAllLayer[i].Count;j++)
                {
                    WeightsOfNeuron actualWeights = new WeightsOfNeuron();
                    newWeight.Add(actualWeights);
                    
                    for (int k = 0;k<WeightsOfAllLayer[i][j].Count;k++)
                    {
                        float value = Random.Range(0f, 1f) < _Probability ? Random.Range(-5f, 5f) : WeightsOfAllLayer[i][j][k];
                        newWeight[j].Add(value);
                    }
                }

                crossOveredWeights.Add(newWeight);
            }

            this.WeightsOfAllLayer = crossOveredWeights;
        }
    }

    
}