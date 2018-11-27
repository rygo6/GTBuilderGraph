using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GeoTetra.GTBuilder.Graph
{
    [System.Serializable]
    public class UnityEventLink
    {
        [SerializeField] string _startFieldName;
        [SerializeField] Rect _startRect;
        [SerializeField] List<object> _targetObjectList = new List<object>();
        [SerializeField] List<string> _methodNameList = new List<string>();

        public string StartFieldName { get { return _startFieldName; } set { _startFieldName = value; } }
        public Rect StartRect { get { return _startRect; } set { _startRect = value; } }
        public List<object> TargetObjectList { get { return _targetObjectList; } }
        public List<string> MethodNameList { get { return _methodNameList; } }
    }
}