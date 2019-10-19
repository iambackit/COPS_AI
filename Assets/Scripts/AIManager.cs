using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;
using System.Linq;

public class AIManager : MonoBehaviour
{
    public enum NewPopulationGeneratedMethod
    {
        HalfBestHalfCrossovered,
        IgnorePunishedIndividuals,
        ChanceByScore
    }

    #region public
    public GameObject CarPrefab;
    public int Population = 50;
    public NewPopulationGeneratedMethod SelectedMethod;
    #endregion

    #region private
    private List<GameObject> _Cars = new List<GameObject>();
    private List<GameObject> _NewPopulation;

    private DNA _First;
    private DNA _Second;

    private GUIStyle guiStyle;

    private int _Generation = 0;
    private int _ActiveCars = 0;

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
        GenerateFirstPopulation();
        guiStyle = new GUIStyle();
    }

    public void DestroyCar(GameObject gameObject)
    {
        gameObject.SetActive(false);
        SetCarDeath(gameObject);

        if (_ActiveCars == 1)
        {
            _TempAverageScore = 0;
            SelectPopulationGenerator(SelectedMethod);
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

    private void GenerateFirstPopulation()
    {
        _ActiveCars = Population;
        for (int i = 0; i < Population; i++)
        {
            GameObject gameObjectCar = Instantiate(CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            gameObjectCar.GetComponent<Car>().Initialize();
            _Cars.Add(gameObjectCar);
        }
        _Generation++;
    }

    private void SelectPopulationGenerator(NewPopulationGeneratedMethod method)
    {
        switch (method)
        {
            case NewPopulationGeneratedMethod.HalfBestHalfCrossovered:
                GenerateNewPopulationHalfBestHalfCrossovered();
                break;
            case NewPopulationGeneratedMethod.IgnorePunishedIndividuals:
                GenerateNewPopulationIgnorePunishments();
                break;
            case NewPopulationGeneratedMethod.ChanceByScore:
                GenerateNewPopulationChanceByScore();
                break;
            default:
                throw new System.NotImplementedException("Not implemented method");
        }
    }

    private void GenerateNewPopulationHalfBestHalfCrossovered()
    {
        _NewPopulation = new List<GameObject>();

        _Cars = _Cars.OrderByDescending(x => x.GetComponent<Car>().Score).ToList();

        for (int i = 0; i < Population / 2; i++)
        {
            DNA dna = _Cars[i].GetComponent<Car>().DNA;
            dna.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(dna);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < Population / 2; i++)
        {
            DNA first = _Cars[i].GetComponent<Car>().DNA;
            DNA second = _Cars[i + 1].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Cars.Count; i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
    }

    private void GenerateNewPopulationChanceByScore()
    {
        _NewPopulation = new List<GameObject>();
        _Cars = _Cars.Select(x => x).Where(x => x.GetComponent<Car>().Punished == false).ToList();
        int totalScore = _Cars.Sum(x => x.GetComponent<Car>().Score);

        int[] indexProbability = new int[totalScore];
        int actIndex = 0;
        for (int i = 0;i<_Cars.Count;i++)
        {
            for (int j = 0;j<_Cars[i].GetComponent<Car>().Score;j++)
            {
                indexProbability[actIndex] = i;
                actIndex++;
            }
        }

        for (int i = 0;i<Population;i++)
        {
            int firstCarIndex = indexProbability[Random.Range(0, totalScore-1)];
            int secondCarIndex = indexProbability[Random.Range(0, totalScore-1)];

            DNA first = _Cars[firstCarIndex].GetComponent<Car>().DNA;
            DNA second = _Cars[secondCarIndex].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Cars.Count; i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
    }

    private void GenerateNewPopulationIgnorePunishments()
    {
        _NewPopulation = new List<GameObject>();

        _Cars = _Cars.Select(x => x).Where(x => x.GetComponent<Car>().Punished == false).ToList();
        int notPunishedCarsCount = _Cars.Count;
        int howManyCarsWeNeedToAdd = Population;

        for (int i = 0; i < notPunishedCarsCount; i++)
        {
            DNA dna = _Cars[i].GetComponent<Car>().DNA;
            dna.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(dna);
            _NewPopulation.Add(newCar);
        }
        howManyCarsWeNeedToAdd -= notPunishedCarsCount;

        //get 2 random from the not punished set
        for (int i = 0; i < howManyCarsWeNeedToAdd; i++)
        {
            int firstCarIndex = Random.Range(0, notPunishedCarsCount - 1);
            int secondCarIndex = Random.Range(0, notPunishedCarsCount - 1);

            DNA first = _Cars[firstCarIndex].GetComponent<Car>().DNA;
            DNA second = _Cars[secondCarIndex].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Cars.Count; i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
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
