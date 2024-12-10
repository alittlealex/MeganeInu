using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GlassesController glassesController;
    public GlassesController GlassesController { get => glassesController; }
}
