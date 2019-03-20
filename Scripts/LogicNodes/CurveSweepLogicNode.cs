using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Events;
using GeoTetra.Common.Extensions;
using GeoTetra.GTCommon;
using GeoTetra.GTLogicGraph;

namespace GeoTetra.GTBuilder
{
    public class CurveSweepLogicNode : LogicNode
    {
        [SerializeField] private bool _flipNormals;

        [SerializeField] private Center _center;

        [SerializeField] private Bool2 _compressUV;

        [SerializeField] private Bool2 _compressUV2;

        public event Action<MeshGroup> MeshGroupOutput;
        
        [LogicNodePort] public event Action<Mesh> MeshOutput;

        private List<Vertex> _railPoints;

        private List<VertexGroup> _pathVertexGroups;

        private Vector3 _railCenter;

        private MeshGroup _meshGroup = new MeshGroup();

        public void FlipNormals(bool value)
        {
            _flipNormals = value;
        }

        public void Center(Center value)
        {
            _center = value;
        }

        public void CompressUv(Bool2 value)
        {
            _compressUV = value;
        }

        public void CompressUv2(Bool2 value)
        {
            _compressUV2 = value;
        }

        [LogicNodePort]
        public void RailVerticesInput(List<Vertex> vertexList)
        {
            _railPoints = vertexList;
            if (_center == GTBuilder.Center.Local)
            {
                _railCenter = VerticesCenter(_railPoints);
            }
            else if (_center == GTBuilder.Center.Origin)
            {
                _railCenter = Vector3.zero;
            }

            GenerateMesh();
        }

        [LogicNodePort]
        public void PathVertexGroupsInput(List<VertexGroup> groupList)
        {
            _pathVertexGroups = groupList;
            GenerateMesh();
        }

        [LogicNodePort]
        public void PathVerticesInput(List<Vertex> vertexList)
        {
            //if setting PathPoints directly override PathVertexGroups
            _pathVertexGroups = new List<VertexGroup>();
            _pathVertexGroups.Add(new VertexGroup(vertexList));
            GenerateMesh();
        }

        private Vector3 VerticesCenter(List<Vertex> vertices)
        {
            Vector3 average = Vector3.zero;
            for (int i = 0; i < vertices.Count; ++i)
            {
                average += vertices[i].Position;
            }

            average /= vertices.Count;
            return average;
        }

        private void GenerateMesh()
        {
            if (_railPoints == null)
                return;
            
            if (_pathVertexGroups != null)
            {
                _meshGroup.Clear();
                foreach (VertexGroup group in _pathVertexGroups)
                {
                    GenerateMesh(group);
                }

                MeshGroupOutput?.Invoke(_meshGroup);
                MeshOutput?.Invoke(_meshGroup.Meshes[0]);
            }
        }

        private void GenerateMesh(VertexGroup vertexGroup)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            Vector2 uv = new Vector2();
            for (int p = 0; p < vertexGroup.Vertices.Count; ++p)
            {
                uv.y = 0;
                if (p > 0)
                {
                    uv.x += Vector3.Distance(vertexGroup.Vertices[p - 1].Position, vertexGroup.Vertices[p].Position);
                }

                Quaternion lookRotation =
                    Quaternion.LookRotation(vertexGroup.Vertices[p].Tangent, vertexGroup.Vertices[p].Normal);
                Vector3 centerOffset = vertexGroup.Vertices[p].Position - _railCenter;
                for (int r = 0; r < _railPoints.Count; ++r)
                {
                    Vector3 offsetRail = _railPoints[r].Position + centerOffset;
                    Vector3 point = offsetRail.RotateAroundPivot(vertexGroup.Vertices[p].Position, lookRotation);
                    vertices.Add(point);

                    if (r > 0)
                    {
                        uv.y += Vector3.Distance(_railPoints[r - 1].Position, _railPoints[r].Position);
                    }

                    uvs.Add(uv);

                    if (r > 0 && r < _railPoints.Count - 1)
                    {
                        //add double for hard edges
                        uvs.Add(uv);
                        vertices.Add(point);
                    }
                }
            }

