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
    #endregion

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        _NeuralNetwork = new NeuralNetwork();
        WeightsOfAllLayer weights = _NeuralNetwork.WeightsOfAllLayer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AIManager aiManager = GameObject.Find("AIManager").GetComponent<AIManager>();
        aiManager.DestroyCar(this.gameObject);
    }
}
