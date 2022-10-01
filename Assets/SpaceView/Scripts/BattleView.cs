using System;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Представление боя, заботится о том, чтобы все рисовалось.
    /// </summary>
    public class BattleView : MonoBehaviourLogger
    {
        ///Место в hierarchy для неактивных объектов.
        [SerializeField]
        private Transform nonActiveObjectPlace = null;
        [SerializeField]
        private BattlePrefabProvider battlePrefabProvider = null;
        [SerializeField]
        private SpaceObjectView templateObject = null;

        /// <summary>
        /// Инициализация прошла.
        /// </summary>
        public Action OnInited;
        /// <summary>
        /// Представление было инициализировано.
        /// </summary>
        private Boolean isInitedPrivate = false;
        /// <summary>
        /// Представление было инициализировано.
        /// </summary>
        public Boolean isInited
        {
            get => this.isInitedPrivate;
        }

        /// <summary>
        /// Границы боевого поля.
        /// </summary>
        [SerializeField]
        private Borders battleFieldBordersPrivate = null;

        /// <summary>
        /// Границы боевого поля.
        /// </summary>
        public Borders battleFieldBorders
        {
            get => this.battleFieldBordersPrivate;
        }
        private void Awake()
        {
            Init();
        }
        private void Init()
        {
            PoolsKeeper.instance.Init(this.battlePrefabProvider, this.templateObject, this.battleFieldBordersPrivate, this.nonActiveObjectPlace);
            this.OnInited?.Invoke();
            this.isInitedPrivate = true;
        }
    }
}
