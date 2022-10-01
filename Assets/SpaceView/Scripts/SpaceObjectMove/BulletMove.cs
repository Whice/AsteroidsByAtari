using UnityEngine;

namespace View
{
    public class BulletMove : AbstractMove
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.speed = 7;
            this.type = Assets.SpaceModel.SpaceObjectType.simpleBullet;
        }
    }
}