using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration;
    public float steering;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        float h = 0;
        float v = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            v = 1;
        else if (Input.GetKey(KeyCode.DownArrow))
            v = -1;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            h = .8f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            h=-.8f;
        }

        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }

    //void FixedUpdate()
    //{
    //    float h = 0;
    //    float v = 0;
    //    if (Input.GetKey(KeyCode.UpArrow))
    //        v = 1;
    //    else if (Input.GetKey(KeyCode.DownArrow))
    //        v = -1;

    //    if (Input.GetKey(KeyCode.LeftArrow))
    //        h = 1;
    //    else if (Input.GetKey(KeyCode.RightArrow))
    //        h = -1;
    //    float h = -Input.GetAxis("Horizontal");
    //    float v = Input.GetAxis("Vertical");

    //    Vector2 speed = transform.up * (v * acceleration);
    //    rb.AddForce(speed);

    //    float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
    //    if (direction >= 0.0f)
    //    {
    //        rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
    //    }
    //    else
    //    {
    //        rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
    //    }

    //    Vector2 forward = new Vector2(0.0f, 0.5f);
    //    float steeringRightAngle;
    //    if (rb.angularVelocity > 0)
    //    {
    //        steeringRightAngle = -90;
    //    }
    //    else
    //    {
    //        steeringRightAngle = 90;
    //    }

    //    Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
    //    //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

    //    float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

    //    Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


    //    //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

    //    rb.AddForce(rb.GetRelativeVector(relativeForce));
    //}
}
