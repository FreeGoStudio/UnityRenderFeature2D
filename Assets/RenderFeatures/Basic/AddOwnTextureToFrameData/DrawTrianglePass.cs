using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace FreeGo.RenderFeatures.Basic.AddOwnTextureToFrameData
{
    internal class DrawTrianglePass : ScriptableRenderPass
    {
        private class DrawTrianglePassData
        {
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            using (var builder = renderGraph.AddRasterRenderPass<DrawTrianglePassData>("Draw Triangle Pass", out var passData))
            {
                var customData = frameData.Get<AddOwnTexturePass.CustomData>();
                var customTexture = customData.newTextureForFrameData;
                builder.SetRenderAttachment(customTexture, 0, AccessFlags.Write);
                builder.AllowPassCulling(false);
                builder.SetRenderFunc((DrawTrianglePassData data, RasterGraphContext context) => ExecutePass(data, context));
            }
        }

        private static void ExecutePass(DrawTrianglePassData data, RasterGraphContext context)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0) };
            mesh.triangles = new int[] { 0, 1, 2 };

            context.cmd.DrawMesh(mesh, Matrix4x4.identity, new Material(Shader.Find("Universal Render Pipeline/Unlit")));
        }
    }
}
