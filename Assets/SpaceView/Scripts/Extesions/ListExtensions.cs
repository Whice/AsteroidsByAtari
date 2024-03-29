﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Перемешать.
        /// <br/>Тасование Фишера-Йетса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this List<T> list, Int32 seed)
        {
            Random rand = new Random(seed);

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                (list[j], list[i]) = (list[i], list[j]);
            }
        }
        /// <summary>
        /// Перемешать.
        /// <br/>Тасование Фишера-Йетса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this List<T> list)
        {
            Random rand = new Random();

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                (list[j], list[i]) = (list[i], list[j]);
            }
        }
        /// <summary>
        /// Получить последний индекс в списке.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static Int32 GetLastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }
    }
}

