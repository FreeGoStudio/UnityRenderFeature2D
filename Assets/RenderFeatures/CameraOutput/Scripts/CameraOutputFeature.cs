using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.CameraOutput
{
    public class CameraOutputFeature : ScriptableRendererFeature
    {
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        CameraOutputRenderPass m_CameraOutputPass;

        public override void Create()
        {
            m_CameraOutputPass = new CameraOutputRenderPass(RenderPassEvent);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_CameraOutputPass);
        }
    }
}