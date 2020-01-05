using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [Header("Rotations per minute around the arena.")]
    public float RotationsPerMinute = 1.0f;

    void Update()
    {
        // yAngle (in degrees) = 6 * x * deltaTime (seconds).
        // If RotationsPerMinute is equal to 1, yAngle = 360 degrees (6 * 1 * 60).
        var yAngle = 6.0f * RotationsPerMinute * Time.deltaTime;
        transform.Rotate(0f, yAngle, 0f);
    }
}