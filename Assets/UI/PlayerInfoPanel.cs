using Assets.SpaceModel;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using View;

/// <summary>
/// Информация о игроке.
/// </summary>
public class PlayerInfoPanel : MonoBehaviourLogger
{
    [SerializeField]
    private TextMeshProUGUI position = null;
    [SerializeField]
    private TextMeshProUGUI rotation = null;
    [SerializeField]
    private TextMeshProUGUI speed = null;
    [SerializeField]
    private TextMeshProUGUI lazerCharge = null;
    [SerializeField]
    private TextMeshProUGUI lazerRestore = null;

    private BattleManager battleManager;

    public void Init(BattleManager manager)
    {
        this.gameObject.SetActive(true);
        this.battleManager = manager;

        manager.onGameStarted += OnStartGame;
        manager.onGameEnded += OnEndGame;
    }

    private SpaceObjectView playerView;
    private Transform playerViewTransform;
    private Vector2 playerPosition
    {
        get => new Vector2(this.playerViewTransform.position.x, this.playerViewTransform.position.y);
    }
    private float playerRotation
    {
        get => this.playerViewTransform.rotation.eulerAngles.z;
    }
    private void Update()
    {
        if(this.playerView==null)
        {
            this.playerView = this.battleManager.playerView;
            this.playerViewTransform = this.battleManager.playerView.transform;
        }

        this.position.text = "Position: " +this.playerPosition.ToString();
        this.rotation.text = "Rotation: " + String.Format("{0:0}", this.playerRotation);
        this.speed.text = "Speed: " + String.Format("{0:f2}", this.playerView.moveComponent.GetMovementSpeed());

        var playerModelInfo = this.battleManager.battleModel.GetPlayerInfo();
        //Чтобы не было лишнего шума
        this.lazerCharge.text = "Laser charge: " + playerModelInfo.chargeCount.ToString();
        this.lazerRestore.text = "Laset time restore: " + String.Format("{0:f2}", playerModelInfo.timeToRechargeLaser);
    }

    private void OnEndGame(BattleInfo info)
    {
        this.playerView = null;
        this.playerViewTransform = null;
        this.gameObject.SetActive(false);
    }
    private void OnStartGame()
    {
        this.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        this.battleManager.onGameStarted -= OnStartGame;
        this.battleManager.onGameEnded -= OnEndGame;
    }
}