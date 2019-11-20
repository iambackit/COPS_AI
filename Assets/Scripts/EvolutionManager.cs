using System;
using UnityEngine;

using Assets.Scripts.Selection;
using Assets.Scripts.Interfaces;

class EvolutionManager : MonoBehaviour
{
    #region properties
    [Header("Car prefabs")]
    public GameObject Police;
    public GameObject Target;

    [Header("Genetic algorithm")]
    public SelectionName SelectionName;
    [Range(1,50)]public int Population = 1;
    public Vector2 AICarPosition = new Vector2(0, 0);

    private Quaternion _Rotation = Quaternion.Euler(0, 0, 0);
    private ISelectable _Selection;
    #endregion

    private void Start()
    {
        _Selection = SelectionFactory.Selection(SelectionName);
        _Selection.Prefab = this.Police;
        _Selection.Position = this.AICarPosition;
        _Selection.Rotation = this._Rotation;
        _Selection.Population = this.Population;
        _Selection.Target = this.Target;
        _Selection.CreateFirstGeneration();
        _Selection.PopulationReduced += OnPopulationReduced;
    }

    private void OnPopulationReduced(object source, PopulationEventArgs e)
    {
        Debug.Log(e.ActualPopulation);
        if (e.ActualPopulation==0)
            _Selection.CreateNewGeneration();
    }
}
