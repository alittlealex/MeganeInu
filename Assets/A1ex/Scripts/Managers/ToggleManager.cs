using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class ToggleManager : MonoBehaviour
{
    [SerializeField][ReadOnly] List<Togglable> togglableList;

    [Button]
    public void UpdateLevelTogglableList()
    {
        togglableList.Clear();
        togglableList.AddRange(GameObject.FindObjectsByType<Togglable>(FindObjectsSortMode.None));
    }

    public void ToggleAllObjs(float time)
    {
        foreach(Togglable t in togglableList)
        {
            t.Toggle(time);
        }
    }
}
