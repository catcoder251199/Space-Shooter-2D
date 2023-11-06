using UnityEngine;
using Enemy.CrashChargerState;

namespace Enemy
{
    public class CrashCharger : FSMEnemy
    {
        private Player _target;

        [SerializeField] private bool _playOnStart = false;
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private float _chargeSpeed = 5.0f;
        [SerializeField] private float _chargeTime = 5.0f;
        [SerializeField] private float _rotateSpeed = 360.0f;
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] float _offsetFromBounds = 4f; // in pixels
        [SerializeField] Vector2 _rectSize;
        [SerializeField] ParticleSystem _explosionEffect;
        [SerializeField] private PooledSpawnableProduct _spawnableProduct;

        private MoveToScreenState _moveToScreenState;
        private WaitAndLookState _waitAndLookState;
        private ChargeState _chargeState;
        public MoveToScreenState MoveToScreenState => _moveToScreenState;
        public WaitAndLookState WaiAndLookState => _waitAndLookState;
        public ChargeState ChargeState => _chargeState;

        private Rigidbody2D _rb;
        private Health _health;

        public Player Target => _target;
        public float Speed => _speed;
        public float ChargeSpeed => _chargeSpeed;
        public float ChargeTime => _chargeTime;
        public float WaitTime => _waitTime;
        public float RotateSpeed => _rotateSpeed;

        public Rigidbody2D Rigidbody => _rb;

        private void Awake()
        {
            _moveToScreenState = new MoveToScreenState(this);
            _waitAndLookState = new WaitAndLookState(this);
            _chargeState = new ChargeState(this);

            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            if (_playOnStart)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            _target = GameManager.Instance.Player;
            if (_target == null)
                Debug.LogError("LaserShooter.Start(): _target == null");

            _health.SetHealth(_health.GetMaxHealth());
            ChangeState(_moveToScreenState);
        }

        private void Update()
        {
            _currentState?.UpdateExecute();
        }
        private void FixedUpdate()
        {
            _currentState?.FixedUpdateExecute();
        }

        public Vector3 GetRandomOffScreenPosition()
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
        public Vector3 GetRandomOnScreenPos()
        {
            // left, right, top, bottom are normalized
            return Helper.Cam.GetRandomPositionInRect(0.1f, 0.9f, 0.55f, 0.75f);
        }
        public bool IsOutOfScreen()
        {
            // Transform this object's position from world space to screen space
            // Then attach transformed position with a defined rect
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
        public bool IsOnScreen()
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            return screenPosition.x > 0 && screenPosition.x < Screen.width;
        }

        public void OnTakeDamage(int damage, bool isCritical = false)
        {
            DamagePopup.Create(damage, transform.position, isCritical);
            if (_health.GetHealth() <= 0)
                OnDied();
        }

        private void OnDied()
        {
            if (_explosionEffect != null)
                Instantiate(_explosionEffect, transform.position, Quaternion.identity, PlaySceneGlobal.Instance.VFXParent);

            Deactivate();
        }

        private void Deactivate()
        {
            if (_spawnableProduct != null)
            {
                _rb.angularVelocity = 0f;
                _rb.velocity = Vector3.zero;
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
                _spawnableProduct.Release();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

