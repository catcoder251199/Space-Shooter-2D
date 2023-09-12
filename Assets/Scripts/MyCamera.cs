using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Camera _camera;
    void Start()
    {
        _camera = this.GetComponent<Camera>();


    }

    void Update()
    {
        //Debug.Log($"Screen Space " + _camera.pixelWidth + ", " + _camera.pixelHeight);
        //Debug.Log($"Screen Space 2 " + Screen.width + ", " + Screen.height);
    }
}
