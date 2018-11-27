using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoTetra.GTBuilder.Gizmo
{
    public static class GizmoMeshUtility
    {
#if UNITY_EDITOR
        public static void MakeVertexSelectionMesh(ref Mesh mesh, List<SeralizedSelectablePoint> selectablePoints)
        {
            Vector3[] vertices = new Vector3[selectablePoints.Count * 4];
            Vector3[] normals = new Vector3[selectablePoints.Count * 4];
            Vector2[] uv1s = new Vector2[selectablePoints.Count * 4];
            Vector2[] uv2s = new Vector2[selectablePoints.Count * 4];
            Color32[] colors = new Color32[selectablePoints.Count * 4];
            int[] triangles = new int[selectablePoints.Count * 6];

            int triangleIndex = 0;
            int vertexIndex = 0;

            Vector3 up = Vector3.up;
            Vector3 right = Vector3.right;

            for (int i = 0; i < selectablePoints.Count; i++)
            {
                vertices[vertexIndex + 0] = selectablePoints[i].GetPosition();
                vertices[vertexIndex + 1] = selectablePoints[i].GetPosition();
                vertices[vertexIndex + 2] = selectablePoints[i].GetPosition();
                vertices[vertexIndex + 3] = selectablePoints[i].GetPosition();

                uv1s[vertexIndex + 0] = Vector3.zero;
                uv1s[vertexIndex + 1] = Vector3.right;
                uv1s[vertexIndex + 2] = Vector3.up;
                uv1s[vertexIndex + 3] = Vector3.one;

                uv2s[vertexIndex + 0] = -up - right;
                uv2s[vertexIndex + 1] = -up + right;
                uv2s[vertexIndex + 2] = up - right;
                uv2s[vertexIndex + 3] = up + right;

                normals[vertexIndex + 0] = Vector3.forward;
                normals[vertexIndex + 1] = Vector3.forward;
                normals[vertexIndex + 2] = Vector3.forward;
                normals[vertexIndex + 3] = Vector3.forward;

                triangles[triangleIndex + 0] = vertexIndex + 0;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + 2;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + 3;
                triangles[triangleIndex + 5] = vertexIndex + 2;

                Color32 color = selectablePoints[i].Selected ? Color.green : selectablePoints[i].PointColor;

                colors[vertexIndex + 0] = color;
                colors[vertexIndex + 1] = color;
                colors[vertexIndex + 2] = color;
                colors[vertexIndex + 3] = color;

                vertexIndex += 4;
                triangleIndex += 6;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv1s;
            mesh.uv2 = uv2s;
            mesh.colors32 = colors;
            mesh.triangles = triangles;
        }
#endif
    }
}