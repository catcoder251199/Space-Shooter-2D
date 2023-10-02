using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateBoxCollider : MonoBehaviour
{

    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        BoxCollider2D camBox = GetComponent<BoxCollider2D>();

        camBox.size = new Vector2(cam.orthographicSize * 2 * cam.aspect,cam.orthographicSize * 2);
    }
}
