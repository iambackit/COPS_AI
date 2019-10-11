using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float Distance { get { return _Distance; } }

    private Vector2 _StartPosition;
    private Vector2 _EndPosition;
    private int _LayerMask = 1 << 8;
    private int _LaserLength = 20;
    private float _Distance = 0f;
    private LineRenderer _LineRenderer;

    private void Start()
    {
        _LineRenderer = gameObject.AddComponent<LineRenderer>();
        _LineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _LineRenderer.SetColors(new Color(1, 0, 0), new Color(1, 0, 0));
        _LineRenderer.SetWidth(0.05f, 0.05f);
    }

    void FixedUpdate()
    {
        _StartPosition = this.transform.position;
        _EndPosition = (Vector2)this.transform.position + (Vector2)this.transform.up * _LaserLength;
        RaycastHit2D collisonPoint = Physics2D.Raycast(_StartPosition, _EndPosition-_StartPosition, 100, _LayerMask);
        if (collisonPoint.collider != null)
        {
            _LineRenderer.SetPosition(0, _StartPosition);
            _LineRenderer.SetPosition(1, collisonPoint.point);
            _Distance = collisonPoint.distance;
        }
    }
}
