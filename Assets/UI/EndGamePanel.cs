using Assets.SpaceModel;
using TMPro;
using UnityEngine;
using View;

namespace UI
{
    /// <summary>
    /// Панелька, которая появляется при окончании игры.
    /// </summary>
    public class EndGamePanel : MonoBehaviourLogger
    {
        [SerializeField]
        private TextMeshProUGUI scoreText = null;
        private BattleManager battleManager;

        public void Init(BattleManager manager)
        {
            this.gameObject.SetActive(false);
            this.battleManager = manager;
            manager.onGameEnded += OnEndGame;
        }

        private void OnEndGame(BattleInfo info)
        {
            this.scoreText.text = "Score: " + info.score.ToString();
            this.gameObject.SetActive(true);
        }
        public void StartGameButtonClick()
        {
            this.battleManager.StartGame();
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            this.battleManager.onGameEnded -= OnEndGame;
        }
    }
}