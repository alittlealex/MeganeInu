using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    InputActionMap playerInputMap;

    InputAction toggleAction;

    private void Awake()
    {
        if (inputActions != null)
        {
            playerInputMap = inputActions.FindActionMap("PlayerMap");
        }

        if (playerInputMap != null)
        {
            toggleAction = playerInputMap.FindAction("Toggle");
            if (toggleAction != null)
            {
                toggleAction.performed += OnToggle;
                toggleAction.Enable();
            }
        }
    }

    void OnToggle(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //TODO: Toggle Glasses
            GameManager.Instance.ToggleManager.ToggleAllObjs();
        }
    }
}
