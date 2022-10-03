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
            float PI8 = Mathf.PI / ANGLE_DIVISOR;
            this.rotateAngle = new Vector3(Mathf.Cos(PI8), Mathf.Sin(PI8), PI8 * 180 / Mathf.PI);
            this.input = InputSystemProvider.instance.battlePlayerInputSystem;
        }

        #region Input system

        /// <summary>
        /// Система ввода во время боя.
        /// </summary>
        private BattleInput input;
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

        /// <summary>
        /// Последнее сохраненное значение времени между нынешним кадром и предыдущим.
        /// </summary>
        private float lastTick = 1;
        public override void Move(float tick)
        {
            this.lastTick = tick;
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
        public override float GetMovementSpeed()
        {
            if (inertia.x == 0 && inertia.y == 0)
            {
                return 0;
            }
            else
            {
                Vector2 inertiaPerSecond = this.inertia / this.lastTick;
                return inertiaPerSecond.magnitude;
            }
        }

        #endregion Общие методы класса передвижения.

        #region Реализация передвижения игрока.

        /// <summary>
        /// Делитель пи для уменьшения угла поворота.
        /// </summary>
        private const Int32 ANGLE_DIVISOR = 8;
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
                for (Int32 i = 0; i < ANGLE_DIVISOR; i++)
                    if (MathF.Abs(oldAngle - rotateAngle * i) < deltaAngles)
                    {
                        angle = CorrectWithIndex(i);
                        break;
                    }

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
        private float speedOfTurn = 5;
        /// <summary>
        /// ВЫполнить поворот.
        /// </summary>
        private void PerformTurn()
        {
            if (this.isTurnOnLeft || this.isTurnOnRight)
            {
                float delatTime = Time.deltaTime;
                this.leftTimeAfterCorrect += delatTime;
                float deltaTimeAndSpeedOfTurn = delatTime * this.speedOfTurn;
                float a = this.rotateAngle.x * deltaTimeAndSpeedOfTurn;
                float b = this.rotateAngle.y * deltaTimeAndSpeedOfTurn;
                Vector2 rotate = RotateVectorWithComplex(this.direction, a, b);
                float deltaRotate = this.rotateAngle.z * deltaTimeAndSpeedOfTurn;
                Vector3 rot = this.rotation.eulerAngles;
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