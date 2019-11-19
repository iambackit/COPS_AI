using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class CarController : MonoBehaviour, IControllable
{
    #region public
    private float Acceleration = 3;
    private float Steering = 10;
    public bool IsControlledByPlayer = false;
    #endregion

    #region private
    private Rigidbody2D rb;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(List<float> inputs)
    {
        float v = 0;
        float h = 0;

        inputs[0] = inputs[0] * 2 - 1;
        inputs[1] = inputs[1] * 2 - 1;

        v = inputs[0];
        h = inputs[1];

        Vector2 speed = transform.up * (v * Acceleration);
        rb.AddForce(speed);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * Steering * (rb.velocity.magnitude / 5.0f);
        }
        else
        {
            rb.rotation -= h * Steering * (rb.velocity.magnitude / 5.0f);
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

    void FixedUpdate()
    {
        if (IsControlledByPlayer)
        {
            float h = -Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 speed = transform.up * (v * Acceleration);
            rb.AddForce(speed);

            float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
            if (direction >= 0.0f)
            {
                rb.rotation += h * Steering * (rb.velocity.magnitude / 5.0f);
            }
            else
            {
                rb.rotation -= h * Steering * (rb.velocity.magnitude / 5.0f);
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
    }

}
