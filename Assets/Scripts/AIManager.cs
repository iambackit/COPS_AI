using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    #region public
    public GameObject CarPrefab;
    public int Population = 50;
    public PopulationGeneratorName SelectedMethod;
    public Sprite DarkCar;
    public Text Generation;
    public Text Statistic;
    #endregion

    #region private
    private PopulationManager _PopulationManager;

    private DNA _First;
    private DNA _Second;

    private GUIStyle guiStyle;

    private int _Generation = 0;
    private int _ActiveCars;

    //for some statistic data
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

    private void FixedUpdate()
    {
        SetGenerationText();
        SetStatisticText();
    }

    public void DestroyCar(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = DarkCar;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

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

    #region buttons
    public void NextGenerationButton()
    {
        _ActiveCars = 1;
        DestroyCar(null);
    }
    #endregion

    #region calculations for statistic

    public void SetScore(GameObject gameObject)
    {
        //calculate average generation score
        _TempAverageScore++;
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
    #endregion


    #region GUI
    private void SetGenerationText()
    {
        Generation.text = 
            StringContainer.Generation + _Generation + "\n"
            + StringContainer.Population + Population + "\n"
            + StringContainer.CurrentPopulation + _ActiveCars + "\n"
            + StringContainer.MutationRate;
    }

    private void SetStatisticText()
    {
        Statistic.text =
            StringContainer.BestAverageScoreIndex + _IndexOfBestGenerationAverageScore + "\n"
            + StringContainer.AverageScore + _ActualGenerationAverageScore + "\n"
            + StringContainer.BestAverageScore + _BestGenerationAverageScore + "\n";
    }

    #endregion
}
