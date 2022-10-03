using Assets.SpaceModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class SpaceObjectViewPool
    {
        /// <summary>
        /// Шаблон объекта, по которому будут созданы прочие.
        /// </summary>
        private SpaceObjectView templateObject;
        /// <summary>
        /// Пулл объектов для переиспользования.
        /// </summary>
        private Stack<SpaceObjectView> pool = new Stack<SpaceObjectView>(100);
        public void Reset()
        {
            this.pool = new Stack<SpaceObjectView>();
        }
        public SpaceObjectViewPool(SpaceObjectView templateObject)
        {
            this.templateObject = templateObject;
        }

        /// <summary>
        /// Получить космический объект.
        /// </summary>
        /// <param name="spaceObject">Модельный боевой объект.</param>
        /// <param name="battleFieldborders">Границы боевого пространства.</param>
        /// <param name="targetForBorn">Цель для появления объекта, 
        /// если требуется появление объета в определенном месте, где был другой.</param>
        public SpaceObjectView GetSpaceObjectView(SpaceObject spaceObject, Borders battleFieldborders, SpaceObjectView targetForBorn = null)
        {
            SpaceObjectView soView = null;
            if (pool.Count > 0)
            {
                soView = this.pool.Pop();
            }
            else
            {
                soView = GameObject.Instantiate(this.templateObject);
            }

            soView.Initialize(spaceObject, battleFieldborders, targetForBorn);
            soView.InitPool(this);
            soView.gameObject.SetActive(true);
            return soView;
        }
        /// <summary>
        /// Вернуть космический объект в пулл.
        /// </summary>
        public void PushSpaceObjectView(SpaceObjectView sObject)
        {
            sObject.gameObject.SetActive(false);
            this.pool.Push(sObject);
        }
    }
}