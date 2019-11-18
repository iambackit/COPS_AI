using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

using WeightsOfAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<float>>>;

using OutputOfNeuronsInSingleLayer = System.Collections.Generic.List<float>;
using OutputOfNeuronsInAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;

public class Car : CarController
{
    #region properties
    public DNA DNA { get; set; }
    public bool Punished { get; private set; }
    public int Score { get; set; }

    private NeuralNetwork _NeuralNetwork;
    private bool _IsAlive = true;
    #endregion

    public void Initialize()
    {
        _NeuralNetwork = new NeuralNetwork();
        DNA = new DNA(_NeuralNetwork.WeightsOfAllLayer);
        _IsAlive = true;
    }

    public void Initialize(DNA dna)
    {
        //_NeuralNetwork = new NeuralNetwork(dna);
        _NeuralNetwork.ModifyDNA(dna);
        DNA = dna;
        _IsAlive = true;
    }

    void Update()
    {
        if (_IsAlive)
        {
            try
            {
                List<float> distances = GetComponent<LaserContainer>().GetDistances();
                List<float> neuralNetworkOutput = _NeuralNetwork.FeedForward(distances);

                GetComponent<CarController>().Move(neuralNetworkOutput);
            }
            catch(System.Exception e)
            {
                int k = 3;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (_IsAlive && collision.gameObject.tag == StringContainer.TagMap)
        //{
        //    //_AIManager.DestroyCar(this.gameObject);
        //    _IsAlive = false;
        //    FreezeCar();
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (_IsAlive && collision.gameObject.tag == StringContainer.TagScore)
        //{
        //    CalculateScore(collision.gameObject.name);
        //}
    }

    private void CalculateScore(string colliderName)
    {
        //int convertedColliderName = int.Parse(colliderName);

        //if (convertedColliderName == _NextScoreCollider)
        //{
        //    _Score+=10;
        //    _AIManager.SetScore(this.gameObject); ;

        //    if (_NextScoreCollider == _ScoreSystemChildrenCount)
        //        _NextScoreCollider = 0;
        //    else
        //        _NextScoreCollider++;
        //}
        //else
        //    Punished = true;

        //if (_Score >= 500)
        //{
        //    _AIManager.DestroyCar(this.gameObject);
        //    FreezeCar();
        //}
    }

    private void FreezeCar()
    {
        //Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
