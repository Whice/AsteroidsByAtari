using UnityEngine;

namespace View
{
    public class AsteroidShardMove : AbstractMove
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.speed = 2.3f;
            this.type = Assets.SpaceModel.SpaceObjectType.asteroidShard;
        }
    }
}