#if (UNITY_EDITOR) //Prevent this file from being compiled into your game.

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using SpaceRace;
[CustomEditor(typeof(SceneSwitcher))] //Tell unity that this is an Editor for objects of type colorFader.
public class SceneSwitcherInspector : Editor //We must derive from Editor to use the Editor GUI.
{
    public SceneSwitcher sceneSwitcher;
    public override void OnInspectorGUI() //We are overriding the unity-generated inspector for our colorFader.
    {
        sceneSwitcher = target as SceneSwitcher; //Getting out object. Checking for null is not necessary, unity will not throw an error if it is destroyed.
        EditorGUILayout.BeginVertical(EditorStyles.helpBox); //Begin our layout, with a nice helpbox look.
        sceneSwitcher.switchType = (SceneSwitcher.ChangeType)EditorGUILayout.EnumPopup("Type", sceneSwitcher.switchType);
        if (sceneSwitcher.switchType != SceneSwitcher.ChangeType.None)
        {

            if (sceneSwitcher.switchType == SceneSwitcher.ChangeType.Time)
            {
                sceneSwitcher.switchTime = EditorGUILayout.FloatField("Time in Seconds", sceneSwitcher.switchTime);
                sceneSwitcher.destination = EditorGUILayout.IntField("Destination ID", sceneSwitcher.destination);
            }
            else if (sceneSwitcher.switchType == SceneSwitcher.ChangeType.SceneId || sceneSwitcher.switchType == SceneSwitcher.ChangeType.Time)
            {
                sceneSwitcher.destination = EditorGUILayout.IntField("Destination ID", sceneSwitcher.destination);
            }
        }
        EditorGUILayout.EndVertical(); //End the GUI.
        if (sceneSwitcher.switchType != SceneSwitcher.ChangeType.Time)
        {
            if (sceneSwitcher.gameObject.GetComponent<Button>() == null)
            {
                sceneSwitcher.switchType = SceneSwitcher.ChangeType.None;
            }
        }
    }
}
#endif