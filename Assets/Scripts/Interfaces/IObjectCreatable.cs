using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Selection;

namespace Assets.Scripts.Interfaces
{
    public interface IObjectCreatable
    {
        GameObject Prefab { get; set; }
        GameObject Target { get; set; }
        List<Vector2> Positions { get; set; }
        Quaternion Rotation { get; set; }
        int Population { get; set; }
        event EventHandler<PopulationEventArgs> PopulationReduced;

    }
}
