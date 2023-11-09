using UnityEngine;

namespace PlayerNS
{
    [RequireComponent(typeof(Player))]
    public class WeaponHandler : MonoBehaviour
    {
        private IShootPattern _shootPattern;
        private Player _player;
        [SerializeField] private Transform _bulletCenterSpawn;
        [SerializeField] private Transform _bulletLeftSpawn;
        [SerializeField] private Transform _bulletRightSpawn;

        [SerializeField] AudioClip _shootSoundClip;
        public AudioClip ShootSound => _shootSoundClip;

        public int FireRateStack = 0;
        public float BonusScale = 0;
        public int SpeedStack = 0;
        public Player Player => _player;
        public Transform BulletCenterSpawn => _bulletCenterSpawn;
        public Transform BulletLeftSpawn => _bulletLeftSpawn;
        public Transform BulletRightSpawn => _bulletRightSpawn;
        public IShootPattern CurrentPattern => _shootPattern;
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public void ChangeShootPattern(IShootPattern shootPattern)
        {
            _shootPattern?.OnRemoved();
            _shootPattern = shootPattern;
            _shootPattern?.OnAdded();
        }

        public void Activate()
        {
            if (_shootPattern != null && !_shootPattern.IsShooting())
                _shootPattern.Start();
        }

        public void Deactivate()
        {
            if (_shootPattern != null && _shootPattern.IsShooting())
                _shootPattern.Stop();
        }
    }
}

