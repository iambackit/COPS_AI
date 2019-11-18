using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class LaserContainer : MonoBehaviour, ILaserCreatable
{
    private List<float> _Distances;
    private List<GameObject> Container;
    private int LaserCount = 5;

    void Start()
    {
        _Distances = new List<float>();
        Container = new List<GameObject>();
        GenerateLasers(LaserCount);
    }

    public List<float> GetDistances()
    {
        _Distances.Clear();

        foreach(GameObject laser in Container)
        {
            _Distances.Add(laser.GetComponent<Laser>().Distance);
        }

        return _Distances;
    }

    public void GenerateLasers(int laserCount)
    {
        float viewAngle = 180;
        float normalizeAngle = viewAngle / 2;
        float currentAngle = 0;
        float angleDifferences = viewAngle / (laserCount - 1);

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
            Container.Add(laserGameobject);
        }
    }



}
