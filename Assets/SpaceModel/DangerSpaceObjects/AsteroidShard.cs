namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal class AsteroidShard : DangerSpaceObject
    {
        public AsteroidShard(IModelLogger logger) : base(SpaceObjectType.asteroidShard, logger) { }

        public override void SetMaxHP()
        {
            this.hp = 1;
        }
        public override int GetScore()
        {
            return this.isNeedGetScore ? 1 : 0;
        }
    }
}
