using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public enum ToggleState
{
    Enabled = 0,
    Disabled
}

public class Togglable : MonoBehaviour
{
    [SerializeField] GameObject togglableObj;
    public ToggleState initToggleState = ToggleState.Enabled;

    ToggleState curToggleState;

    Renderer rend;
    MaterialPropertyBlock prop = null;
    int dissolve;
    Collider collider;

    private void Awake()
    {
        curToggleState = initToggleState;
        rend = togglableObj.GetComponent<Renderer>();
        collider = togglableObj.GetComponent<Collider>();
        dissolve = Shader.PropertyToID("_Dissolve");
        prop = new MaterialPropertyBlock();
        InitToggle();
    }

    void InitToggle()
    {
        switch (curToggleState)
        {
            case ToggleState.Enabled:
                rend.GetPropertyBlock(prop);
                prop.SetFloat(dissolve, 1f);
                rend.SetPropertyBlock(prop);
                //togglableObj.SetActive(true);
                break;
            case ToggleState.Disabled:
                rend.GetPropertyBlock(prop);
                prop.SetFloat(dissolve, 0f);
                rend.SetPropertyBlock(prop);
                //togglableObj.SetActive(false);
                break;
        }
        ExecuteToggle();
    }

    float curDissolve;

    float CurDissolve
    {
        get => curDissolve;
        set
        {
            curDissolve = value;
            rend.GetPropertyBlock(prop);
            prop.SetFloat(dissolve, curDissolve);
            rend.SetPropertyBlock(prop);
        }
    }
    public void Toggle(float time)
    {
        switch (curToggleState)
        {
            case ToggleState.Enabled:
                curToggleState = ToggleState.Disabled;
                DOTween.To(() => CurDissolve, x => CurDissolve = x, 0, time).OnComplete(ExecuteToggle);
                break;
            case ToggleState.Disabled:
                curToggleState = ToggleState.Enabled;
                DOTween.To(() => CurDissolve, x => CurDissolve = x, 1, time).OnComplete(ExecuteToggle);
                break;
        }
        
    }

    void ExecuteToggle()
    {
        switch (curToggleState)
        {
            case ToggleState.Enabled:
                collider.enabled = true;
                break;
            case ToggleState.Disabled:
                collider.enabled = false;
                break;
        }
    }
}
