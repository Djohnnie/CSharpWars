using System;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    #region <| Public Properties |>

    public Single RotationsPerMinute = 10.0f;

    #endregion

    void Update()
    {
        transform.Rotate(0f, 6.0f * RotationsPerMinute * Time.deltaTime, 0f);
    }
}