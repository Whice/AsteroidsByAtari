using UnityEngine;
using View;

namespace UI
{
    public class UIManager : MonoBehaviourLogger
    {
        [SerializeField]
        private BattleManager battleManager;
#pragma warning disable CS0414
        [SerializeField]
        private EndGamePanel endGamePanelTemplate = null;
        private EndGamePanel endGamePanel;
        [SerializeField]
        private PlayerInfoPanel playerInfoPanelTemplate = null;
        private PlayerInfoPanel playerInfoPanel;
#pragma warning restore CS0414

        private void Start()
        {
            this.endGamePanel = Instantiate(this.endGamePanelTemplate);
            this.endGamePanel.Init(this.battleManager);
            this.endGamePanel.transform.SetParent(this.transform, false);

            this.playerInfoPanel = Instantiate(this.playerInfoPanelTemplate);
            this.playerInfoPanel.Init(this.battleManager);
            this.playerInfoPanel.transform.SetParent(this.transform, false);
        }
    }
}