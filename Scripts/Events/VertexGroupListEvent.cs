﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GeoTetra.GTBuilder.Events
{
    [System.Serializable]
    public class VertexGroupListEvent : UnityEvent<List<VertexGroup>> {}
}
