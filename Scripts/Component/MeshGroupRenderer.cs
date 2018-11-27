using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoTetra.GTBuilder
{
    public class MeshGroupRenderer : MonoBehaviour
    {
        [SerializeField]
        private Material _material;

        [SerializeField]
        private MeshGroup _meshGroup;

        [SerializeField]
        private List<MeshFilter> _meshFilters = new List<MeshFilter>();

        private List<MeshFilter> _destroyMeshFilters = new List<MeshFilter>();

        public MeshGroup MeshGroup
        {
            get
            {
                return _meshGroup;
            }
            set
            {
                _meshGroup = value;
                UpdateAndGenerateMeshes();
            }
        }

        private void OnValidate()
        {
//            UpdateAndGenerateMeshes();
        }

        private void OnDrawGizmos()
        {
            //done because calling Destroy from Editor gives error
            if (_destroyMeshFilters.Count > 0)
            {
                for (int i = 0; i < _destroyMeshFilters.Count; ++i)
                {
                    DestroyImmediate(_destroyMeshFilters[i].gameObject);
                }
                _destroyMeshFilters.Clear();
            }
        }

        private void UpdateAndGenerateMeshes()
        {
            for (int i = 0; i < _meshGroup.Meshes.Count; ++i)
            {
                MeshFilter meshFilter = null;
                if (i < _meshFilters.Count)
                {
                    meshFilter = _meshFilters[i];
                }
                else
                {
                    GameObject newGameObject = new GameObject("Mesh" + i);
                    meshFilter = newGameObject.AddComponent<MeshFilter>();
                    newGameObject.AddComponent<MeshRenderer>();
                    newGameObject.transform.parent = transform;
                    newGameObject.hideFlags = HideFlags.NotEditable;
                    _meshFilters.Add(meshFilter);
                }
                _meshFilters[i].gameObject.layer = gameObject.layer;
                _meshFilters[i].gameObject.tag = gameObject.tag;

#if UNITY_EDITOR
                UnityEditor.GameObjectUtility.SetStaticEditorFlags(_meshFilters[i].gameObject, UnityEditor.GameObjectUtility.GetStaticEditorFlags(gameObject));
 #endif

                _meshFilters[i].GetComponent<MeshRenderer>().material = _material;
                _meshFilters[i].sharedMesh = _meshGroup.Meshes[i];
            }

            for (int i = _meshGroup.Meshes.Count; i < _meshFilters.Count; ++i)
            {
                _destroyMeshFilters.Add(_meshFilters[i]);
                _meshFilters[i] = null;
            }

            _meshFilters.RemoveAll(mf => mf == null);
        }

        [ContextMenu("Regenerate Meshes")]
        public void RegenerateMeshes()
        {
            for (int i = transform.childCount - 1; i > -1; --i)
            {
                transform.GetChild(i).gameObject.hideFlags = HideFlags.None;
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            _meshFilters.Clear();
            UpdateAndGenerateMeshes();
        }
    }
}