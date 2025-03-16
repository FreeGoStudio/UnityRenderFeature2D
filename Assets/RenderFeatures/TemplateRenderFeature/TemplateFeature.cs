using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.CameraOutputToShader
{
    public class TemplateFeature : ScriptableRendererFeature
    {
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        private TemplateRenderPass m_Pass;

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