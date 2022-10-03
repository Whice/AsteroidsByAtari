using System;
using UnityEngine;

/// <summary>
/// Провайдер для передачи префабов для боя.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Prefab Provider", menuName = "Game view/Prefab Provider")]
public class BattlePrefabProvider : ScriptableObject
{
    [Serializable]
   private class PrefabWithID
    {
#pragma warning disable CS0649
        public Int32 id;
        public GameObject prefab;
#pragma warning restore CS0649
    }

    [SerializeField]
    private PrefabWithID[] prefabs = new PrefabWithID[0];

    /// <summary>
    /// Получить клон префаба по id.
    /// </summary>
    /// <returns></returns>
    public virtual GameObject GetPrefabClone(Int32 id)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab.id == id)
            {
                return Instantiate(prefab.prefab);
            }
        }

        Debug.LogError("In " + nameof(this.name) + " not found prefab with is: " + id + "!");
        return null;
    }
}
