using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Класс отвечающий за границы.
    /// </summary>
    public class Borders : MonoBehaviourLogger
    {
        [SerializeField]
        private Transform battleField = null;
        [SerializeField]
        private Transform rigthBorder = null;
        [SerializeField]
        private Transform leftBorder = null;
        [SerializeField]
        private Transform upBorder = null;
        [SerializeField]
        private Transform bottomBorder = null;

        /// <summary>
        /// Местоположение границ.
        /// </summary>
        public struct BorderPosition
        {
            public float right;
            public float bottom;
            public float left;
            public float up;
        }
        /// <summary>
        /// Получить позииции границ.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private BorderPosition GetBorderPosition(Transform place)
        {
            BorderPosition borderPosition = new BorderPosition();

            borderPosition.up = place.position.y + place.localScale.y / 2;
            borderPosition.bottom = place.position.y - place.localScale.y / 2;

            borderPosition.right = place.position.x + place.localScale.x / 2;
            borderPosition.left = place.position.x - place.localScale.x / 2;

            return borderPosition;
        }
        /// <summary>
        /// Получить позиции границ игрового поля.
        /// </summary>
        /// <returns></returns>
        public BorderPosition GetBorderPositionBattleField()
        {
            return GetBorderPosition(this.battleField);
        }

        /// <summary>
        /// Два вектора: положения и направления.
        /// </summary>
        public struct PositionAndDirection
        {
            /// <summary>
            /// Положение.
            /// </summary>
            public Vector2 position;
            /// <summary>
            /// Направление.
            /// </summary>
            public Vector2 direction;
        }
        /// <summary>
        /// Получить случайное положение в заданном прямоугольнике.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private Vector2 GetRandomPosition(Transform place)
        {
            BorderPosition borderPosition = GetBorderPosition(place);
            return new Vector2
                (
                Random.Range(borderPosition.left, borderPosition.right),
                Random.Range(borderPosition.up, borderPosition.bottom)
                );
        }

        /// <summary>
        /// Получить случайные положение и направление.
        /// </summary>
        /// <returns></returns>
        public PositionAndDirection GetRandomPositionAndDirection()
        {
            PositionAndDirection positionAndDirection = new PositionAndDirection();
            int side = Random.Range(0, 100);
            if (side < 25)
            {
                positionAndDirection.position = GetRandomPosition(this.leftBorder);
            }
            else if (side < 50)
            {
                positionAndDirection.position = GetRandomPosition(this.rigthBorder);
            }
            else if (side < 75)
            {
                positionAndDirection.position = GetRandomPosition(this.upBorder);
            }
            else
            {
                positionAndDirection.position = GetRandomPosition(this.bottomBorder);
            }

            //Направление всегда будет куда-то на поле боя.
            positionAndDirection.direction = GetRandomPosition(this.battleField).normalized;

            return positionAndDirection;
        }
    }
}