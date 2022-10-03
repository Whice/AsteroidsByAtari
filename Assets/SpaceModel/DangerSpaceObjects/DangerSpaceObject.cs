using Assets.SpaceModel.Extensions;
using System;

namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal abstract class DangerSpaceObject : SpaceObject
    {
        /// <summary>
        /// Нужно ли выдать счет за уничтожение этого объекта.
        /// </summary>
        public Boolean isNeedGetScore;
        public DangerSpaceObject(SpaceObjectType type, IModelLogger logger) : base(type, logger) 
        {
            hp = 1;
        }

        /// <summary>
        /// Счет, который добавляется после уничтожения этого объекта.
        /// </summary>
        /// <returns></returns>
        public abstract Int32 GetScore();

        public override void Destroy(Boolean isNow = false)
        {
            this.isNeedGetScore = false;
            base.Destroy(isNow);
        }
    }
}
