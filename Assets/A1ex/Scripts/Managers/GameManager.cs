using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField] InputManager inputManager;
    public InputManager InputManager { get => inputManager; }

    [SerializeField] ToggleManager toggleManager;
    public ToggleManager ToggleManager { get => toggleManager; }
}
