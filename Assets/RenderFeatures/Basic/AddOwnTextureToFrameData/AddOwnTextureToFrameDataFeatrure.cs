using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.Basic.AddOwnTextureToFrameData
{
    public class AddOwnTextureToFrameDataFeatrure : ScriptableRendererFeature
    {
        private AddOwnTexturePass m_AddOwnTexturePass;
        private DrawTrianglePass m_DrawTrianglePass;

        public override void Create()
        {
            m_AddOwnTexturePass = new AddOwnTexturePass();
            m_DrawTrianglePass = new DrawTrianglePass();

            m_AddOwnTexturePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
            m_DrawTrianglePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_AddOwnTexturePass);
            renderer.EnqueuePass(m_DrawTrianglePass);
        }
    }
}
