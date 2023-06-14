using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class GameObjectExtensions
    {


        // Update is called once per frame
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T: MonoBehaviour
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}