using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.AddFrameData
{
    /// <summary>
    /// 自定义渲染特性，添加帧数据
    /// </summary>
    public class AddFrameDataFeature : ScriptableRendererFeature
    {
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        private AddFrameDataRenderPass m_Pass;

        public override void Create()
        {
            m_Pass = new(RenderPassEvent);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_Pass);
        }
    }
}