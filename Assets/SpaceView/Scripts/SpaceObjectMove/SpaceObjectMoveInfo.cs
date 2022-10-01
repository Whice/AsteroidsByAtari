using UnityEngine;

namespace View
{
    /// <summary>
    /// Информация передаваемая в компонент передвижения.
    /// </summary>
    public struct SpaceObjectMoveInfo
    {
        public Transform spaceObjectTransform;
        public  Borders battleFieldborders;
    }
}
