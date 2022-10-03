using UnityEngine;

namespace View
{
    /// <summary>
    /// Хранитель пуллов.
    /// </summary>
    public class PoolsKeeper : MonoSingleton<PoolsKeeper>
    {
        private BattlePrefabPool battlePrefabPool;
        private SpaceObjectViewPool spaceObjectViewPool;
        private SpaceObjectMovePool spaceObjectMovePool;
        public void Init(BattlePrefabProvider battlePrefabProvider, SpaceObjectView templateObject, Borders battleFieldborders, Transform nonActiveObjectPlace)
        {
            this.battlePrefabPool = new BattlePrefabPool(battlePrefabProvider, nonActiveObjectPlace);
            this.spaceObjectViewPool = new SpaceObjectViewPool(templateObject);
            this.spaceObjectMovePool = new SpaceObjectMovePool(battleFieldborders);
        }
        public void ResetPools()
        {
            this.battlePrefabPool.Reset();
            this.spaceObjectMovePool.Reset();
            this.spaceObjectViewPool.Reset();
        }
        public BattlePrefabPool GetBattlePrefabPool()
        {
            return this.battlePrefabPool;
        }
        public SpaceObjectViewPool GetSpaceObjectViewPool()
        {
            return this.spaceObjectViewPool;
        }
        public SpaceObjectMovePool GetSpaceObjectMovePool()
        {
            return this.spaceObjectMovePool;
        }
    }
}