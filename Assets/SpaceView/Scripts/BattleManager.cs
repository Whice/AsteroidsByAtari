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
        /// <summary>
        /// Границы боевого поля.
        /// </summary>
        [SerializeField]
        private Borders battleFieldBorders = null;
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
            SpaceObjectView view = pool.GetSpaceObjectView
                (
                sObject,
                this.battleFieldBorders.GetRandomPositionAndDirection()
                );
            this.activeSpaceObjects.Add(view);
            if (view.transform.parent == null)
                view.transform.SetParent(this.activeObjectPlaceInHierarchy, false);
        }
        /// <su/// <summary>
        /// Создать и нарисовать космический объект.
        /// </summary>
        /// <param name="sObject"></param>
        private void OnDestoySpaceObject(SpaceObject sObject)
        {
            SpaceObjectView soView = null;
            for (int i = 0; i < this.activeSpaceObjects.Count; i++)
            {
                if (this.activeSpaceObjects[i].modelSpaceObject == sObject)
                    soView = this.activeSpaceObjects[i];
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

        /// <summary>
        /// Выполнить окончание боя.
        /// </summary>
        /// <param name="info"></param>
        private void OnEndGame(BattleInfo info)
        {
            this.isGameStarted = false;
            for (Int32 i = 0; i < this.activeSpaceObjects.Count; i++)
            {
                this.activeSpaceObjects[i].DestroySpaceObject();
            }
        }

        private List<SpaceObjectView> activeSpaceObjects = new List<SpaceObjectView>();

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
            this.battleModel.onEndedGame -= OnEndGame;
        }
    }
}