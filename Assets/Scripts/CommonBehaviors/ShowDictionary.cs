using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Skills;

public class ShowDictionary : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string , ListHeroSettings> _listHero;
    [SerializeField, SerializeReference] private List< ISkillSettings> _listHeroSettings;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 500, 500), "Dictionary"))
            Show();

    }

    void Show()
    {
        foreach (var item in _listHeroSettings)
        {
            //Debug.Log(item.Key + " " + item.Value);
            //Debug.Log(item.ToString());
            //Debug.Log(item.Level.ToString());
            //Debug.Log(item.Description.ToString());
            //Debug.Log(item.ToString());
        }
    }
}
