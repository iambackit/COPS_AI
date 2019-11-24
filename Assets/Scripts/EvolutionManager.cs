using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Selection;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Statistic;
using Assets.Scripts.Data;

class EvolutionManager : MonoBehaviour
{
    #region properties
    [Header("Car prefabs")]
    public GameObject Police;
    public GameObject Target;

    [Header("Genetic algorithm")]
    [Range(1, 50)] public int Population = 1;
    [Range(3, 50)] public int TimeToLearn = 10;

    [Header("Car init parent")]
    public GameObject InitPoint;

    [Header("Statistics")]
    public Text Current;
    public Text Best;
    public Text Basic;

    private Quaternion _Rotation = Quaternion.Euler(0, 0, -90);
    private float _Timer = 0.0f;
    private ISelectable _Selection;
    private IStatisticCalculable _StatisticCalculator;
    #endregion

    private void Awake()
    {
        _Selection = gameObject.AddComponent<ChanceByScoreSelector>();
        _Selection.Prefab = this.Police;
        _Selection.InitPosition = InitPoint.transform.position;
        _Selection.Rotation = this._Rotation;
        _Selection.Population = this.Population;
        _Selection.Target = this.Target;
        _Selection.CreateFirstGeneration();
        _Selection.PopulationReduced += OnPopulationReduced;

        _StatisticCalculator = gameObject.AddComponent<StatisticCalculator>();
        _StatisticCalculator.SetStatisticTexts(Current, Best, Basic);
        _StatisticCalculator.SetNotChanginStats(this.Population, DNA.MutationRate);
    }

    //public void NextGenButton()
    //{
    //    _Timer = 0;
    //    _Selection.CreateNewGeneration();
    //}

    private void Update()
    {
        CountDown();
        _StatisticCalculator.SetChaningStats(_Selection.ActualGeneration, _Selection.ActualPopulation, _Selection.BestFitness);
    }

    #region timer
    private void CountDown()
    {
        _Timer += Time.deltaTime;
        if (_Timer >= TimeToLearn)
        {
            CreateNewGeneration();
        }
    }

    private void CreateNewGeneration()
    {
        _Selection.CreateNewGeneration();
        _Timer = 0;
    }
    #endregion

    private void OnPopulationReduced(object source, PopulationEventArgs e)
    {
        if (e.ActualPopulation == 0)
        {
            CreateNewGeneration();
        }
    }


}
