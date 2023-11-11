using UnityEngine;
using TMPro;
using DG.Tweening;

namespace GameState
{
    [RequireComponent(typeof(GameManager))]
    public class EndState : MonoBehaviour, IGameState
    {
        private GameManager _gm;
        [SerializeField] private RectTransform _endPanel;
        [SerializeField] private TextMeshProUGUI _victoryText;

        public bool Victory { get; set; }

        private void Awake()
        {
            _gm = GetComponent<GameManager>();
        }

        public void OnStateEnter() 
        {
            Debug.Log("Game End State: Enter");
            _gm.Player.Controller.enabled = false;
            _gm.Player.WeaponHandler.Deactivate();
            _gm.SpawnManager.StopSpawn();
            _gm.PowerUpSpawner.StopSpawn();
            ShowEndPanel();
        }
        private void ShowEndPanel()
        {
            if (Victory)
                _gm.UIManager.VictoryPopup.ShowVictory();
            else
                _gm.UIManager.VictoryPopup.ShowDefeat();
        }


        public void OnStateExit() 
        {
            Debug.Log("Game End State: Exit");
        }
        public void UpdateExecute() { }
        public void FixedUpdateExecute() { }
    } 
}