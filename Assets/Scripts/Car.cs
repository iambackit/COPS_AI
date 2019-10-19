using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

using WeightsOfAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<float>>>;

using OutputOfNeuronsInSingleLayer = System.Collections.Generic.List<float>;
using OutputOfNeuronsInAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;

public class Car : MonoBehaviour
{
    #region public
    public DNA DNA { get { return _DNA; } }
    public int Score { get { return _Score; } }
    public bool Punished { get; private set; }
    #endregion
    #region private
    private NeuralNetwork _NeuralNetwork;
    private DNA _DNA;

    private bool _Initialized = false;
    private bool _IsAlive = true;

    private int _Score = 0;
    private int _NextScoreCollider = 0;
    #endregion

    public void Initialize()
    {
        _NeuralNetwork = new NeuralNetwork();
        _DNA = new DNA(_NeuralNetwork.WeightsOfAllLayer);
        _Initialized = true;
    }

    public void Initialize(DNA dna)
    {
        _NeuralNetwork = new NeuralNetwork(dna);
        _DNA = dna;
        _Initialized = true;
    }

    void Update()
    {
        //if (_IsAlive && _Initialized)
        if (_Initialized)
        {
            List<float> distances = GetComponent<LaserContainer>().GetDistances();
            List<float> output = _NeuralNetwork.FeedForward(distances);

            GetComponent<CarController>().Move(output);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == StringContainer.TagMap)
        {
            AIManager aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
            aiManager.DestroyCar(this.gameObject);
            _IsAlive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == StringContainer.TagScore)
        {
            CalculateScore(collision.gameObject.name);
        }
    }

    private void CalculateScore(string colliderName)
    {
        int convertedColliderName = int.Parse(colliderName);

        if (convertedColliderName == _NextScoreCollider)
        {
            _Score++;
            AIManager aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
            aiManager.SetScore(this.gameObject); ;

            if (_NextScoreCollider == GameObject.Find("ScoreSystem").transform.childCount-1)
                _NextScoreCollider = 0;
            else
                _NextScoreCollider++;
        }
        else
        {
            //_NextScoreCollider--;
            Punished = true;
            AIManager aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
            aiManager.DestroyCar(this.gameObject);
        }
    }

}
