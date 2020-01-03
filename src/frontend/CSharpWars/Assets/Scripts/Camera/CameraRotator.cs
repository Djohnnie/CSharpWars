using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float RotationsPerMinute = 10.0f;

    void Update()
    {
        var yAngle = 6.0f * RotationsPerMinute * Time.deltaTime;
        transform.Rotate(0f, yAngle, 0f);
    }
}