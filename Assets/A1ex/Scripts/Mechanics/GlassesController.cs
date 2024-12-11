using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class GlassesController : MonoBehaviour
{
    [SerializeField] Transform glasses;
    [SerializeField] Transform onPos;
    [SerializeField] Transform offPos;
    [SerializeField] float moveTime;

    [SerializeField] Volume normalVolume;
    [SerializeField] Volume glassesVolume;
    [SerializeField] Animator animator;


    float timer = 0;

    bool moving = false;

    bool isOn = false;

    private void Awake()
    {
        glasses.localPosition = offPos.localPosition;
        glassesVolume.weight = 0f;
        normalVolume.weight = 1f;
        isOn = false;
        animator.gameObject.SetActive(false);
    }

    [Button]
    public void ToggleGlasses()
    {
        if (moving)
            return;
        Vector3 destination = offPos.localPosition;
        float glassVolumeWeight = 0f;
        float normalVolumeWeight = 1f;
        if(!isOn)
        {
            destination = onPos.localPosition;
            glassVolumeWeight = 1f;
            normalVolumeWeight = 0f;
        }

        glasses.DOKill();
        glasses.DOLocalMove(destination, moveTime).OnStart(OnStart).OnComplete(OnComplete);
        DOTween.To(() => glassesVolume.weight, x => glassesVolume.weight = x, glassVolumeWeight, moveTime);
        DOTween.To(() => normalVolume.weight, x => normalVolume.weight = x, normalVolumeWeight, moveTime);
    }

    void OnStart()
    {
        moving = true; 
        GameManager.Instance.ToggleManager.ToggleAllObjs(moveTime);
        animator.gameObject.SetActive(true);
        if (isOn)
            animator.CrossFade("TakeOff", 0.2f);
        else
            animator.CrossFade("PutOn", 0.2f);
    }

    void OnComplete()
    {
        moving = false; 
        isOn = !isOn; 
        GameManager.Instance.MusicManager.GlassesON = isOn;
        animator.gameObject.SetActive(false);
    }
}
