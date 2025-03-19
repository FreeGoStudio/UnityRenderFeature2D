using UnityEngine;
using UnityEngine.UI;

namespace FreeGo
{
    [ExecuteAlways]
    public class FogOfWarManager : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private ComputeShader m_AddComputeShader;
        [SerializeField] private RawImage m_FogOfWarMask;

        private Texture m_LightTexture;
        private RenderTexture m_FogOfWarMaskRenderTexture;
        private int m_CSMainKernelID;

        private int m_PropertyId;

        private void Awake()
        {
            if (m_Camera != null)
            {
                //设置相机比例为正方形
                m_Camera.aspect = 1.0f;
            }
        }

        void Start()
        {
            string propertyName = "_OutputCameraTexture";
            m_PropertyId = Shader.PropertyToID(propertyName);

            m_FogOfWarMaskRenderTexture = new RenderTexture(128, 128, 0);
            m_FogOfWarMaskRenderTexture.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SNorm;
            m_FogOfWarMaskRenderTexture.depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.None;
            m_FogOfWarMaskRenderTexture.filterMode = FilterMode.Bilinear;
            m_FogOfWarMaskRenderTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            m_FogOfWarMaskRenderTexture.enableRandomWrite = true;

            m_FogOfWarMaskRenderTexture.Create();

            m_FogOfWarMask.gameObject.SetActive(true);
            m_FogOfWarMask.texture = m_FogOfWarMaskRenderTexture;
            m_CSMainKernelID = m_AddComputeShader.FindKernel("CSMain");
            m_AddComputeShader.SetTexture(m_CSMainKernelID, "Result", m_FogOfWarMaskRenderTexture);

        }

        void Update()
        {
            m_LightTexture = Shader.GetGlobalTexture(m_PropertyId);
            m_AddComputeShader.SetTexture(m_CSMainKernelID, "Input", m_LightTexture);

            m_AddComputeShader.Dispatch(m_CSMainKernelID, m_LightTexture.width / 8, m_LightTexture.height / 8, 1);
        }
    }
}
