#if (UNITY_EDITOR)

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(objectToInspect))]
public class CustomInspector : Editor
{

    // Variables Section
    public GameObject myTarget;
    public override void OnInspectorGUI()
    {
        myTarget = target as GameObject;
        //base.DrawDefaultInspector(); // To add the defaults back
        GUILayout.BeginVertical(EditorStyles.helpBox);


        GUILayout.EndVertical();

    }
}
#endif
