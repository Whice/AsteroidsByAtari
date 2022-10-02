using System;

namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal class BigAsteroid : DangerSpaceObject
    {
        public BigAsteroid(IModelLogger logger) : base(SpaceObjectType.bigAsteroid, logger) 
        {       
        }
        public override void SetMaxHP()
        {
            this.hp = 1;
        }

        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            Boolean result = base.CollideWithObject(spaceObject);

            //При попадании пули в большой астероид,
            //он должен расколоться на маленькие.
            if ((Int32)spaceObject.type == (Int32)SpaceObjectType.simpleBullet)
            {
                return true;
            }
            //При попадании лазером просто уничтожить большой астероид.
            else if ((Int32)spaceObject.type == (Int32)SpaceObjectType.laser)
            {
                this.isNeedSetHPZero = true;
                return false;
            }

            return result;
        }

        public override int GetScore()
        {
            return this.isNeedGetScore ? 3 : 0;
        }
    }
}
