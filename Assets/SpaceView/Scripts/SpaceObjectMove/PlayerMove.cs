using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace View
{
    /// <summary>
    /// Класс компонента передвижения игрока.
    /// </summary>
    public class PlayerMove : AbstractMove
    {
        #region Borders

        /// <summary>
        /// Информация о границах боевого поля.
        /// </summary>
        private Borders.BorderPosition battleFieldborders;
        private void CheckIfOutBordersBattlefield()
        {
            Vector2 position = this.position;
            Borders.BorderPosition borders = this.battleFieldborders;

            //x
            if(position.x>borders.right)
            {
                this.position = new Vector2(borders.left, position.y);
            }
            else if(position.x<borders.left)
            {
                this.position = new Vector2(borders.right, position.y);
            }

            //y
            if (position.y > borders.up)
            {
                this.position = new Vector2(position.x, borders.bottom);
            }
            else if (position.y < borders.bottom)
            {
                this.position = new Vector2(position.x, borders.up);
            }
        }
        #endregion Borders

        public PlayerMove()
        {
            this.direction = ORIGIN_DIRECTION;
            float PI2 = Mathf.PI / 2;
            this.rotateAngle = new Vector3(Mathf.Cos(PI2), Mathf.Sin(PI2), PI2 * 180 / Mathf.PI);
        }

        #region Input system

        /// <summary>
        /// Система ввода во время боя.
        /// </summary>
        private BattleInput input = new BattleInput();
        /// <summary>
        /// Подписаться на события системы ввода и включить ее.
        /// </summary>
        private void SubscribeInput()
        {
            this.input.Enable();
        }
        /// <summary>
        /// Отписаться на события системы ввода и выключить ее.
        /// </summary>
        private void UnsubscribeInput()
        {
            this.input.Disable();
        }

        #endregion Input system

        #region Общие методы класса передвижения.

        public override void Move(float tick)
        {
            //Считать значения.
            float rotateValue = this.input.Player.Rotate.ReadValue<float>();
            this.isTurnOnLeft = rotateValue == -1;
            this.isTurnOnRight = rotateValue == 1;
            this.isAccelerate = this.input.Player.Move.ReadValue<float>() != 0;

            //ВЫполнить поворот.
            PerformTurn();

            //Выполнить смещение
            this.speed = this.isAccelerate ? 0.01f : 0;
            this.inertia += this.direction * this.speed * tick;
            this.position += this.inertia;

            if (this.inertia.sqrMagnitude > 1E-30)
            {
                this.inertia -= this.inertia*FADE * tick;
                if (this.inertia.sqrMagnitude <= 1E-30)
                    this.inertia *= 0;
            Debug.Log(nameof(inertia) + ": " + inertia.ToString());
            }
            CheckIfOutBordersBattlefield();
        }
        public override void Init(in SpaceObjectMoveInfo info)
        {
            SubscribeInput();


            this.battleFieldborders = info.battleFieldborders.GetBorderPositionBattleField();

            base.Init(info);

            this.position = new Vector2(0, 0);
            this.direction = new Vector2(0, 1);

            this.type = Assets.SpaceModel.SpaceObjectType.player;
        }

        public override void DestroyMoveObject()
        {
            UnsubscribeInput();
            base.DestroyMoveObject();
        }

        #endregion Общие методы класса передвижения.

        #region Реализация передвижения игрока.

        /// <summary>
        /// Исходное напрваление.
        /// </summary>
        private Vector2 ORIGIN_DIRECTION = new Vector3(0, 1);
        /// <summary>
        /// Величина замедления.
        /// </summary>
        private const float FADE = 0.3f;
        /// <summary>
        /// Вектор силы инерции.
        /// </summary>
        private Vector2 inertia = Vector2.zero;
        /// <summary>
        /// Величина ускорения.
        /// </summary>
        private const float acceleration = 1.3f;
        /// <summary>
        /// Выполняется ускорение.
        /// </summary>
        private bool isAccelerate = false;
        /// <summary>
        /// ВЫполняется поворот влево.
        /// </summary>
        private bool isTurnOnLeft = false;
        /// <summary>
        /// ВЫполняется поворот вправо.
        /// </summary>
        private bool isTurnOnRight = false;
        /// <summary>
        /// Выполнить замедление или ускорение.
        /// </summary>
        private void PerformAccelerationOrFade()
        {
            if (this.isAccelerate)
            {
                this.speed += acceleration * Time.deltaTime;
            }
            else
            {
                if (this.inertia.sqrMagnitude > 0)
                    this.speed -= FADE * Time.deltaTime;
                else
                    this.speed = 0;
            }
        }
        /// <summary>
        /// Переменная, хранящая данные об угле поворота.
        /// x - a;
        /// y - b;
        /// z - angle in radians;
        /// </summary>
        private Vector3 rotateAngle;
        /// <summary>
        /// Времени надо перед следующей корректировкой вращения по направлению и представлению.
        /// </summary>
        private float maxTimeForCorrect = 1;
        /// <summary>
        /// Времени прошло перед следующей корректировкой вращения по направлению и представлению.
        /// </summary>
        private float leftTimeAfterCorrect = 0;
        /// <summary>
        /// Грязный флаг для определения, надо ли скорректировать вращение по направлению и представлению.
        /// </summary>
        private Boolean isCorrected = true;
        /// <summary>
        /// Скорректировать данные по повороту.
        /// </summary>
        /// <param name="angle"></param>
        private void CorrectRotateData(ref float angle)
        {
            float oldAngle = angle;
            float rotateAngle = this.rotateAngle.z;
            float deltaAngles = 0.3f;
            float CorrectWithIndex(int index)
            {
                oldAngle = rotateAngle * index;
                this.direction = ORIGIN_DIRECTION;
                for (int i = 0; i < index; i++)
                    this.direction = RotateVectorWithComplex(this.direction, this.rotateAngle.x, this.rotateAngle.y);

                this.leftTimeAfterCorrect = 0;
                this.isCorrected = true;
                return oldAngle;
            }
            if (this.isCorrected && this.leftTimeAfterCorrect > this.maxTimeForCorrect)
            {
                this.isCorrected = false;
            }
            if (!this.isCorrected)
            {
                if (MathF.Abs(oldAngle - rotateAngle * 0) < deltaAngles)
                    angle = CorrectWithIndex(0);
                else if (MathF.Abs(oldAngle - rotateAngle * 1) < deltaAngles)
                    angle = CorrectWithIndex(1);
                else if (MathF.Abs(oldAngle - rotateAngle * 2) < deltaAngles)
                    angle = CorrectWithIndex(2);
                else if (MathF.Abs(oldAngle - rotateAngle * 3) < deltaAngles)
                    angle = CorrectWithIndex(3);
                else if (MathF.Abs(oldAngle - rotateAngle * 4) < deltaAngles)
                    angle = CorrectWithIndex(4);
                else if (MathF.Abs(oldAngle - rotateAngle * 5) < deltaAngles)
                    angle = CorrectWithIndex(5);
                else if (MathF.Abs(oldAngle - rotateAngle * 6) < deltaAngles)
                    angle = CorrectWithIndex(6);
                else if (MathF.Abs(oldAngle - rotateAngle * 7) < deltaAngles)
                    angle = CorrectWithIndex(7);

            }
        }
        /// <summary>
        /// Вращение вектора на угол, представленый комплексным числом.
        /// </summary>
        /// <param name="vector">Вектор.</param>
        /// <param name="a">Действительная часть числа.</param>
        /// <param name="b">Мнимая часть числа.</param>
        /// <returns></returns>
        private Vector2 RotateVectorWithComplex(Vector3 vector, float a, float b)
        {
            return new Vector2(vector.x * a - vector.y * b, vector.x * b + vector.y * a);
        }
        /// <summary>
        /// ВЫполнить поворот.
        /// </summary>
        private void PerformTurn()
        {
            if (this.isTurnOnLeft || this.isTurnOnRight)
            {
                float delatTime = Time.deltaTime;
                this.leftTimeAfterCorrect += delatTime;
                float a = this.rotateAngle.x * delatTime;
                float b = this.rotateAngle.y * delatTime;
                Vector2 rotate = RotateVectorWithComplex(this.direction, a, b);
                float deltaRotate = this.rotateAngle.z * delatTime;
                Vector3 rot = this.spaceObjectTransform.rotation.eulerAngles;
                if (this.isTurnOnLeft)
                {
                    float newAngle = rot.z + deltaRotate;
                    CorrectRotateData(ref newAngle);
                    this.direction += rotate;
                    this.spaceObjectTransform.rotation = Quaternion.Euler(rot.x, rot.y, newAngle);
                }
                if (this.isTurnOnRight)
                {
                    float newAngle = rot.z - deltaRotate;
                    CorrectRotateData(ref newAngle);
                    this.direction = this.direction - rotate;
                    this.spaceObjectTransform.rotation = Quaternion.Euler(rot.x, rot.y, newAngle);
                }
                this.direction = this.direction.normalized;
            }
        }

        #endregion Реализация передвижения игрока.

    }
}