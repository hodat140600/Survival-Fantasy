using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetChildExtension
{
    public static List<GameObject> GetAllChildren(this Transform aParent)
    {
        List<GameObject> children = new List<GameObject>();

        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();

            children.Add(c.gameObject);

            foreach (Transform t in c)
                queue.Enqueue(t);
        }

        return children;
    }
}
