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
    public DNA DNA {get { return _DNA; } }
    #endregion
    #region private
    private NeuralNetwork _NeuralNetwork;
    private DNA _DNA;
    private bool _Initialized = false;

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
        if (_Initialized)
        {
            List<float> distances = GetComponent<LaserContainer>().GetDistances();
            List<float> output = _NeuralNetwork.FeedForward(distances);

            GetComponent<CarController>().Move(output);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Tag.Map)
        {
            AIManager aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
            aiManager.DestroyCar(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLLISION HAPPENED");

        if (collision.gameObject.tag == Tag.Score)
        {
            CalculateScore(collision.gameObject.name);
        }
    }

    private void CalculateScore(string colliderName)
    {
        int convertedColliderName = int.Parse(colliderName);
        //Debug.Log(convertedColliderName);
        //Debug.Log(_NextScoreCollider);

        if (convertedColliderName == _NextScoreCollider)
        {
            _Score++;
            if (_NextScoreCollider == 8)
                _NextScoreCollider = 0;
            else
                _NextScoreCollider++;
            //_NextScoreCollider = (_NextScoreCollider == 8) ? 0 : _NextScoreCollider++;
            Debug.Log(_NextScoreCollider);
        }
    }
}
