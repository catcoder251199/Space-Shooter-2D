using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class SelfDestructor : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private float _offsetFromBounds = 2f;
    [SerializeField] private int _damage = 200;

    [SerializeField, Header("Count down")] private float _countDownTime = 5f;
    [SerializeField] private float _countDownDelay = 0f;
    private bool _isCountDownTriggered = false;

    //[SerializeField, Header("Attached")] GameObject _explosionVFX;
    [SerializeField, Header("Attached")] private DamagableExplosion _explosionPrefab;
    [SerializeField] private Blink2Colors _blinkAnimator;
    [SerializeField] private RectTransform _canvasUI;
    private Quaternion _canvasUIOriginalRotation;
    [SerializeField] private TextMeshProUGUI _countDownText;

    private Player _target;
    private Rigidbody2D _rb;
    private Health _health;

    private Vector3 _moveDirection;
    private bool _following = false;

    private void Awake()
    {
        _blinkAnimator = GetComponent<Blink2Colors>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    void Start()
    {
        _target = GameManager.Instance.Player;
        if (_target == null)
            Debug.LogError("SelfDestructor.Start(): _target == null");
        StartCoroutine(StartRoutine());
    }
    private IEnumerator StartRoutine()
    {
        this.transform.position = GetStartPosition(); // set up first position
        if (_blinkAnimator != null)
            _blinkAnimator.StartAnimationWithTimer(); // make object blink
        _canvasUIOriginalRotation = _canvasUI.rotation; 
        _following = true; // trigger flag to make object start follow target
        yield return new WaitForSeconds(_countDownDelay);
        _isCountDownTriggered = true; // trigger flag to make object start count down to zero
    }
    private void Update()
    {
        if (_countDownTime <= 0)
        {
            OnDied();
        }
        else
        {
            if (_isCountDownTriggered)
                _countDownTime -= Time.deltaTime;
        }

        _moveDirection = (_target.transform.position - transform.position).normalized;
        _canvasUI.rotation = _canvasUIOriginalRotation;
    }

    void FixedUpdate()
    {
        if (_following && _target.IsAlive())
        {
            _rb.velocity = _moveDirection * _speed;
            
            Vector2 direction = _target.transform.position - transform.position;
            direction.Normalize();
            float step = Time.fixedDeltaTime * _rotateSpeed;
            float toTargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            float nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, toTargetAngle, step);
            _rb.MoveRotation(nextAngle);
        }
    }

    private void LateUpdate()
    {
        _canvasUI.position = (Vector2) this.transform.position + new Vector2(0.5f, -0.5f);
        _countDownText.text = ((int) _countDownTime).ToString();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamagableCollider hitCollider = collision.GetComponent<DamagableCollider>();
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag(PlaySceneGlobal.Instance.Tag_PlayerBullet))
            {
                var bullet = hitCollider.GetComponent<BulletBase>();
                if (bullet != null)
                    bullet.TriggerHitVFX();
                bool isCritical = false;
                int damage = hitCollider.GetCalculatedDamage(out isCritical);
                TakeDamage(damage, isCritical);
                Destroy(hitCollider.gameObject);
            }
        }
    }

    private void TakeDamage(int damage, bool isCritical = false)
    {
        _health.SetHealth(_health.GetHealth() - Mathf.Max(0, damage));
        DamagePopup.Create(damage, transform.position, isCritical);
        if (_health.GetHealth() <= 0)
        {
            OnDied();
        }
    }
    private void OnDied()
    {
        Destroy(gameObject);
        if (_explosionPrefab != null)
        {
            DamagableExplosion explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            explosion.SetDamage(_damage);
        }
    }
}
