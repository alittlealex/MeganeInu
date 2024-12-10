﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;
using UnityEditor;
using UnityEngine.Rendering.RenderGraphModule;

public class OldFilm2_RLPRO : ScriptableRendererFeature
{
    public sealed class maskChannelModeParameter : VolumeParameter<maskChannelMode> { };
    [SerializeField]
    private Shader m_Shader;
    private Material material;
    private OldFilm2_RLPROPass OldFilm2RenderPass;
    public RenderPassEvent Event = RenderPassEvent.AfterRenderingPostProcessing;

    public override void Create()
    {
        m_Shader = Shader.Find("Hidden/Shader/OldFilm2Effect_RLPRO");
        if (m_Shader == null)
        {
            return;
        }
        material = new Material(m_Shader);
        OldFilm2RenderPass = new OldFilm2_RLPROPass(material);

        OldFilm2RenderPass.renderPassEvent = Event;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        OldFilm2 myVolume = VolumeManager.instance.stack?.GetComponent<OldFilm2>();
        if (myVolume == null || !myVolume.IsActive())
            return;
        if (!renderingData.cameraData.postProcessEnabled && myVolume.GlobalPostProcessingSettings.value) return;

        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(OldFilm2RenderPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            Destroy(material);
        }
        else
        {
            DestroyImmediate(material);
        }
#else
                Destroy(material);
#endif
    }
    public class OldFilm2_RLPROPass : ScriptableRenderPass
	{
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
		static readonly int _Mask = Shader.PropertyToID("_Mask");

        private Material material;

        public OldFilm2_RLPROPass(Material material)
        {
            this.material = material;
        }
        private void ParamSwitch(Material mat, bool paramValue, string paramName)
        {
            if (paramValue) mat.EnableKeyword(paramName);
            else mat.DisableKeyword(paramName);
        }

        private static RenderTextureDescriptor GetCopyPassTextureDescriptor(RenderTextureDescriptor desc)
        {
            // Unless 'desc.bindMS = true' for an MSAA texture a resolve pass will be inserted before it is bound for sampling.
            // Since our main pass shader does not expect to sample an MSAA target we will leave 'bindMS = false'.
            // If the camera target has MSAA enabled an MSAA resolve will still happen before our copy-color pass but
            // with this change we will avoid an unnecessary MSAA resolve before our main pass.
            desc.msaaSamples = 1;

            // This avoids copying the depth buffer tied to the current descriptor as the main pass in this example does not use it
            desc.depthBufferBits = (int)DepthBits.None;

            return desc;
        }
        private static void ExecuteCopyColorPass(RasterCommandBuffer cmd, RTHandle sourceTexture)
        {
            Blitter.BlitTexture(cmd, sourceTexture, new Vector4(1, 1, 0, 0), 0.0f, false);
        }
        private static void ExecuteCopyColorPass(CopyPassData data, RasterGraphContext context)
        {
            ExecuteCopyColorPass(context.cmd, data.inputTexture);
        }

        private static void ExecuteMainPass(RasterCommandBuffer cmd, RTHandle sourceTexture, Material material)
        {
            Blitter.BlitTexture(cmd, sourceTexture, new Vector4(1f, 1f, 0f, 0f), material, 0);
        }
        private static void ExecuteMainPass(PassData data, RasterGraphContext context)
        {
            ExecuteMainPass(context.cmd, data.src.IsValid() ? data.src : null, data.material);
        }

        private class PassData
        {
            internal TextureHandle src;
            internal Material material;
        }
        private class CopyPassData
        {
            public TextureHandle inputTexture;
        }
        float T;
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (material == null) return;

            // Use the Volume settings or the default settings if no Volume is set.
            var volumeComponent = VolumeManager.instance.stack.GetComponent<OldFilm2>();


            material.SetFloat("SepiaValue", volumeComponent.SepiaAmount.value);
            material.SetFloat("NoiseValue", volumeComponent.NoiseAmount.value);
            material.SetFloat("ScratchValue", volumeComponent.ScratchAmount.value);
            material.SetFloat("ScratchSize", volumeComponent.ScratchSize.value);
            material.SetFloat("ScratchResolution", volumeComponent.ScratchResolution.value);
            material.SetFloat("_Grain", volumeComponent.Grain.value);
            material.SetFloat("Fade", volumeComponent.Fade.value);
            material.SetFloat("speed", volumeComponent.speed.value);
			if (volumeComponent.mask.value != null)
			{
                material.SetTexture(_Mask, volumeComponent.mask.value);
				material.SetFloat(_FadeMultiplier, 1);
				ParamSwitch(material, volumeComponent.maskChannel.value == maskChannelMode.alphaChannel ? true : false, "ALPHA_CHANNEL");
			}
			else
			{
				material.SetFloat(_FadeMultiplier, 0);
            }


            UniversalResourceData resourcesData = frameData.Get<UniversalResourceData>();
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

            UniversalRenderer renderer = (UniversalRenderer)cameraData.renderer;
            var colorCopyDescriptor = GetCopyPassTextureDescriptor(cameraData.cameraTargetDescriptor);
            TextureHandle copiedColor = TextureHandle.nullHandle;

            copiedColor = UniversalRenderer.CreateRenderGraphTexture(renderGraph, colorCopyDescriptor, "_CustomPostPassColorCopy", false);

            using (var builder = renderGraph.AddRasterRenderPass<CopyPassData>("CustomPostPass_CopyColor", out var passData, profilingSampler))
            {
                passData.inputTexture = resourcesData.activeColorTexture;
                builder.UseTexture(resourcesData.activeColorTexture, AccessFlags.Read);
                builder.SetRenderAttachment(copiedColor, 0, AccessFlags.Write);
                builder.SetRenderFunc((CopyPassData data, RasterGraphContext context) => ExecuteCopyColorPass(data, context));
            }

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("CustomPostPass", out var passData, profilingSampler))
            {
                passData.material = material;

                passData.src = copiedColor;
                builder.UseTexture(copiedColor, AccessFlags.Read);

                builder.SetRenderAttachment(resourcesData.activeColorTexture, 0, AccessFlags.Write);

                // This branch is currently not taken, but if your main pass needed the depth and/or stencil buffer to be bound this is how you would do it
                builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecuteMainPass(data, context));
            }
        }
	}
}