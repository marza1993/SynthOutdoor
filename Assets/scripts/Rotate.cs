using UnityEngine;

// script applicato al sole ora
public class Rotate : MonoBehaviour
{

    public float speed = 10.0f;

    Vector3 angle;
    float rotation = 0f;

    bool forward = true;

    public enum Axis
    {
        X,
        Y,
        Z
    }
    public Axis axis = Axis.X;
    public bool direction = true;

    void Start()
    {
        angle = transform.localEulerAngles;
    }

    void Update()
    {
        switch (axis)
        {
            case Axis.X:
                transform.localEulerAngles = new Vector3(AlternateRotation(speed), angle.y, angle.z);
                break;
            case Axis.Y:
                transform.localEulerAngles = new Vector3(angle.x, AlternateRotation(speed), angle.z);
                break;
            case Axis.Z:
                transform.localEulerAngles = new Vector3(angle.x, angle.y, AlternateRotation(speed));
                break;
        }

        //transform.localEulerAngles = new Vector3(RotationAlternata(w.x), RotationAlternata(w.y), RotationAlternata(w.z));

        //Debug.Log(transform.localEulerAngles);
    }

    float AlternateRotation(float speed)
    {

        if (forward)
        {
            rotation += speed * Time.deltaTime;
            if (rotation >= 180f)
            {
                forward = false;
            }
        }
        else
        {
            rotation -= speed * Time.deltaTime;
            if (rotation <= 0)
            {
                forward = true;
            }

        }

        return direction ? rotation : -rotation;
    }


}