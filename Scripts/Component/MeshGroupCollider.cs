using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoTetra.GTBuilder
{
    public class MeshGroupCollider : MonoBehaviour
    {
        [SerializeField]
        private PhysicMaterial _physicMaterial;

        [HideInInspector]
        [SerializeField]
        private MeshGroup _meshGroup;

        [HideInInspector]
        [SerializeField]
        private List<MeshCollider> _meshColliders = new List<MeshCollider>();

        private List<MeshCollider> _destroyMeshColliders = new List<MeshCollider>();

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
            UpdateAndGenerateMeshes();
        }

        private void OnDrawGizmos()
        {
            //done because calling Destroy from Editor gives error
            if (_destroyMeshColliders.Count > 0)
            {
                for (int i = 0; i < _destroyMeshColliders.Count; ++i)
                {
                    DestroyImmediate(_destroyMeshColliders[i].gameObject);
                }
                _destroyMeshColliders.Clear();
            }
        }

        private void UpdateAndGenerateMeshes()
        {
            for (int i = 0; i < _meshGroup.Meshes.Count; ++i)
            {
                MeshCollider meshCollider = null;
                if (i < _meshColliders.Count)
                {
                    meshCollider = _meshColliders[i];
                }
                else
                {
                    GameObject newGameObject = new GameObject("Mesh" + i);
                    meshCollider = newGameObject.AddComponent<MeshCollider>();
                    newGameObject.layer = gameObject.layer;
                    newGameObject.tag = gameObject.tag;
                    newGameObject.transform.parent = transform;
                    newGameObject.hideFlags = HideFlags.NotEditable;
                    _meshColliders.Add(meshCollider);
                }
                _meshColliders[i].sharedMaterial = _physicMaterial;
                _meshColliders[i].sharedMesh = _meshGroup.Meshes[i];
            }

            for (int i = _meshGroup.Meshes.Count; i < _meshColliders.Count; ++i)
            {
                _destroyMeshColliders.Add(_meshColliders[i]);
                _meshColliders[i] = null;
            }

            _meshColliders.RemoveAll(mf => mf == null);
        }

        [ContextMenu("Regenerate Meshes")]
        public void RegenerateMeshes()
        {
            for (int i = transform.childCount - 1; i > -1; --i)
            {
                transform.GetChild(i).gameObject.hideFlags = HideFlags.None;
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            _meshColliders.Clear();
            UpdateAndGenerateMeshes();
        }
    }
}