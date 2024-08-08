using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using BehaviorDesigner.Runtime;

namespace BehaviorDesigner.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ExternalBehaviorTree))]
    public class ExternalBehaviorTreeInspector : ExternalBehaviorInspector
    {
        // intentionally left blank
    }
#endif
}
