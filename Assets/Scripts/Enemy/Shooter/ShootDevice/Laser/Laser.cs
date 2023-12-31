using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    public UnityEvent OnLaserBeamStop;

    private enum State
    {
        None,
        Shoot,
        Disappear,
    }
    private State[] _orderedStateList = new State[3] {State.None, State.Shoot, State.Disappear};
    private int _curStateIndex = 0;
    private State CurrentState => _orderedStateList[_curStateIndex];

    [SerializeField, Header("Basic")] float _speed = 100f;
    [SerializeField] float _maxLength = 20f;
    [SerializeField] float _width = 0.3f;
    [SerializeField] float _length = 0.01f;
    [SerializeField] float _lifeTime = 3f;
    [SerializeField] float _disappearTime = 1f;
    [SerializeField] float _scaleFitOneUnit = 1f; // Modify this to adjust the scale so that when scaleY == 1, it fit one unit
    [SerializeField] LayerMask _layerMask;
    [SerializeField] bool _enableOnAwake = false;

    [SerializeField, Header("Hit")] float _hitRate = 1f;
    private float _hitNextTime = -1f;
    [SerializeField] private int _damagePerHit = 10;
    [SerializeField] ParticleSystem _hitVFX;

    private float _existedTime = 3f;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    private bool _isLaserEnabled = false;

    public int DamagePerHit
    {
        set { _damagePerHit = value; }
        get { return _damagePerHit; }
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(_width, _length, 1f);
        SetCurrentLength(0.01f);
        _spriteRenderer.enabled = _enableOnAwake; // Turn off on awake
    }
    public void SetColliderEnabled(bool enabled)
    {
        _boxCollider.enabled = enabled;
    }
    public void SetMaxLength(float maxLength) 
    {
        _maxLength = maxLength;
        if (_maxLength < _length)
        {
            SetCurrentLength(_maxLength, false);
        }
    }
    public float GetMaxLength() => _maxLength;
    public float GetCurrentLength() => transform.localScale.y;
    public void SetCurrentLength(float length, bool forceLen = false) {
        _length = length;
        if (_maxLength < _length)
        {
            if (forceLen)
                _maxLength = _length;
            else
                _length = _maxLength;
        }
        transform.localScale = new Vector3(_width, _length * _scaleFitOneUnit, transform.localScale.z);
    }

    public bool IsAvailableToShoot()
    {
        return !_isLaserEnabled && CurrentState == State.None;
    }

    public void Launch()
    {
        if (IsAvailableToShoot())
        {
            SetCurrentLength(0.01f);
            SetCurrentLength(20f);

            _spriteRenderer.enabled = true;
            _isLaserEnabled = true;
            _existedTime = _lifeTime;
            MoveToNextState();
        }
    }
    private void ShootStateUpdate()
    {
        bool hitSomething;
        if (_isLaserEnabled && _existedTime > 0)
        {
            SetCurrentLength(_length + _speed * Time.deltaTime);
            _existedTime -= Time.deltaTime;

            Vector2 startPosition = transform.position;
            Vector2 endPosition;
            Vector2 direction = transform.up;

            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _length, _layerMask);
            hitSomething = hit.collider != null;
            if (hitSomething) // hit something
            {
                //endPosition = hit.collider.transform.position;
                endPosition = hit.point;
                SetCurrentLength((endPosition - startPosition).magnitude);
                ProcessCollision(hit.collider);
            }
        }
       
        transform.localScale = new Vector3(_width, _length * _scaleFitOneUnit, transform.localScale.z);
        
        if (_existedTime <= 0 && _isLaserEnabled) // When the beam is being active, then it begins disappearing
            MoveToNextState();
        
    }
    private void DisappearStateUpdate()
    {
        float startWidth = _width;
        float endWidth = 0.01f;
        float rate = 1 / _disappearTime;
        float decreaseAmount01, absDecreaseAmount;
        if (transform.localScale.x > endWidth)
        {
            decreaseAmount01 = Mathf.Clamp01(Time.deltaTime * rate); // time(s) * rate_of_change(unit/s);
            absDecreaseAmount = decreaseAmount01 * (startWidth  - endWidth);
            transform.localScale = new Vector3(transform.localScale.x - absDecreaseAmount, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            SetCurrentLength(0.01f);
            _spriteRenderer.enabled = false;
            _isLaserEnabled = false;
            MoveToNextState();
            OnLaserBeamStop?.Invoke();
        }
    }

    private void MoveToNextState()
    {
        _curStateIndex++;
        _curStateIndex = _curStateIndex % _orderedStateList.Length;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Shoot:
                ShootStateUpdate();
                break;
            case State.Disappear:
                DisappearStateUpdate();
                break;
        }
    }

    private void ProcessCollision(Collider2D collision)
    {
        if (Time.time >= _hitNextTime)
        {
            _hitNextTime = Time.time + _hitRate;
            // Hit Player
            if (collision.CompareTag(PlaySceneGlobal.Instance.Tag_Player))
            {
                Player player = collision.attachedRigidbody.GetComponent<Player>();
                player.TakeDamage(_damagePerHit, true);
            }
        }
    }
}