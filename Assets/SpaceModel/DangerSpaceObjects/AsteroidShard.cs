namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal class AsteroidShard : DangerSpaceObject
    {
        public AsteroidShard(IModelLogger logger) : base(SpaceObjectType.asteroidShard, logger) { }

        public override int GetScore()
        {
            return 1;
        }
    }
}
