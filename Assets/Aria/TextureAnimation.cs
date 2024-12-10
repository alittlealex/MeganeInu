using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    [Header("Frame Animation Settings")]
    public Texture2D[] frames;           // 存放所有帧的贴图
    public float frameRate = 12f;        // 帧率

    [Header("Texture Transform Settings")]
    public Vector2 textureScale = new Vector2(1, 1);    // 贴图平铺
    public Vector2 textureOffset = new Vector2(0, 0);   // 贴图偏移

    private Material material;
    private float frameTimer;
    private int currentFrame;

    void Start()
    {
        // 获取材质并设置初始变换
        material = GetComponent<Renderer>().material;
        UpdateTextureTransform();
    }

    void Update()
    {
        // 更新帧动画
        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            material.mainTexture = frames[currentFrame];
        }

        // 实时更新贴图变换（如果需要在运行时通过Inspector调整）
        UpdateTextureTransform();
    }

    // 更新贴图变换
    void UpdateTextureTransform()
    {
        material.mainTextureScale = textureScale;
        material.mainTextureOffset = textureOffset;
    }

    // 提供方法供其他脚本调用
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

    // 提供帧动画控制方法
    public void SetFrameRate(float rate)
    {
        frameRate = rate;
    }
}