using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;
using System.Linq;

public class AIManager : MonoBehaviour
{
    #region public
    public GameObject CarPrefab;
    public int Population = 50;
    public PopulationGeneratorName SelectedMethod;
    #endregion

    #region private
    private PopulationManager _PopulationManager;

    private DNA _First;
    private DNA _Second;

    private GUIStyle guiStyle;

    private int _Generation = 0;
    private int _ActiveCars;

    //for some statistic data
    private int _DeathByWall = 0;
    private int _DeathByPunishment = 0;
    private int _BestScore = 0;
    private float _ActualGenerationAverageScore = 0;
    private float _TempAverageScore = 0;
    private float _BestGenerationAverageScore = 0;
    private int _IndexOfBestGenerationAverageScore = 0;
    #endregion

    void Start()
    {
        guiStyle = new GUIStyle();
        _PopulationManager = new PopulationManager(SelectedMethod, Population, CarPrefab);
        _PopulationManager.CreateNewGeneration();

        _ActiveCars = Population;
        _Generation++;
    }

    public void DestroyCar(GameObject gameObject)
    {
        gameObject.SetActive(false);
        SetCarDeath(gameObject);

        if (_ActiveCars == 1)
        {
            _TempAverageScore = 0;
            _PopulationManager.CreateNewGeneration();
            _Generation++;
            _ActiveCars = Population;
        }
        else
            _ActiveCars--;
    }

    public void SetCarDeath(GameObject gameObject)
    {
        if (gameObject.GetComponent<Car>().Punished)
            _DeathByPunishment++;
        else
            _DeathByWall++;
    }

    public void SetScore(GameObject gameObject)
    {
        //calculate average generation score
        _TempAverageScore += gameObject.GetComponent<Car>().Score;
        _ActualGenerationAverageScore = _TempAverageScore / Population;

        //set best individual score
        if (gameObject.GetComponent<Car>().Score > _BestScore)
            _BestScore = gameObject.GetComponent<Car>().Score;

        //set best average score
        if (_ActualGenerationAverageScore > _BestGenerationAverageScore)
        {
            _IndexOfBestGenerationAverageScore = _Generation;
            _BestGenerationAverageScore = _ActualGenerationAverageScore;
        }
    }


    #region GUI
    private void OnGUI()
    {
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.red;
        GUI.BeginGroup(new Rect(10, 10, 450, 300));
        GUI.Label(new Rect(0, 0, 100, 100), StringContainer.Generation + _Generation, guiStyle);
        GUI.Label(new Rect(0, 30, 100, 100), StringContainer.Population + Population, guiStyle);
        GUI.Label(new Rect(0, 60, 100, 100), StringContainer.CurrentPopulation + _ActiveCars, guiStyle);
        GUI.Label(new Rect(0, 90, 100, 100), StringContainer.DeathByWall + _DeathByWall, guiStyle);
        GUI.Label(new Rect(0, 120, 100, 100), StringContainer.DeathByPunishment + _DeathByPunishment, guiStyle);
        GUI.Label(new Rect(0, 150, 100, 100), StringContainer.BestScore + _BestScore, guiStyle);
        GUI.Label(new Rect(0, 180, 100, 100), StringContainer.AverageScore + _ActualGenerationAverageScore, guiStyle);
        GUI.Label(new Rect(0, 210, 100, 100), StringContainer.BestAverageScore + _BestGenerationAverageScore, guiStyle);
        GUI.Label(new Rect(0, 240, 100, 100), StringContainer.BestAverageScoreIndex + _IndexOfBestGenerationAverageScore, guiStyle);
        GUI.EndGroup();
    }

    #endregion
}
