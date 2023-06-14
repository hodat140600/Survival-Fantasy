using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewardSettings
{
    void Active(GameObject gameObject);
    string GetName();
}
