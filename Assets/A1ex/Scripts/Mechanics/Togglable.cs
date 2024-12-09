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

    private void Awake()
    {
        curToggleState = initToggleState;
        ExcuteToggle();
    }

    void ExcuteToggle()
    {
        switch (curToggleState)
        {
            case ToggleState.Enabled:
                togglableObj.SetActive(true);
                break;
            case ToggleState.Disabled:
                togglableObj.SetActive(false);
                break;
        }
    }

    public void Toggle()
    {
        switch (curToggleState)
        {
            case ToggleState.Enabled:
                curToggleState = ToggleState.Disabled;
                break;
            case ToggleState.Disabled:
                curToggleState = ToggleState.Enabled;
                break;
        }
        ExcuteToggle();
    }
}
