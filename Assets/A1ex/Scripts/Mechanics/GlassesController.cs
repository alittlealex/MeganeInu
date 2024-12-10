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


    float timer = 0;

    bool moving = false;

    bool isOn = false;

    private void Awake()
    {
        glasses.localPosition = offPos.localPosition;
        glassesVolume.weight = 0f;
        normalVolume.weight = 1f;
        isOn = false;
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
        glasses.DOLocalMove(destination, moveTime).OnStart(() => { moving = true; GameManager.Instance.ToggleManager.ToggleAllObjs(moveTime); } ).OnComplete(() => { moving = false; isOn = !isOn; GameManager.Instance.MusicManager.GlassesON = isOn; });
        DOTween.To(() => glassesVolume.weight, x => glassesVolume.weight = x, glassVolumeWeight, moveTime);
        DOTween.To(() => normalVolume.weight, x => normalVolume.weight = x, normalVolumeWeight, moveTime);
    }
}
