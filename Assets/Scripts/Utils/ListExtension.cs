using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Random = UnityEngine.Random;


namespace Assets.Scripts.Utils
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this IList<T> list)  
        {  
            int listCount = list.Count;  
            while (listCount > 1) {  
                listCount--;  
                int index = Random.Range(0,listCount);
                (list[index], list[listCount]) = (list[listCount], list[index]);
            }  
        }
        public static List<T> RandomInList<T>(this IList<T> list,int number)
        {
            var randomList = new List<T>();
            for (int i = 0; i < number; i++)
            {
                //out khi het phan tu
                if (list.Count == 0)
                {
                    break;
                }
                 T item=list[Random.Range(0,list.Count)];
                list.Remove(item);
                randomList.Add(item);
            }
            return randomList;
        }

        public static List<T> GetListType<T>(this IList list)
        {
            List<T> result = new List<T>();
            foreach (var item in list)
            {
                result.Add((T)item);
            }
            return result;
        }
    }

    
}