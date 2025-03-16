using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.AddFrameData
{
    /// <summary>
    /// �Զ�����Ⱦ���ԣ����֡����
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