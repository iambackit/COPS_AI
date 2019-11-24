using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class LaserContainer : MonoBehaviour, ILaserCreatable
{
    private List<float> _Distances;
    private List<GameObject> _Container;
    private int _LaserCount = 8;

    void Start()
    {
        _Distances = new List<float>();
        _Container = new List<GameObject>();
        GenerateLasers(_LaserCount);
    }

    public List<float> GetDistances()
    {
        _Distances.Clear();

        foreach(GameObject laser in _Container)
        {
            _Distances.Add(laser.GetComponent<Laser>().Distance);
        }

        return _Distances;
    }

    public void GenerateLasers(int laserCount)
    {
        float viewAngle = 360;
        float normalizeAngle = viewAngle / 2;
        float currentAngle = 0;
        float angleDifferences = viewAngle / (laserCount);

        for (int i = 0;i<laserCount;i++)
        {
            GameObject laserGameobject = new GameObject();
            laserGameobject.name = "Laser" + i;
            laserGameobject.AddComponent<Laser>();
            laserGameobject.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            laserGameobject.transform.rotation = this.transform.rotation;

            currentAngle = i * angleDifferences - normalizeAngle;
            laserGameobject.transform.Rotate(new Vector3(0, 0, currentAngle));

            laserGameobject.transform.SetParent(transform);
            _Container.Add(laserGameobject);
        }
    }



}
