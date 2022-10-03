using Assets.SpaceModel;
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
        /// Трансформ префаба спрайта выданного этому объекту.
        /// </summary>
        private Transform spritePrefab;
        /// <summary>
        /// Коллайдер, который висит на этом объекте.
        /// </summary>
        [SerializeField]
        private BoxCollider2D boxCollider = null;
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

        #region Move component

        /// <summary>
        /// Компонент для передвижения этого объекта.
        /// </summary>
        public AbstractMove moveComponent { get; private set; }
        /// <summary>
        /// Пулл компонентов для передвижения.
        /// </summary>
        private SpaceObjectMovePool movePool
        {
            get => PoolsKeeper.instance.GetSpaceObjectMovePool();
        }

        #endregion Move component

        /// <summary>
        /// Инициализировать объект.
        /// </summary>
        /// <param name="spaceObject">Модельный боевой объект.</param>
        /// <param name="battleFieldborders">Границы боевого пространства.</param>
        /// <param name="targetForBorn">Цель для появления объекта, 
        /// если требуется появление объета в определенном месте, где был другой.</param>
        public void Initialize(SpaceObject spaceObject, Borders battleFieldborders, SpaceObjectView targetForBorn=null)
        {
            Int32 type = (Int32)spaceObject.type;
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
            if (type == (Int32)SpaceObjectType.unknow
            || type > (Int32)SpaceObjectType.end)
            {
                LogError("Type of space object unknow!");
                return;
            }

            //Инициализация.
            this.modelSpaceObjectPrivate = spaceObject;

            SpaceObjectMoveInfo moveInfo = new SpaceObjectMoveInfo()
            {
                spaceObjectTransform = this.transform,
                battleFieldborders = battleFieldborders,
            };
            this.moveComponent = this.movePool.GetMoveComponent(type);
            if (targetForBorn != null && this.moveComponent is AsteroidShardMove shardMove)
                shardMove.SetTarget(targetForBorn.moveComponent);
            this.moveComponent.Init(moveInfo);
            this.moveComponent.onOutFromBattleZone += spaceObject.Destroy;

            this.gameObject.name = spaceObject.type.ToString();
            BattlePrefabPool pool = PoolsKeeper.instance.GetBattlePrefabPool();
            Transform spritePrefab = pool.GetBattlePrefab(type).transform;
            spritePrefab.SetParent(this.spriteSlot, false);
            this.spritePrefab = spritePrefab;
            this.boxCollider.size = spritePrefab.localScale;
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
            pool.PushBattlePrefab((Int32)this.modelSpaceObjectPrivate.type, this.spritePrefab.gameObject);
            this.moveComponent.onOutFromBattleZone -= this.modelSpaceObjectPrivate.Destroy;
            this.moveComponent.DestroyMoveObject();
            this.moveComponent = null;
            this.modelSpaceObjectPrivate = null;
            this.pool.PushSpaceObjectView(this);
            this.pool = null;
            this.spritePrefab = null;
            this.gameObject.name = "SpaceObjectView";
        }
        /// <summary>
        /// Требуется дополнительное действие при уничтожении объекта.
        /// </summary>
        private Boolean isNeedAdditionalAction = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpaceObjectView view = collision.gameObject.GetComponent<SpaceObjectView>();
            this.isNeedAdditionalAction = this.modelSpaceObject.CollideWithObject(view.modelSpaceObject);
        }
    }
}