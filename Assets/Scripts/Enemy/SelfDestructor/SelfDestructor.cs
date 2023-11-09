using System.Collections;
using UnityEngine;
using TMPro;
using Enemy;

public class SelfDestructor : MonoBehaviour
{
    [SerializeField] private bool _playOnStart = false;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private float _offsetFromBounds = 2f;

    [SerializeField, Header("Count down")] private float _countDownTime = 5f;
    [SerializeField] private float _countDownDelay = 0f; // A time to wait before starting count down
    private bool _isCountDownTriggered = false;
    private float _countedDownTime = 0f;

    [SerializeField, Header("Attached")] private ConstantHitCollider _explosionPrefab;
    [SerializeField] private int _explosionDamage = 10; // * override the damage value on ConstantHitCollider
    [SerializeField] private Blink2Colors _blinkAnimator;
    [SerializeField] private RectTransform _canvasUI;
    private Quaternion _canvasUIOriginalRotation;
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private PooledSpawnableProduct _spawnableProduct;
    [SerializeField, Header("Sound")] AudioClip _explosionSound;

    private Player _target;
    private Rigidbody2D _rb;
    private Health _health;
    private bool _following = false;

    private void Awake()
    {
        _blinkAnimator = GetComponent<Blink2Colors>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    void Start()
    {
        if (_playOnStart)
            Initialize();
    }

    public void Initialize()
    {
        _target = GameManager.Instance.Player;
        if (_target == null)
            Debug.LogError("SelfDestructor.Start(): _target == null");
        _health.SetHealth(_health.GetMaxHealth());
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        _countedDownTime = _countDownTime; // reset the timer
        transform.position = GetStartPosition(); // set up first position
        if (_blinkAnimator != null)
            _blinkAnimator.StartAnimationWithTimer(); // make object blink
        _canvasUIOriginalRotation = _canvasUI.rotation; 
        _following = true; // trigger flag to make object start follow target
        yield return new WaitForSeconds(_countDownDelay);
        _isCountDownTriggered = true; // trigger flag to make object start count down to zero
    }
    private void Update()
    {
        if (_countedDownTime <= 0)
            OnDied();
        else if (_isCountDownTriggered)
            _countedDownTime -= Time.deltaTime;

        _canvasUI.rotation = _canvasUIOriginalRotation;
    }

    void FixedUpdate()
    {
        if (_following && _target.IsAlive())
        {
            
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            Vector2 nextPosition = Vector2.MoveTowards(_rb.position, _target.transform.position, _speed * Time.fixedDeltaTime);
            _rb.MovePosition(nextPosition);

            float step = Time.fixedDeltaTime * _rotateSpeed;
            float toTargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            float nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, toTargetAngle, step);
            _rb.MoveRotation(nextAngle);
        }
    }

    private void LateUpdate()
    {
        _canvasUI.position = (Vector2) this.transform.position + new Vector2(0.5f, -0.5f);
        _countDownText.text = ((int)_countedDownTime).ToString();
    }

    private Vector2 GetStartPosition()
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

    public void OnTakeDamage(int damage, bool isCritical = false)
    {
        DamagePopup.Create(damage, transform.position, isCritical);
        if (_health.GetHealth() <= 0)
            OnDied();
    }

    private void OnDied()
    {
        if (_explosionPrefab != null)
        {
            var explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            explosion.damage = _explosionDamage;
        }

        if (_explosionSound != null)
            SoundManager.Instance.PlayEffectOneShot(_explosionSound);

        Deactivate();
    }

    private void Deactivate()
    {
        if (_spawnableProduct != null)
        {
            _following = false;
            _isCountDownTriggered = false;
            _blinkAnimator.StopAnimation();
            _spawnableProduct.Release();
        }
        else
            Destroy(gameObject);
    }
}
