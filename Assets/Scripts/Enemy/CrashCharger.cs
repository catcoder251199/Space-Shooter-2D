using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class CrashCharger : MonoBehaviour
    {
        private enum State
        {
            None,
            MoveToScreen,
            WaitAndLook,
            Charge
        }
        private State _currentState;

        private Player _target;

        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private float _chargeSpeed = 5.0f;

        [SerializeField] private float _rotateSpeed = 30.0f;
        [SerializeField] private float _rotationTime = 2f;
        private float _rotatedTime = 0f;

        private Vector3 _onScreenPos;

        [SerializeField] float _offsetFromBounds = 4f; // in pixels
        [SerializeField] Vector2 _rectSize;

        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            TransitionTo(State.MoveToScreen);
        }
        private void Update()
        {
            switch (_currentState)
            {
                case State.MoveToScreen:
                    MoveToScreen();
                    break;
                case State.WaitAndLook:
                    WaitAndLook();
                    break;
                case State.Charge:
                    Charge();
                    break;
            }
        }

        private void TransitionTo(State newState)
        {
            //if (newState == State.None)
            //{
            //    // Do nothing
            //}
            
            if(newState == State.MoveToScreen)
            {
                OnMoveToScreenStateEntered();
                _currentState = newState;
            }
            else if (newState == State.WaitAndLook)
            {
                OnWaitAndLookStateEntered();
                _currentState = newState;
            }
            else if (newState == State.Charge)
            {
                OnChargeStateEntered();
                _currentState = newState;
            }
        }


        //---WaitAndLook State---Begin---
        private Vector3 GetRandomOffScreenPosition()
        {
            var sides = new Helper.Cam.Side[] {
            Helper.Cam.Side.Top,
            Helper.Cam.Side.Left,
            Helper.Cam.Side.Right};
            var randSide = sides[Random.Range(0, sides.Length)];
            Vector2 retPos = Vector2.zero;
            switch (randSide)
            {
                case Helper.Cam.Side.Top:
                    retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0f, 1f); break;
                case Helper.Cam.Side.Left:
                    retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0.7f, 0.95f); break;
                case Helper.Cam.Side.Right:
                    retPos = Helper.Cam.GetRandomPosOnSide(randSide, _offsetFromBounds, 0, 0.7f, 0.95f); break;
            }
            return retPos;
        }
        private Vector2 GetRandomPositionInRect(float left, float right, float top, float bottom) // left, right, top, bottom are normalized
        {
            return Helper.Cam.GetRandomPositionInRect(left, right, top, bottom);
        }

        private void OnMoveToScreenStateEntered()
        {
            this.transform.position = GetRandomOffScreenPosition();
            _onScreenPos = GetRandomPositionInRect(0.1f, 0.9f, 0.55f, 0.75f);
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, _onScreenPos - this.transform.position);
        }
        private void MoveToScreen()
        {
            if (Vector2.Distance(this.transform.position, _onScreenPos) > Mathf.Epsilon)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, _onScreenPos, Time.deltaTime * _speed);
            }
            else
            {
                TransitionTo(State.WaitAndLook);
            }
        }
        //---WaitAndLook State---End---


        //---WaitAndLook State---Begin---
        private void OnWaitAndLookStateEntered()
        {
            _rotatedTime = 0f;
        }
        private void WaitAndLook() // stay still and keep looking at the player
        {
            if (!_target.IsAlive())
                return;

            if (_rotatedTime < _rotationTime)
            {
                var directionToTarget = _target.transform.position - this.transform.position;
                directionToTarget.z = this.transform.position.z; // Keep the z coord of this object
                var targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
                _rotatedTime += Time.deltaTime;
            }
            else
            {
                TransitionTo(State.Charge);
            }
        }
        //---WaitAndLook State---End---


        //---Charge State---Begin---
        private void OnChargeStateEntered()
        {
        }
        private void Charge() // Rush forward and attack the player
        {
            if (!_target.IsAlive())
                return;

            if (!IsOutOfScreen())
            {
                this.transform.Translate(Vector3.up * Time.deltaTime * _chargeSpeed);
            }
            else
            {
                TransitionTo(State.MoveToScreen);
            }
        }
        private bool IsOutOfScreen()
        {
            // Transform this object's position from world space to screen space
            // Then attach tranformed position with a defined rect
            // This object is out of screen if the obtained rect is outside of screen
            Camera cam = Camera.main;
            var screenPosition = cam.WorldToScreenPoint(this.transform.position);
            var rectInScreenSpace = new Rect();
            rectInScreenSpace.center = screenPosition;
            rectInScreenSpace.size = _rectSize;

            return rectInScreenSpace.xMax < Screen.safeArea.xMin
                || rectInScreenSpace.xMin > Screen.safeArea.xMax
               || rectInScreenSpace.yMin > Screen.safeArea.yMax
               || rectInScreenSpace.yMax < Screen.safeArea.yMin;
        }
        //---WaitAndLook State---End---
    }
}

