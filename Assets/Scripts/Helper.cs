using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Test;

namespace Helper
{
    public static class Cam
    {
        public enum Side
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        public static Vector3 GetRandomPosOn1Side(Side[] sides, float offset = 0, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
        {
            int randIdx = Random.Range(0, sides.Length);
            switch (sides[randIdx])
            {
                case Side.Left:
                    return GetLeftSideRandomPos(offset, inputZ, minRange, maxRange);
                case Side.Right:
                    return GetRightSideRandomPos(offset, inputZ, minRange, maxRange);
                case Side.Top:
                    return GetTopSideRandomPos(offset, inputZ, minRange, maxRange);
                case Side.Bottom:
                    return GetBottomSideRandomPos(offset, inputZ, minRange, maxRange);
                default:
                    return Vector3.zero;
            }
        }

        public static Vector3 GetTopSideRandomPos(float offset = 0f, float inputZ = 0)
        {

            return GetTopSideRandomPos(offset, inputZ, 0, 1);
        }

        public static Vector3 GetTopSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
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

        public static Vector3 GetRightSideRandomPos(float offset = 0f, float inputZ = 0)
        {

            return GetRightSideRandomPos(offset, inputZ, 0, 1);
        }

        public static Vector3 GetRightSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
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

        public static Vector3 GetLeftSideRandomPos(float offset = 0f, float inputZ = 0)
        {
            return GetLeftSideRandomPos(offset, inputZ, 0, 1);
        }

        public static Vector3 GetLeftSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
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

        public static Vector3 GetBottomSideRandomPos(float offset = 0f, float inputZ = 0)
        {
            return GetBottomSideRandomPos(offset, inputZ, 0f, 1f);
        }

        public static Vector3 GetBottomSideRandomPos(float offset = 0f, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
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
    };
}

