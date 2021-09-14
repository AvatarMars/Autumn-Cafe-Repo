using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "new" + nameof(DesiredMealUI_SO), menuName = "Scriptable Objects/" + nameof(DesiredMealUI_SO))]
public class DesiredMealUI_SO : ScriptableObject
{
    public MealType mealType;
    public Sprite mealSprite;

#if UNITY_EDITOR
    //private void OnValidate()
    //{
    //    if (mealType == MealType.None) return;

    //    var path = AssetDatabase.GetAssetPath(GetInstanceID());
    //    var newName = $"{mealType}{Path.GetExtension(path)}";
    //    AssetDatabase.RenameAsset(path, newName);
    //    AssetDatabase.SaveAssets();
    //}
#endif
}
