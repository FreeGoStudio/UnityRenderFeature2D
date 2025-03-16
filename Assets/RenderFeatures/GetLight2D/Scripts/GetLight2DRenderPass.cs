using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.CameraOutputToShader
{
    internal class GetLight2DRenderPass : ScriptableRenderPass
    {
        private RTHandle m_OutputCameraRTHandle;

        private const string c_ShaderGloabaPropertyName = "_OutputCameraTexture";
        private static readonly int s_ShaderGlobalPropertyId = Shader.PropertyToID(c_ShaderGloabaPropertyName);
        public GetLight2DRenderPass(RenderPassEvent evt)
        {
            this.renderPassEvent = evt;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData2D = frameData.Get<Universal2DResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            if (cameraData.camera.cameraType != CameraType.Game)
            {
                return;
            }

            var renderTextureDescriptor = cameraData.cameraTargetDescriptor;
            renderTextureDescriptor.depthBufferBits = 0;
            renderTextureDescriptor.msaaSamples = 1;

            RenderingUtils.ReAllocateHandleIfNeeded(ref m_OutputCameraRTHandle, renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: c_ShaderGloabaPropertyName);
            Shader.SetGlobalTexture(s_ShaderGlobalPropertyId, m_OutputCameraRTHandle);

            if (resourceData2D != null)
            {
                for (var i = 0; i < resourceData2D.lightTextures.Length; i++)
                {
                    for (var j = 0; j < resourceData2D.lightTextures[i].Length; j++)
                    {
                        var source = resourceData2D.lightTextures[i][j];
                        var destination = renderGraph.ImportTexture(m_OutputCameraRTHandle);

                        if (!source.IsValid() || !destination.IsValid())
                        {
                            return;
                        }
                        var blitMaterialParameters = new UnityEngine.Rendering.RenderGraphModule.Util.RenderGraphUtils.BlitMaterialParameters(source, destination, Blitter.GetBlitMaterial(TextureDimension.Tex2D), 0);
                        renderGraph.AddBlitPass(blitMaterialParameters, "BlitToRTHandle_CameraOutput");
                    }
                }
            }
        }
    }
}