using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.AddFrameData
{
    /// <summary>
    /// 自定义渲染通道，添加帧数据
    /// </summary>
    internal class AddFrameDataRenderPass : ScriptableRenderPass
    {
        /// <summary>
        /// 自定义帧数据类型
        /// </summary>
        public class MyCustomData : ContextItem
        {
            public TextureHandle TextureToTransfer;

            public override void Reset()
            {
                TextureToTransfer = TextureHandle.nullHandle;
            }
        }

        public AddFrameDataRenderPass(RenderPassEvent evt)
        {
            this.renderPassEvent = evt;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            //在帧数据中创建一个自定义数据类型
            var customData = frameData.Create<MyCustomData>();
            //创建一个匹配屏幕大小的纹理属性
            RenderTextureDescriptor textureProperties = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0);
            //创建纹理
            TextureHandle texture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, textureProperties, "CopyTexture", false);
            customData.TextureToTransfer = texture;
        }
    }
}