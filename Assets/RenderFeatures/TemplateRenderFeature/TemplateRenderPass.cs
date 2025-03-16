using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

using static UnityEngine.Rendering.Universal.Internal.DrawObjectsPass;

namespace FreeGo.RenderFeatures.CameraOutputToShader
{
    internal class TemplateRenderPass : ScriptableRenderPass
    {
        private class TemplatePassData
        {

        }

        public TemplateRenderPass(RenderPassEvent evt)
        {
            this.renderPassEvent = evt;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            using (var builder = renderGraph.AddRasterRenderPass<TemplatePassData>("TemplateRenderPass", out var passData))
            {
                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

                builder.SetRenderAttachment(resourceData.activeColorTexture, 0);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
            }
        }

        private static void ExecutePass(PassData data, RasterGraphContext context)
        {
        }
    }
}