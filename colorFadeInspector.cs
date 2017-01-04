#if (UNITY_EDITOR) //Prevent this file from being compiled into your game.

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(colorFader))] //Tell unity that this is an Editor for objects of type colorFader.
public class colorFadeInspector : Editor //We must derive from Editor to use the Editor GUI.
{
    public override void OnInspectorGUI() //We are overriding the unity-generated inspector for our colorFader.
    {
        colorFader fader = target as colorFader; //Getting out object. Checking for null is not necessary, unity will not throw an error if it is destroyed.
        GUILayout.BeginVertical(EditorStyles.helpBox); //Begin our layout, with a nice helpbox look.
        if (fader.ColorType == colorFader.colorType.Unselected) //The default state of the fader.
        {
            fader.ColorType = (colorFader.colorType)EditorGUILayout.EnumPopup("Source Type", fader.ColorType); //Display our enums to choose from.
        }
        if (fader.ColorType == colorFader.colorType.Image) //Check if the chosen Enum is Image.
        {
            fader.ColorType = (colorFader.colorType)EditorGUILayout.EnumPopup("Source Type", fader.ColorType);  //Display our enums to choose from.
            fader.objectToColor = fader.gameObject.GetComponent<Image>(); //Let the unity API try to find the Image.
            fader.objectToColor = EditorGUILayout.ObjectField("Source", (Object)fader.objectToColor, typeof(Image), true); //Display an object field to select the Image object.
        }
        if (fader.ColorType == colorFader.colorType.RawImage) //Check if the chosen Enum is RawImage.
        {
            fader.ColorType = (colorFader.colorType)EditorGUILayout.EnumPopup("Source Type", fader.ColorType); //Display our enums to choose from.
            fader.objectToColor = fader.gameObject.GetComponent<RawImage>(); //Let the unity API try to find the RawImage.
            fader.objectToColor = EditorGUILayout.ObjectField("Source", (Object)fader.objectToColor, typeof(RawImage), true); //Display an object field to select the RawImage object.
        }
        if (fader.ColorType == colorFader.colorType.Sprite) //Check if the chosen Enum is Sprite.
        {
            fader.ColorType = (colorFader.colorType)EditorGUILayout.EnumPopup("Source Type", fader.ColorType); //Display our enums to choose from.
            fader.objectToColor = fader.gameObject.GetComponent<SpriteRenderer>(); //Let the unity API try to find the Sprite.
            fader.objectToColor = EditorGUILayout.ObjectField("Source", (Object)fader.objectToColor, typeof(SpriteRenderer), true); //Display an object field to select the Sprite object.
        }
        if (fader.ColorType == colorFader.colorType.Text) //Check if the chosen Enum is Text.
        {
            fader.ColorType = (colorFader.colorType)EditorGUILayout.EnumPopup("Source Type", fader.ColorType); //Display our enums to choose from.
            fader.objectToColor = fader.gameObject.GetComponent<Text>(); //Let the unity API try to find the Text.
            fader.objectToColor = EditorGUILayout.ObjectField("Source", (Object)fader.objectToColor, typeof(Text), true); //Display an object field to select the Text object.
        }
        if (fader.objectToColor != null && fader.ColorType != colorFader.colorType.Unselected) //Check to make sure nothing is null or unselected.
        {
            fader.FadeToColor = EditorGUILayout.ColorField("Fade Color", fader.FadeToColor); //Display a color field for your fade color.
            fader.fadeTime = EditorGUILayout.FloatField("Fade Time", fader.fadeTime); //Display a float field for your fade time.
            fader.destroyOnFade = EditorGUILayout.Toggle("Destroy On Fade?", fader.destroyOnFade); //Display a bool for whether or not it gets destroyed on fadeTime being met.
            if (!fader.destroyOnFade) //We can't destroy AND loop, so if you chose destroy, loop disappers.
                fader.willLoop = EditorGUILayout.Toggle("Loop?", fader.willLoop); //Display the bool for whether or not to loop the object color fade.
        }
        GUILayout.EndVertical(); //End the GUI.
    }
}
#endif