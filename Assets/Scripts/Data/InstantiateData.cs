using UnityEngine;

namespace Assets.Scripts.Data
{
    class InstantiateData
    {
        protected GameObject _Prefab;
        protected Vector2 _Position;
        protected Quaternion _Rotation;

        public InstantiateData(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            this._Prefab = prefab;
            this._Position = position;
            this._Rotation = rotation;
        }
    }
}
