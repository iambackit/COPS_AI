using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;
using Assets.Scripts.Interfaces;

using WeightsOfAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<float>>>;

using OutputOfNeuronsInSingleLayer = System.Collections.Generic.List<float>;
using OutputOfNeuronsInAllLayer = System.Collections.Generic.List<System.Collections.Generic.List<float>>;

public class Car : MonoBehaviour
{
    #region properties
    public bool IsControlledByPlayer = false;

    public DNA DNA { get; set; }
    public bool IsAlive { get; private set; }
    public int Score { get; set; }
    public delegate void CarEventHandler(object source, EventArgs args);
    public event CarEventHandler CarEvent;

    private NeuralNetwork _NeuralNetwork;
    private Vector2 _TargetPosition;
    private float _StartDistance;
    private GameObject _Target;

    private List<float> _NeuralNetworkInputs;
    private List<float> _NeuralNeetworkOutput;

    IControllable _Controller;
    #endregion

    public void Awake()
    {
        _Controller = gameObject.AddComponent<CarController>();
        _Controller.IsControlledByPlayer = IsControlledByPlayer;
    }

    public void Initialize(GameObject target)
    {
        _NeuralNetwork = new NeuralNetwork();
        DNA = new DNA(_NeuralNetwork.WeightsOfAllLayer);
        SetParameters(target);
    }

    public void Initialize(DNA dna, GameObject target)
    {
        _NeuralNetwork = new NeuralNetwork(dna);
        DNA = dna;
        SetParameters(target);
    }

    private void SetParameters(GameObject target)
    {
        IsAlive = true;
        _Target = target;
        _TargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        _StartDistance = Vector2.Distance(transform.position, _TargetPosition);
    }

    public void Kill()
    {
        IsAlive = false;
        FreezeCar();
        ChangeAlphaAndSortingOrder();
        CalculateScore();
    }

    protected virtual void OnCarEvent()
    {
        if (CarEvent != null)
            CarEvent(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (!IsControlledByPlayer)
        {
            _TargetPosition = new Vector2(_Target.transform.position.x, _Target.transform.position.y);
            _NeuralNetworkInputs = GetComponent<LaserContainer>().GetDistances();
            _NeuralNetworkInputs.Add(_TargetPosition.x);
            _NeuralNetworkInputs.Add(_TargetPosition.y);
            _NeuralNetworkInputs.Add(transform.position.x);
            _NeuralNetworkInputs.Add(transform.position.y);
            _NeuralNeetworkOutput = _NeuralNetwork.FeedForward(_NeuralNetworkInputs);
            _Controller.Move(_NeuralNeetworkOutput);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsControlledByPlayer)
        {
            if (IsAlive)
                Kill();
        }
        else if (IsAlive && collision.gameObject.tag == StringContainer.TagMap)
        {
            Kill();
            OnCarEvent();
        }
        else if (IsAlive && collision.gameObject.tag == StringContainer.Player)
        {
            Kill();
            Score += 100;
        }
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

    private void CalculateScore()
    {
        float distance = Vector2.Distance(_TargetPosition, this.transform.position);
        float tmp = _StartDistance - distance;

        if (tmp < 0)
        {
            Score = 1;
        }
        else
        {
            tmp = Mathf.Pow(tmp, 3);
            Score = (int)tmp;
        }
        
    }

}