            //if (_compressU || _compressV)
            //{
            //    Vector2 maxUV = uvs[uvs.Count - 1];
            //    Vector2 ratioUV = new Vector2(1f / maxUV.x, 1f / maxUV.y);
            //    for (int i = 0; i < uvs.Count; ++i)
            //    {
            //        uv = uvs[i];
            //        if (_compressU)
            //        {
            //            uv.x *= ratioUV.x;
            //        }
            //        if (_compressV)
            //        {
            //            uv.y *= ratioUV.y;
            //        }
            //        uvs[i] = uv;
            //    }
            //}

            //due to hard edges, twice as many points, minus 2 because first and last isn't doubled
            int railPointsCount = _railPoints.Count * 2 - 2;

            List<int> triangles = new List<int>();
            int railStart = 0;
            int secondRailStart = railPointsCount;

            for (int p = 0; p < vertexGroup.Vertices.Count - 1; ++p)
            {
                for (int r = 0; r < railPointsCount - 1; ++r)
                {
                    //skip odd for hard edges
                    if (r % 2 == 0)
                    {
                        if (_flipNormals)
                        {
                            triangles.Add(railStart + r);
                            triangles.Add(secondRailStart + r);
                            triangles.Add(railStart + r + 1);

                            triangles.Add(railStart + r + 1);
                            triangles.Add(secondRailStart + r);
                            triangles.Add(secondRailStart + r + 1);
                        }
                        else
                        {
                            triangles.Add(railStart + r);
                            triangles.Add(railStart + r + 1);
                            triangles.Add(secondRailStart + r);

                            triangles.Add(railStart + r + 1);
                            triangles.Add(secondRailStart + r + 1);
                            triangles.Add(secondRailStart + r);
                        }
                    }
                }

                railStart = secondRailStart;
                secondRailStart += railPointsCount;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.uv = CompressUVToArray(_compressUV, uvs);
            mesh.uv2 = CompressUVToArray(_compressUV2, uvs);
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            _meshGroup.AddMesh(mesh);
        }

        private Vector2[] CompressUVToArray(Bool2 compressUV, List<Vector2> uvList)
        {
            Vector2[] uvArray = uvList.ToArray();
            if (compressUV.X || compressUV.Y)
            {
                Vector2 uv = new Vector2();
                Vector2 maxUV = uvList[uvList.Count - 1];
                Vector2 ratioUV = new Vector2(1f / maxUV.x, 1f / maxUV.y);
                for (int i = 0; i < uvArray.Length; ++i)
                {
                    uv = uvArray[i];
                    if (compressUV.X)
                    {
                        uv.x *= ratioUV.x;
                    }

                    if (compressUV.Y)
                    {
                        uv.y *= ratioUV.y;
                    }

                    uvArray[i] = uv;
                }
            }

            return uvArray;
        }

        public void OnDrawGizmos()
        {
            //            Gizmos.color = Color.gray;
            //            if (_pathPoints != null)
            //            {
            //                for (int i = 0; i < _pathPoints.Count - 1; ++i)
            //                {
            //                    Quaternion lookRotation = Quaternion.LookRotation(_pathPoints[i].Tangent, _pathPoints[i].Normal);
            //                    Vector3 centerOffset = _pathPoints[i].Position - _railCenter;
            //                    for (int j = 0; j < _railPoints.Count - 1; ++j)
            //                    {
            //                        Vector3 offsetRail = _railPoints[j].Position + centerOffset;
            //                        Vector3 offsetRail1 = _railPoints[j + 1].Position + centerOffset;
            //                        Gizmos.DrawLine(offsetRail.RotateAroundPivot(_pathPoints[i].Position, lookRotation),
            //                            offsetRail1.RotateAroundPivot(_pathPoints[i].Position, lookRotation));
            //                    }
            //                }
            //            }
        }
    }
}