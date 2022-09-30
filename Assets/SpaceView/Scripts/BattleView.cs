using System;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Представление боя, заботится о том, чтобы все рисовалось.
    /// </summary>
    public class BattleView : MonoBehaviourLogger
    {
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
        public Boolean isInited
        {
            get => this.isInitedPrivate;
        }
        private void Awake()
        {
            Init();
        }
        private void Init()
        {
            PoolsKeeper.instance.Init(this.battlePrefabProvider, this.templateObject);
            this.OnInited?.Invoke();
            this.isInitedPrivate = true;
        }
    }
}
