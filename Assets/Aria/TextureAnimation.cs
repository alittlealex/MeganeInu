using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    [Header("Frame Animation Settings")]
    public Texture2D[] frames;           // �������֡����ͼ
    public float frameRate = 12f;        // ֡��

    [Header("Texture Transform Settings")]
    public Vector2 textureScale = new Vector2(1, 1);    // ��ͼƽ��
    public Vector2 textureOffset = new Vector2(0, 0);   // ��ͼƫ��

    private Material material;
    private float frameTimer;
    private int currentFrame;

    void Start()
    {
        // ��ȡ���ʲ����ó�ʼ�任
        material = GetComponent<Renderer>().material;
        UpdateTextureTransform();
    }

    void Update()
    {
        // ����֡����
        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            material.mainTexture = frames[currentFrame];
        }

        // ʵʱ������ͼ�任�������Ҫ������ʱͨ��Inspector������
        UpdateTextureTransform();
    }

    // ������ͼ�任
    void UpdateTextureTransform()
    {
        material.mainTextureScale = textureScale;
        material.mainTextureOffset = textureOffset;
    }

    // �ṩ�����������ű�����
    public void SetTextureScale(Vector2 scale)
    {
        textureScale = scale;
        UpdateTextureTransform();
    }

    public void SetTextureOffset(Vector2 offset)
    {
        textureOffset = offset;
        UpdateTextureTransform();
    }

    // �ṩ֡�������Ʒ���
    public void SetFrameRate(float rate)
    {
        frameRate = rate;
    }
}