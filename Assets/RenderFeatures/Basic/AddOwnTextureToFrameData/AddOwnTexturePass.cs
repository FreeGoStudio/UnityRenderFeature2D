using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.Basic.AddOwnTextureToFrameData
{
    internal class AddOwnTexturePass : ScriptableRenderPass
    {
        private class AddOwnTexturePassData
        {
            internal TextureHandle copySourceTexture;
        }

        public class CustomData : ContextItem
        {
            public TextureHandle newTextureForFrameData;
            public override void Reset()
            {
                newTextureForFrameData = TextureHandle.nullHandle;
            }
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            using (var builder = renderGraph.AddRasterRenderPass<AddOwnTexturePassData>("AddOwnTextureToFrameData", out var passData))
            {
                //创建纹理配置
                RenderTextureDescriptor desc = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0);
                TextureHandle texture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "AddOwnTextureToFrameDataTexture", false);
                CustomData customData = frameData.Create<CustomData>();
                builder.SetRenderAttachment(texture, 0, AccessFlags.Write);
                builder.AllowPassCulling(false);
                builder.SetRenderFunc((AddOwnTexturePassData data, RasterGraphContext context) => ExecutePass(data, context));
            }
        }

        private static void ExecutePass(AddOwnTexturePassData data, RasterGraphContext context)
        {
            context.cmd.ClearRenderTarget(true, true, Color.yellow);
        }
    }
}
