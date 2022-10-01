using UnityEngine;

namespace View
{
    public class BigAsteriodMove : AbstractMove
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.speed = 1;
            this.type = Assets.SpaceModel.SpaceObjectType.bigAsteroid;
        }
    }
}
