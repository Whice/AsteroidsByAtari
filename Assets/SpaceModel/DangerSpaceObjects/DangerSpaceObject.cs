using Assets.SpaceModel.Extensions;
using System;

namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal abstract class DangerSpaceObject : SpaceObject
    {
        public DangerSpaceObject(SpaceObjectType type, IModelLogger logger) : base(type, logger) 
        {
            hp = 1;
        }

        /// <summary>
        /// Счет, который добавляется после уничтожения этого объекта.
        /// </summary>
        /// <returns></returns>
        public abstract Int32 GetScore();
    }
}
