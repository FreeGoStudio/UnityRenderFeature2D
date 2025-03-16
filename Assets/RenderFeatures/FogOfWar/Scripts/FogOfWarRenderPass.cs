using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

using static UnityEngine.Rendering.Universal.Internal.DrawObjectsPass;

namespace FreeGo.RenderFeatures.FogOfWar
{
    internal class FogOfWarRenderPass : ScriptableRenderPass
    {
        private class FogOfWarComputePassData
        {
            public BufferHandle output;
        }

        private RTHandle m_OutputCameraRTHandle;

        private const string c_ShaderGloabaPropertyName = "_OutputCameraTexture";
        private static readonly int s_ShaderGlobalPropertyId = Shader.PropertyToID(c_ShaderGloabaPropertyName);
        public FogOfWarRenderPass(RenderPassEvent evt)
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

                        //using (var builder = renderGraph.AddComputePass<FogOfWarComputePassData>("FogOfWar ComputePass", out var data))
                        //{

                        //    // Use ComputeGraphContext instead of RasterGraphContext.
                        //    builder.SetRenderFunc((PassData data, ComputeGraphContext context) => ExecuteComputePass(data, context));

                        //}

                        // 添加Compute Pass
                        //renderGraph.AddComputePass<PassData>("ComputeShaderPass", out var passData)
                        //    .SetRenderFunc((PassData data, ComputeGraphContext ctx) =>
                        //    {
                        //        var cmd = ctx.cmd;
                        //        var computeShader = Resources.Load<ComputeShader>("YourComputeShader");
                        //        int kernelHandle = computeShader.FindKernel("CSMain");


                        //        cmd.SetComputeTextureParam(computeShader, kernelHandle, "Source", source);
                        //        cmd.SetComputeTextureParam(computeShader, kernelHandle, "Destination", destination);



                        //        using (var builder = renderGraph.AddComputePass("MyComputePass", out PassData data))
                        //        {

                        //            // Use ComputeGraphContext instead of RasterGraphContext.
                        //            builder.SetRenderFunc((PassData data, ComputeGraphContext context) => ExecutePass(data, context));

                        //        }
                        //    });
                    }
                }
            }
        }

        private static void ExecuteComputePass(PassData data, ComputeGraphContext context)
        {

        }
    }
}