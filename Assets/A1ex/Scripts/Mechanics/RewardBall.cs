using UnityEngine;

public class RewardBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            transform.gameObject.SetActive(false);
    }
}
