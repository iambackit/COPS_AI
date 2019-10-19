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
    private AIManager _AIManager;

    private bool _Initialized = false;
    private bool _IsAlive = true;

    private int _Score = 0;
    private int _NextScoreCollider = 0;
    private int _ScoreSystemChildrenCount = 0;
    #endregion

    public void Initialize()
    {
        _NeuralNetwork = new NeuralNetwork();
        _DNA = new DNA(_NeuralNetwork.WeightsOfAllLayer);
        _Initialized = true;
        _AIManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        _ScoreSystemChildrenCount = GameObject.Find("ScoreSystem").transform.childCount - 1;
    }

    public void Initialize(DNA dna)
    {
        _NeuralNetwork = new NeuralNetwork(dna);
        _DNA = dna;
        _Initialized = true;
        _AIManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        _ScoreSystemChildrenCount = GameObject.Find("ScoreSystem").transform.childCount - 1;
    }

    void Update()
    {
        if (_IsAlive && _Initialized)
        {
            List<float> distances = GetComponent<LaserContainer>().GetDistances();
            List<float> output = _NeuralNetwork.FeedForward(distances);

            GetComponent<CarController>().Move(output);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_IsAlive && collision.gameObject.tag == StringContainer.TagMap)
        {
            _AIManager.DestroyCar(this.gameObject);
            _IsAlive = false;
            FreezeCar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_IsAlive && collision.gameObject.tag == StringContainer.TagScore)
        {
            CalculateScore(collision.gameObject.name);
        }
    }

    private void CalculateScore(string colliderName)
    {
        int convertedColliderName = int.Parse(colliderName);

        int actCollider = (_NextScoreCollider == 0) ? _ScoreSystemChildrenCount : _NextScoreCollider-1;
        if (convertedColliderName == _NextScoreCollider || convertedColliderName == actCollider)
        {
            _Score+=10;
            _AIManager.SetScore(this.gameObject); ;

            if (_NextScoreCollider == _ScoreSystemChildrenCount)
                _NextScoreCollider = 0;
            else
                _NextScoreCollider++;
        }
        else
        //{
            Punished = true;
        //    _IsAlive = false;
        //    FreezeCar();

        //    //AIManager aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        //    _AIManager.DestroyCar(this.gameObject);
        //}
    }

    private void FreezeCar()
    {
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
