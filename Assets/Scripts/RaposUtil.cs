using UnityEngine;

public class RaposUtil
{
    static public Quaternion LookAtPosition(Vector3 origin, Vector3 target, float offset = 0)
    {
        Quaternion quaternion = Quaternion.LookRotation(Vector3.forward, target - origin);
        quaternion.eulerAngles += Vector3.back * offset;
        return quaternion;
    }

    static public Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = vector.x;
        float ty = vector.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
