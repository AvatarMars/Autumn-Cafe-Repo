#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DesiredMealUI_SO))]
public class DesiredMealUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Please, ensure that there's only one DesiredMealUI_SO with this Meal Type, and that the file has the same name as the Meal Type.", MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif
