using UnityEngine;

namespace FreeGo.RenderFeatures.GetLight2D
{
    [ExecuteAlways]
    public class LightCamera : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;

        private void Awake()
        {
            if (m_Camera != null)
            {
                //设置相机比例为正方形
                m_Camera.aspect = 1.0f;

            }
        }

        void Update()
        {

        }
    }
}
