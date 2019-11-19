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
    private int frames = 0;
    private List<float> _Distances;
    private List<float> _NeuralNeetworkOutput;
    private Vector2 _TargetPosition;
    #endregion

    public void Initialize(GameObject target)
    {
        _NeuralNetwork = new NeuralNetwork();
        DNA = new DNA(_NeuralNetwork.WeightsOfAllLayer);
        _IsAlive = true;
        _TargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        Debug.Log(_TargetPosition);
    }

    public void Initialize(DNA dna, GameObject target)
    {
        _NeuralNetwork = new NeuralNetwork(dna);
        DNA = dna;
        _IsAlive = true;
        //_Target = target;
    }

    void Update()
    {
        //frames++;
        //if (frames % 5 == 0 &&_IsAlive)
        //{
            _Distances = GetComponent<LaserContainer>().GetDistances();
            _NeuralNeetworkOutput = _NeuralNetwork.FeedForward(_Distances);
        //}
        GetComponent<CarController>().Move(_NeuralNeetworkOutput);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_IsAlive && collision.gameObject.tag == StringContainer.TagMap)
        {
            _IsAlive = false;
            FreezeCar();
            ChangeAlphaAndSortingOrder();
        }
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
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void ChangeAlphaAndSortingOrder()
    {
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.6f); //reduce the alpha channel by 60%
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

}
