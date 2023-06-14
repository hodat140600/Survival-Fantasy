using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarContainer : MonoBehaviour
{
    public void SetStar(int numberStar)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < numberStar; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetLock()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
