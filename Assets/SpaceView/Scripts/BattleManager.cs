using Assets.SpaceModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Управляющий скрипт боя, заботится о том, чтобы все работало.
    /// </summary>
    public class BattleManager : MonoBehaviourLogger
    {
        /// <summary>
        /// Инфо о визуальной части.
        /// </summary>
        [SerializeField]
        private BattleView battleView = null;
        private void Awake()
        {
            if (!this.battleView.isInited)
                this.battleView.OnInited += StartGame;
            else
                StartGame();
        }

        /// <summary>
        /// Модель боя.
        /// </summary>
        private BattleModel battleModel = new BattleModel();

        /// <summary>
        /// Создать и нарисовать космический объект.
        /// </summary>
        /// <param name="sObject"></param>
        private void OnCreateSpaceObject(SpaceObject sObject)
        {
            SpaceObjectViewPool pool = PoolsKeeper.instance.GetSpaceObjectViewPool();
            this.activeSpaceObject.Add(pool.GetSpaceObjectView(sObject));
        }
        /// <su/// <summary>
        /// Создать и нарисовать космический объект.
        /// </summary>
        /// <param name="sObject"></param>
        private void OnDestoySpaceObject(SpaceObject sObject)
        {
            SpaceObjectView soView = null;
            for (int i = 0; i < this.activeSpaceObject.Count; i++)
            {
                if (this.activeSpaceObject[i].modelSpaceObject == sObject)
                    soView = this.activeSpaceObject[i];
            }
            if (soView == null)
            {
                LogError("The specified SpaceObjectView is not found among the active objects!");
                return;
            }
            soView.DestroySpaceObject();
        }


        /// <summary>
        /// Игра началась.
        /// </summary>
        private Boolean isGameStarted = false;
        /// <summary>
        /// Начать игру.
        /// </summary>
        public void StartGame()
        {
            this.battleModel.onCreatedSpaceObject += OnCreateSpaceObject;
            this.battleModel.OnDestroedActiveObject += OnDestoySpaceObject;
            this.battleModel.onEndedGame += OnEndGame;
            this.battleModel.StartGame();
            this.isGameStarted = true;
        }

        private void OnEndGame(BattleInfo info)
        {

        }

        private List<SpaceObjectView> activeSpaceObject = new List<SpaceObjectView>();

        private void Update()
        {
            if (this.isGameStarted)
            {
                this.battleModel.Update(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            this.battleModel.onCreatedSpaceObject -= OnCreateSpaceObject;
            this.battleModel.OnDestroedActiveObject -= OnDestoySpaceObject;
            this.battleView.OnInited -= StartGame;
        }
    }
}