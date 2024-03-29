﻿using Assets.SpaceModel;
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

        #region Input system

        /// <summary>
        /// Система ввода во время боя.
        /// </summary>
        private BattleInput battleInput;

        #endregion Input system

        /// <summary>
        /// Инфо о визуальной части.
        /// </summary>
        [SerializeField]
        private BattleView battleView = null;
        /// <summary>
        /// Место (родитель) , куда будут назначаться активные объекты.
        /// </summary>
        [SerializeField]
        private Transform activeObjectPlaceInHierarchy = null;
        private void Awake()
        {
            this.battleModel = new BattleModel();

            this.battleModel.onCreatedSpaceObject += OnCreateSpaceObject;
            this.battleModel.OnDestroedActiveObject += OnDestoySpaceObject;
            this.battleModel.onEndedGame += OnEndGame;

            if (!this.battleView.isInited)
                this.battleView.OnInited += StartGame;
            else
                StartGame();
        }

        /// <summary>
        /// Модель боя.
        /// </summary>
        public BattleModel battleModel { get; private set; }
        /// <summary>
        /// Представление игрока.
        /// </summary>
        public SpaceObjectView playerView { get; private set; }

        /// <summary>
        /// Большой астероид для создания его частей.
        /// </summary>
        private class BigSteroidForShards
        {
            public BigSteroidForShards(Int32 shardsLeftOver, SpaceObjectView targetBigAsteroid)
            {
                this.shardsLeftOver = shardsLeftOver;
                this.targetBigAsteroidPrivate = targetBigAsteroid;
            }
            /// <summary>
            /// Частей осталось создать.
            /// </summary>
            private Int32 shardsLeftOver;
            /// <summary>
            /// Цель - большой астероид для создания частей.
            /// </summary>
            private SpaceObjectView targetBigAsteroidPrivate;
            /// <summary>
            /// Цель - большой астероид для создания частей.
            /// </summary>
            public SpaceObjectView targetBigAsteroid
            {
                get
                {
                    --this.shardsLeftOver;
                    return targetBigAsteroidPrivate;
                }
            }
            /// <summary>
            /// Структуру надо уничтожить, она больше не нужна.
            /// </summary>
            public Boolean isNeedDestroy
            {
                get => this.shardsLeftOver < 1;
            }
        }
        private List<BigSteroidForShards> destroyedBigAsteroids = new List<BigSteroidForShards>();

        /// <summary>
        /// Создать и нарисовать космический объект.
        /// </summary>
        /// <param name="sObject"></param>
        private void OnCreateSpaceObject(SpaceObject sObject)
        {
            SpaceObjectViewPool pool = PoolsKeeper.instance.GetSpaceObjectViewPool();
            SpaceObjectView view = null;
            if (sObject.type == SpaceObjectType.asteroidShard)
            {
                if (this.destroyedBigAsteroids.Count > 0)
                {
                    int lastIndex = this.destroyedBigAsteroids.Count - 1;
                    BigSteroidForShards target = this.destroyedBigAsteroids[lastIndex];
                    view = pool.GetSpaceObjectView
                    (
                    sObject,
                    this.battleView.battleFieldBorders,
                    target.targetBigAsteroid
                    );
                    if (target.isNeedDestroy)
                    {
                        target.targetBigAsteroid.DestroySpaceObject();
                        this.destroyedBigAsteroids.RemoveAt(lastIndex);
                    }
                }
                else
                {
                    LogError("Attempting to create a part of an asteroid when the asteroid was not destroyed!");
                }
            }
            else
            {
                view = pool.GetSpaceObjectView
                (
                sObject,
                this.battleView.battleFieldBorders
                );
            }

            if (sObject.type == SpaceObjectType.player)
            {
                this.playerView = view;
            }

            this.activeSpaceObjects.Add(view);
            if (view.transform.parent == null)
                view.transform.SetParent(this.activeObjectPlaceInHierarchy, false);
        }
        /// <summary>
        /// Уничтожить и отправить в пулл космический объект.
        /// </summary>
        /// <param name="sObject"></param>
        private void OnDestoySpaceObject(SpaceObject sObject)
        {
            SpaceObjectView soView = null;
            int indexForRemove = -1;
            for (int i = 0; i < this.activeSpaceObjects.Count; i++)
            {
                if (this.activeSpaceObjects[i].modelSpaceObject == sObject)
                {
                    soView = this.activeSpaceObjects[i];
                    indexForRemove = i;
                    break;
                }
            }
            if (soView == null || indexForRemove == -1)
            {
                LogError($"The specified SpaceObjectView is not found among the active objects for {sObject.type}!");
                return;
            }

            if(sObject.type== SpaceObjectType.player)
            {
                this.isGameStarted = false;
            }

            if (sObject.type == SpaceObjectType.bigAsteroid && this.isGameStarted)
            {
                BigSteroidForShards target = new BigSteroidForShards(this.battleModel.shardsAfterDestroyBigAsteroidCount, soView);
                this.destroyedBigAsteroids.Add(target);
            }
            else
            {
                soView.DestroySpaceObject();
            }

            this.activeSpaceObjects.RemoveAt(indexForRemove);
        }


        /// <summary>
        /// Событие, которое срабатывает после начала игры.
        /// </summary>
        public event Action onGameStarted;
        /// <summary>
        /// Игра началась.
        /// </summary>
        private Boolean isGameStarted = false;
        /// <summary>
        /// Начать игру.
        /// </summary>
        public void StartGame()
        {
            this.battleInput = InputSystemProvider.instance.battlePlayerInputSystem;
            this.battleInput.Player.LazerShot.performed += (context) => this.battleModel.PlayerLazerShot();
            this.battleModel.StartGame();
            this.isGameStarted = true;
            this.onGameStarted?.Invoke();
        }

        /// <summary>
        /// Событие окончания игры.
        /// <br/>В аргументы передается информация об игре на момент ее окончания.
        /// </summary>
        public event Action<BattleInfo> onGameEnded;
        /// <summary>
        /// Выполнить окончание боя.
        /// </summary>
        /// <param name="info"></param>
        private void OnEndGame(BattleInfo info)
        {
            this.onGameEnded?.Invoke(info);
            this.battleInput.Player.LazerShot.performed -= (context) => this.battleModel.PlayerLazerShot();
        }

        /// <summary>
        /// Объекты участвующие в бою.
        /// </summary>
        private List<SpaceObjectView> activeSpaceObjects = new List<SpaceObjectView>();

        private void Update()
        {
            if (this.isGameStarted)
            {
                float bulletShot = this.battleInput.Player.BulletShot.ReadValue<float>();
                if(bulletShot != 0)
                    this.battleModel.PlayerBulletShot();
                float delatTime = Time.deltaTime;
                this.battleModel.Update(delatTime);
                for (int i = 0; i < this.activeSpaceObjects.Count; i++)
                    this.activeSpaceObjects[i].moveComponent.Move(delatTime);
            }
        }

        private void OnDestroy()
        {
            this.battleModel.onCreatedSpaceObject -= OnCreateSpaceObject;
            this.battleModel.OnDestroedActiveObject -= OnDestoySpaceObject;
            this.battleView.OnInited -= StartGame;
             this.battleModel.onEndedGame -= OnEndGame;
        }
    }
}