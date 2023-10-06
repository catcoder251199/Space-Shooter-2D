using UnityEngine;
using UnityEngine.Windows;

namespace Helper
{
    /*
     All helper methods are useful only if we are mentioning "main Camera"
     */
    public static class Cam
    {
        public static float HalfHeight() => Camera.main.orthographicSize;
        public static float HalfWidth() => HalfHeight() * Camera.main.aspect;
        public static Vector3 WorldPos() => Camera.main.transform.position;
        public static float WorldLeft() => WorldPos().x - HalfWidth();
        public static float WorldRight() => WorldPos().x - HalfHeight();
        public static float WorldTop() => WorldPos().y + HalfHeight();
        public static float WorldBottom() => WorldPos().y - HalfHeight();

        public enum Side
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }
        public static Vector3 GetRandomPosOnSide(Side side, float offset = 0, float inputZ = 0, float minRange = 0f, float maxRange = 1f)
        {
            switch (side)
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

        public static Vector3 GetRandomPositionInRect(float left, float right, float top, float bottom)
        {
            left = Mathf.Clamp01(left);
            right = Mathf.Clamp01(right);
            top = Mathf.Clamp01(top);
            bottom = Mathf.Clamp01(bottom);

            Camera cam = Camera.main;
            return cam.ViewportToWorldPoint(new Vector3(Random.Range(left, right), Random.Range(bottom, top), cam.nearClipPlane));
        }

        public static bool IsVisibleToCamera()
        {
            return true;
        }

        public static Rect GetWorldCameraRect(Camera cam = null)
        {
            if (cam == null)
                cam = Camera.main;

            var lowerCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var upperCorner = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
            return Rect.MinMaxRect(lowerCorner.x, upperCorner.y, upperCorner.x, lowerCorner.y);
        }

        public static bool IsPositionInWorldCamRect(Vector3 position, float offsetFromBounds = 0f)
        {
            /*
             Consider offsetFromBounds as a unknown number x.
             if offsetFromBounds is positive. It means we're using a larger camera rect which has each side x units larger than the responding side of the original one .. to discern the position.
             if offsetFromBounds is negative. It means we're using a smaller camera rect which has each side x units larger than the responding side of the original one .. to discern the position.
             */
            var camRect = GetWorldCameraRect();
            return (position.x >= camRect.xMin - offsetFromBounds && position.x <= camRect.xMax + offsetFromBounds)
                && (position.y <= camRect.yMin + offsetFromBounds && position.y >= camRect.yMax - offsetFromBounds);
        }

        public static bool IsPositionOutWorldCamRect(Vector3 position, float offsetFromBounds = 0f)
        {
            /*
             Consider offsetFromBounds as a unknown number x.
             if offsetFromBounds is positive. It means we're using a larger camera rect which has each side x units larger than the responding side of the original one .. to discern the position.
             if offsetFromBounds is negative. It means we're using a smaller camera rect which has each side x units larger than the responding side of the original one .. to discern the position.
             */
            return !IsPositionInWorldCamRect(position, offsetFromBounds);
        }

        public static Vector3 GetLeftSidePos(float pos01, float offset = 0f, float inputZ = 0)
        {
            // pos01: normalized-valued position on left edge.
            // pos01 == 0 -> it's located at the lowermost point of the vertical left edge
            // pos01 == 1 -> it's located at the uppermost point of the vertical left edge

            Camera cam = Camera.main;
            float camHalfHeight = cam.orthographicSize; // Height of camera rect
            float camHalfWidth = cam.orthographicSize * cam.aspect; // Width of camera rect == Height * ratio and ratio == Height / Width
            Vector3 camPos = cam.transform.position;

            float absValue = Mathf.Lerp(-camHalfHeight, camHalfHeight, Mathf.Clamp01(pos01));
            offset = Mathf.Max(offset, 0);
            return new Vector3(-camHalfWidth - offset, camPos.y + absValue, inputZ);
        }

        public static float GetVerticalEdge01Pos(float wPosY)
        {
            /*
             wPosY: the world position of a point on a vertical edge (left or right edge).
             return pos01:
                pos01: normalized-valued position on vertical edge.
                pos01 == 0 -> it's located at the lowermost point of the vertical edge
                pos01 == 1 -> it's located at the uppermost point of the vertical edge
             */
            Camera cam = Camera.main;
            float camHalfHeight = cam.orthographicSize; // Height of camera rect
            Vector3 camPos = cam.transform.position;
            return Mathf.InverseLerp(camPos.y - camHalfHeight, camPos.y + camHalfHeight, wPosY);
        }

        public static float GetHorizontalEdge01Pos(float wPosX)
        {
            /*
             wPosX: the world position of a point on a horizontal edge (bottom or top edge).
             return pos01:
                pos01: normalized-valued position on vertical edge.
                pos01 == 0 -> it's located at the lowermost point of the horizontal edge
                pos01 == 1 -> it's located at the uppermost point of the horizontal edge
             */
            Camera cam = Camera.main;
            float camHalfWidth = cam.orthographicSize * cam.aspect; // Width of camera rect == Height * ratio and ratio == Height / Width
            Vector3 camPos = cam.transform.position;
            return Mathf.InverseLerp(camPos.x - camHalfWidth, camPos.x + camHalfWidth, wPosX);
        }

        public static Vector3 GetRightSidePos(float pos01, float offset = 0f, float inputZ = 0)
        {
            // pos01: normalized-valued position on left edge.
            // pos01 == 0 -> it's located at the lowermost point of the vertical right edge
            // pos01 == 1 -> it's located at the uppermost point of the vertical right edge

            Camera cam = Camera.main;
            float camHalfHeight = cam.orthographicSize; // Height of camera rect
            float camHalfWidth = cam.orthographicSize * cam.aspect; // Width of camera rect == Height * ratio and ratio == Height / Width
            Vector3 camPos = cam.transform.position;

            float absValue = Mathf.Lerp(-camHalfHeight, camHalfHeight, Mathf.Clamp01(pos01));
            offset = Mathf.Max(offset, 0);
            return new Vector3(camPos.x + camHalfWidth + offset, camPos.y + absValue, inputZ);
        }

        public static Vector3 GetBottomSidePos(float pos01, float offset = 0f, float inputZ = 0)
        {
            // pos01: normalized-valued position on left edge.
            // pos01 == 0 -> it's located at the leftmost point of the horizontal bottom edge
            // pos01 == 1 -> it's located at the rightmost point of the horizontal bottom edge

            Camera cam = Camera.main;
            float camHalfHeight = cam.orthographicSize; // Height of camera rect
            float camHalfWidth = cam.orthographicSize * cam.aspect; // Width of camera rect == Height * ratio and ratio == Height / Width
            Vector3 camPos = cam.transform.position;

            float absValue = Mathf.Lerp(-camHalfWidth, camHalfWidth, Mathf.Clamp01(pos01));
            offset = Mathf.Max(offset, 0);
            return new Vector3(camPos.x + absValue, camPos.y - camHalfHeight - offset, inputZ);
        }
    }
}

