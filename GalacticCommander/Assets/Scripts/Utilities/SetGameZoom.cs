using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
 
public class SetGameZoom
{
    [PostProcessSceneAttribute(2)]
    public static void OnPostprocessScene()
    {
        SetSize();
    }
 
    public static void SetSize()
    {
        float targetScale = 0.29f;
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        var areaField = gvWndType.GetField("m_ZoomArea", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var areaObj = areaField.GetValue(gvWnd);
        var scaleField = areaObj.GetType().GetField("m_Scale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        scaleField.SetValue(areaObj, new Vector2(targetScale, targetScale));
    }
}