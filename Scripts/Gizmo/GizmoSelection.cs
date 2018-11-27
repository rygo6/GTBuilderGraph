using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Policy;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace GeoTetra.GTBuilder.Gizmo
{
    public class GizmoSelection
    {
#if UNITY_EDITOR
        private static GizmoSelection _instance;

        private const int SelectionRange = 64;
        private const string PositionHandleName = "gizmo_selection_position_handle";
        private readonly string[] PositionHandleAxisNames = {"xAxis", "yAxis", "zAxis"};
        private readonly List<SeralizedSelectablePoint> SelectablePoints = new List<SeralizedSelectablePoint>();
        private readonly List<SeralizedSelectablePoint> DraggingPoints = new List<SeralizedSelectablePoint>();
        private Mesh _gizmoMesh;
        private Material _gizmoMaterial;
        private bool _dragging;
        private bool _positionHandleDrag;
        private bool _altDrag;
        private bool _ctrlDrag;
        private bool _shiftDrag;

        public static GizmoSelection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GizmoSelection();
                }

                return _instance;
            }
        }

        public void ClearGizmo()
        {
            if (_gizmoMesh != null)
            {
                _gizmoMesh.Clear();
            }
        }

        public void OnSceneGUI()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    OnMouseDown();
                    break;
                case EventType.MouseUp:
                    OnMouseUp();
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag();
                    break;
            }

            int selectedCount = 0;
            Vector3 averagePosition = new Vector3();
            foreach (SeralizedSelectablePoint point in SelectablePoints)
            {
                if (point.Selected)
                {
                    selectedCount++;
                    averagePosition += point.GetPosition();
                }
            }

            if (selectedCount > 0)
            {
                averagePosition /= selectedCount;
                PositionHandleDrag(averagePosition);
            }
        }

        public void RenderGizmos()
        {
            if (_gizmoMaterial == null)
            {
                _gizmoMaterial = new Material(Shader.Find("Hidden/GT/Vertex"));
                _gizmoMaterial.SetFloat("_Scale", 7);
            }

            if (_gizmoMesh == null)
            {
                _gizmoMesh = new Mesh();
            }

            GizmoMeshUtility.MakeVertexSelectionMesh(
                ref _gizmoMesh,
                SelectablePoints);

            _gizmoMaterial.SetPass(0);
            Graphics.DrawMeshNow(_gizmoMesh, Matrix4x4.identity);
        }

        public void ClearSelectablePoints()
        {
            SelectablePoints.Clear();
        }

        public void AddSelectablePoint(SeralizedSelectablePoint SelectablePoint)
        {
            SelectablePoints.Add(SelectablePoint);
        }

        private void OnMouseDown()
        {
            _dragging = false;
            _altDrag = (EventModifiers.Alt & Event.current.modifiers) == EventModifiers.Alt;
            _ctrlDrag = (EventModifiers.Control & Event.current.modifiers) == EventModifiers.Control;
            _shiftDrag = (EventModifiers.Shift & Event.current.modifiers) == EventModifiers.Shift;
        }

        private void PositionHandleDrag(Vector3 averagePosition)
        {
            GUI.SetNextControlName(PositionHandleName);
            Vector3 newPosition = Handles.PositionHandle(averagePosition, Quaternion.identity);
            Vector3 deltaPosition = newPosition - averagePosition;

            if (deltaPosition.sqrMagnitude > 0)
            {
                _positionHandleDrag = true;
            }

            if (!_altDrag)
            {
                foreach (SeralizedSelectablePoint point in SelectablePoints)
                {
                    if (point.Selected)
                    {
                        point.Translate(deltaPosition);
                    }
                }
            }
        }

        private void OnMouseDrag()
        {
            _dragging = true;
        }

        private void OnMouseUp()
        {
            int nearestSelectablePointIndex = -1;
            float lastDistance = float.MaxValue;

            //modifiers are not manipulating camera
            if (!_altDrag && !_ctrlDrag && !_positionHandleDrag && Event.current.button == 0)
            {
                for (int i = 0; i < SelectablePoints.Count; ++i)
                {
                    float currentDistance = float.MaxValue;
                    //if holding shifting, only select new points
                    if (Event.current.modifiers == EventModifiers.Shift)
                    {
                        if (!SelectablePoints[i].Selected)
                        {
                            currentDistance = SelectablePoints[i].DistanceToPointFromMouse();
                        }
                    }
                    else
                    {
                        currentDistance = SelectablePoints[i].DistanceToPointFromMouse();
                    }

                    if (currentDistance < lastDistance)
                    {
                        lastDistance = currentDistance;
                        nearestSelectablePointIndex = i;
                    }
                }

                if (lastDistance < SelectionRange && nearestSelectablePointIndex != -1)
                {
                    if (Event.current.modifiers == EventModifiers.Shift)
                    {
                        SelectablePoints[nearestSelectablePointIndex].Selected = true;
                    }
                    else
                    {
                        for (int i = 0; i < SelectablePoints.Count; ++i)
                        {
                            if (i == nearestSelectablePointIndex)
                            {
                                SelectablePoints[i].Selected = true;
                            }
                            else
                            {
                                SelectablePoints[i].Selected = false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SelectablePoints.Count; ++i)
                    {
                        SelectablePoints[i].Selected = false;
                    }
                }
            }

            SceneView.RepaintAll();
            _altDrag = false;
            _ctrlDrag = false;
            _shiftDrag = false;
            _positionHandleDrag = false;
            _dragging = false;
        }

        private bool DraggingPositionHandleAxis()
        {
            return ArrayUtility.Contains(PositionHandleAxisNames, GUI.GetNameOfFocusedControl());
        }
#endif
    }
}