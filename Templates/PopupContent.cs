#if (UNITY_EDITOR)

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
{

    public class PopupContent : PopupWindowContent
    {

        public PopupContent() // Unlike many of Unity editor tools, we actually get a usable constructor for the popup.
        {


        }
        public override void OnGUI(Rect size) // Popup Content REQUOIRES a Rect to be used.
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.EndVertical();
        }
        public override void OnOpen()
        {

        }
    }
}
#endif
