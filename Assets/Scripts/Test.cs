using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float randomTime = 0.5f;
    void Start()
    {
        StartCoroutine(RandomRoutine());
    }

    public enum CamSide
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }

    public IEnumerator RandomRoutine()
    {
        while (true)
        {
            transform.position = Helper.Cam.GetRandomPosOn1Side(
                new Helper.Cam.Side[] { 
                    Helper.Cam.Side.Top, 
                    Helper.Cam.Side.Left, 
                    Helper.Cam.Side.Right },
                0, 0, 0.5f, 1);
            yield return new WaitForSeconds(randomTime);
        }
    }

    Vector3 GetRandomPosOn1Side(CamSide[] sides, float offset = 0, float inputZ = 0)
    {
        int randIdx = Random.Range(0, sides.Length);
        switch (sides[randIdx]) {
            case CamSide.Left:
                return GetLeftSideRandomPos(offset, inputZ);
            case CamSide.Right:
                return GetRightSideRandomPos(offset, inputZ);
            case CamSide.Top:
                return GetTopSideRandomPos(offset, inputZ);
            case CamSide.Bottom:
                return GetBottomSideRandomPos(offset, inputZ);
            default:
                return Vector3.zero;
        }
    }
    Vector3 GetTopSideRandomPos(float offset = 0f, float inputZ = 0)
    {
      
        return GetTopSideRandomPos(offset, inputZ, 0, 1);
    }

    Vector3 GetTopSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
    {
        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        Vector3 camPos = cam.transform.position;

        minRange = Mathf.Clamp(minRange, 0f, 1f);
        maxRange = Mathf.Clamp(maxRange, minRange, 1f);
        float absMinRange = Mathf.Lerp(-camHalfWidth, camHalfWidth, minRange);
        float absMaxRange = Mathf.Lerp(-camHalfWidth, camHalfWidth, maxRange);

        return new Vector3(camPos.x + Random.Range(absMinRange, absMaxRange), camPos.y + camHalfHeight + offset, inputZ);
    }

    Vector3 GetRightSideRandomPos(float offset = 0f, float inputZ = 0)
    {
       
        return GetRightSideRandomPos(offset, inputZ, 0, 1);
    }

    Vector3 GetRightSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
    {
        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        Vector3 camPos = cam.transform.position;

        minRange = Mathf.Clamp(minRange, 0f, 1f);
        maxRange = Mathf.Clamp(maxRange, minRange, 1f);
        float absMinRange = Mathf.Lerp(-camHalfHeight, camHalfHeight, minRange);
        float absMaxRange = Mathf.Lerp(-camHalfHeight, camHalfHeight, maxRange);

        return new Vector3(camHalfWidth + offset, camPos.y + Random.Range(absMinRange, absMaxRange), inputZ);

    }

    Vector3 GetLeftSideRandomPos(float offset = 0f, float inputZ = 0)
    {
        return GetLeftSideRandomPos(offset, inputZ, 0, 1);
    }

    Vector3 GetLeftSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
    {
        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        Vector3 camPos = cam.transform.position;

        minRange = Mathf.Clamp(minRange, 0f, 1f);
        maxRange = Mathf.Clamp(maxRange, minRange, 1f);
        float absMinRange = Mathf.Lerp(-camHalfHeight, camHalfHeight, minRange);
        float absMaxRange = Mathf.Lerp(-camHalfHeight, camHalfHeight, maxRange);

        return new Vector3(-camHalfWidth - offset, camPos.y + Random.Range(absMinRange, absMaxRange), inputZ);

    }

    Vector3 GetBottomSideRandomPos(float offset = 0f, float inputZ = 0)
    {
        return GetBottomSideRandomPos(offset, inputZ, 0f, 1f);
    }

    Vector3 GetBottomSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
    {
        Camera cam = Camera.main;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = cam.orthographicSize * cam.aspect;
        Vector3 camPos = cam.transform.position;

        minRange = Mathf.Clamp(minRange, 0f, 1f);
        maxRange = Mathf.Clamp(maxRange, minRange, 1f);
        float absMinRange = Mathf.Lerp(-camHalfWidth, camHalfWidth, minRange);
        float absMaxRange = Mathf.Lerp(-camHalfWidth, camHalfWidth, maxRange);

        return new Vector3(camPos.x + Random.Range(absMinRange, absMaxRange), camPos.y - camHalfHeight - offset, inputZ);
    }

}
