﻿using System;
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

    private List<float> _NeuralNetworkInputs;
    private List<float> _NeuralNeetworkOutput;

    IControllable _Controller;
    #endregion

    public void Awake()
    {
        _Controller = gameObject.AddComponent<CarController>();
        _Controller.IsControlledByPlayer = IsControlledByPlayer;
    }

    public void Initialize()
    {
        _NeuralNetwork = new NeuralNetwork();
        DNA = new DNA(_NeuralNetwork.WeightsOfAllLayer);
    }

    public void Initialize(DNA dna)
    {
        _NeuralNetwork = new NeuralNetwork(dna);
        DNA = dna;
        IsAlive = true;
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
            _NeuralNetworkInputs = GetComponent<LaserContainer>().GetDistances();
            _NeuralNeetworkOutput = _NeuralNetwork.FeedForward(_NeuralNetworkInputs);
            _Controller.Move(_NeuralNeetworkOutput);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsAlive && collision.gameObject.tag == StringContainer.TagMap)
        {
            Kill();
            OnCarEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == StringContainer.TagScore)
        {
            if (collision.name == Score.ToString())
            {
                Score++;
            }

            if (collision.name == "21")
            {
                int k = 3;
            }
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
        Score = Mathf.Max(1, (int)Mathf.Pow(Score, 2));
    }

}
