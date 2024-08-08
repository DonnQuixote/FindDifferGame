#if UNITY_EDITOR
using UnityEditor;
#endif
using BehaviorDesigner.Runtime;

namespace BehaviorDesigner.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeInspector : BehaviorInspector
    {
        // intentionally left blank
    }
#endif
}