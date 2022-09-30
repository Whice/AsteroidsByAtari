﻿using Assets.SpaceModel;
using System;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Объект в бою.
    /// </summary>
    public class SpaceObjectView : MonoBehaviourLogger
    {
        /// <summary>
        /// Слот, куда будет помещен префаб со спрайтом боевого объекта.
        /// </summary>
        [SerializeField]
        private Transform spriteSlot = null;
        /// <summary>
        /// Модельный боевой объект.
        /// </summary>
        private SpaceObject modelSpaceObjectPrivate;
        /// <summary>
        /// Модельный боевой объект.
        /// </summary>
        public SpaceObject modelSpaceObject
        {
            get => this.modelSpaceObjectPrivate;
        }

        private void UpdatePositionAndDirection()
        {
        }
        /// <summary>
        /// Установить начальнуые положение и направление.
        /// </summary>
        /// <param name="positionAndDirection"></param>
        private void InitPositionAndDirection(in Borders.PositionAndDirection positionAndDirection)
        {
            if ((Int32)this.modelSpaceObject.type == (Int32)SpaceObjectType.player)
            {
                this.transform.localPosition = new Vector3(0, 0, 0);
                this.transform.TransformDirection(new Vector3(0, 1, 0));
            }
            else
            {
                this.transform.localPosition = positionAndDirection.position;
                this.transform.TransformDirection(new Vector3
                    (
                    positionAndDirection.direction.x,
                    positionAndDirection.direction.y,
                    0
                    ));
            }
        }
        /// <summary>
        /// Инициализировать объект.
        /// </summary>
        /// <param name="spaceObject">Модельный боевой объект.</param>
        public void Initialize(SpaceObject spaceObject, in Borders.PositionAndDirection positionAndDirection)
        {
            //Контракты.
            if (spaceObject == null)
            {
                LogError("Arguments contains null value!");
                return;
            }
            if(this.spriteSlot==null)
            {
                LogError("spriteSlot is null!");
                return;
            }
            if ((Int32)spaceObject.type == (Int32)SpaceObjectType.unknow
            || (Int32)spaceObject.type > (Int32)SpaceObjectType.end)
            {
                LogError("Type of space object unknow!");
                return;
            }

            //Инициализация.
            this.modelSpaceObjectPrivate = spaceObject;

            InitPositionAndDirection(positionAndDirection);

            this.gameObject.name = spaceObject.type.ToString();
            BattlePrefabPool pool = PoolsKeeper.instance.GetBattlePrefabPool();
            Transform spritePrefab = pool.GetBattlePrefab((Int32)spaceObject.type).transform;
            spritePrefab.SetParent(this.spriteSlot, false);
        }


        /// <summary>
        /// Пул космических объектов.
        /// </summary>
        private SpaceObjectViewPool pool;
        /// <summary>
        /// Передать пулл в объект.
        /// </summary>
        /// <param name="pool"></param>
        public void InitPool(SpaceObjectViewPool pool)
        {
            this.pool = pool;
        }
        /// <summary>
        /// Обнуляет данные космисекого объекта и помещает его в пулл.
        /// </summary>
        public void DestroySpaceObject()
        {
            BattlePrefabPool pool = PoolsKeeper.instance.GetBattlePrefabPool();
            pool.PushBattlePrefab((Int32)this.modelSpaceObjectPrivate.type, this.spriteSlot.gameObject);
            this.spriteSlot = null;
            this.modelSpaceObjectPrivate = null;
            this.pool.PushSpaceObjectView(this);
            this.gameObject.name = "SpaceObjectView";
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpaceObjectView view = collision.gameObject.GetComponent<SpaceObjectView>();
            //this.modelSpaceObject.CollideWithObject(view.modelSpaceObject);
        }
    }
}