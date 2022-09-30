namespace View
{
    /// <summary>
    /// Хранитель пуллов.
    /// </summary>
    public class PoolsKeeper : MonoSingleton<PoolsKeeper>
    {
        private BattlePrefabPool battlePrefabPool;
        private SpaceObjectViewPool spaceObjectViewPool;
        public void Init(BattlePrefabProvider battlePrefabProvider, SpaceObjectView templateObject)
        {
            this.battlePrefabPool = new BattlePrefabPool(battlePrefabProvider);
            this.spaceObjectViewPool = new SpaceObjectViewPool(templateObject);
        }
        public BattlePrefabPool GetBattlePrefabPool()
        {
            return this.battlePrefabPool;
        }
        public SpaceObjectViewPool GetSpaceObjectViewPool()
        {
            return this.spaceObjectViewPool;
        }
    }
}