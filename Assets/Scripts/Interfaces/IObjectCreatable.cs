using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IObjectCreatable
    {
        GameObject Prefab { get; set; }
        Vector2 Position { get; set; }
        Quaternion Rotation { get; set; }
        int Population { get; set; }
    }
}
