using System;

namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal class BigAsteroid : DangerSpaceObject
    {
        public BigAsteroid(IModelLogger logger) : base(SpaceObjectType.bigAsteroid, logger) { }

        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            Boolean result = base.CollideWithObject(spaceObject);

            //При попадании пули в большой астероид,
            //он должен расколоться на маленькие.
            if(spaceObject.type == SpaceObjectType.simpleBullet)
            {
                result = true;
            }

            return result;
        }

        public override int GetScore()
        {
            return 3;
        }
    }
}
