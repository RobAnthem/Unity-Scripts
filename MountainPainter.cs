using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace Tools.MountainPainter
{
    [Serializable]
    public class MountainPainter : EditorWindow
    {
        #region Data Region
        Rect contextRect = new Rect(80, 5, 432, 412);
        Texture2D maskTexture;
        Texture2D newTexture;
        Color[] alternatingColor;
        Texture2D brush1Tex;
        Texture2D brushTex;
        public Dictionary<string, string> heightmapLib;
        Texture2D cursorTex;
        public activeVrush selectedBrush;
        public enum activeVrush { None = 0, First = 1, Groups = 3, Prefabs = 4, Painter = 5 }
        public Texture2D windowHeader;
        #endregion
        [MenuItem("Tools/VOID.Tools/Open Mountains")]
        public static void ShowWindow()
        {
            EditorWindow mPainter = EditorWindow.GetWindow(typeof(MountainPainter));
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/VOIDTools/voidpad_icon.png");
            GUIContent titlecontent = new GUIContent("VOIDpainter", icon);
            mPainter.titleContent = titlecontent;
            mPainter.maxSize = new Vector2(580, 600);
            mPainter.minSize = new Vector2(580, 600);

        }
        void OnGUI()
        {
            EditorGUI.DrawRect(contextRect, Color.black);
            Event currentEvent = Event.current;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Box(brush1Tex);
            Rect Brush1Rect = new Rect(5, 15, 50, 50);
            if (selectedBrush == activeVrush.First)
                EditorGUI.DrawRect(Brush1Rect, Color.grey);

            else
                EditorGUI.DrawRect(Brush1Rect, Color.clear);

            GUILayout.Box(brush1Tex);
            GUILayout.Box(brush1Tex);
            GUILayout.Box(brush1Tex);
            if (GUILayout.Button("Save", GUILayout.Width(64)))
            {
                byte[] bytes = newTexture.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath + "/../Assets/VOIDanizer/Heightmaps/Heightmap_"  + ".png", bytes);
            }
            EditorGUILayout.EndVertical();

            GUILayout.Box(maskTexture);
            Cursor.SetCursor(cursorTex, new Vector2(0, 0), CursorMode.Auto);
            EditorGUIUtility.AddCursorRect(new Rect(80, 65, 382, 382), MouseCursor.CustomCursor);

            if (currentEvent.type == EventType.MouseDown)
            {
                Vector2 mousePos = currentEvent.mousePosition;
                if (Brush1Rect.Contains(mousePos))
                {
                    brushTex = AssetDatabase.LoadAssetAtPath("Assets/Tree Pack/Mountain_1.png", typeof(Texture2D)) as Texture2D;
                    selectedBrush = activeVrush.First;
                    brushTex.alphaIsTransparency = true;
                }
            }
            if (currentEvent.type == EventType.ContextClick)
            {
                Vector2 mousePos = currentEvent.mousePosition;
                if (contextRect.Contains(mousePos))
                {
                    if (selectedBrush != activeVrush.None)
                    {
                        EditorUtility.DisplayProgressBar("Painting", "Getting Pixels", 0);
                        Color[] testX = brushTex.GetPixels();
                        newTexture.SetPixels(((int)mousePos.x - 70) % 512, 512 - (int)mousePos.y - 84, 64, 64, testX);
                        float switcher = 0.90f;
                        Color[] overLay = maskTexture.GetPixels(((int)mousePos.x - 70) % 512, 512 - (int)mousePos.y - 84, 64, 64);
                        for (int i = 0; i < testX.Length; i++ )
                        {
                            if (testX[i] == Color.black)
                            {
                                testX[i] = Color.Lerp(testX[i], overLay[i], UnityEngine.Random.Range(switcher, switcher + 0.05f));
                            }
                            else if (0.2f > testX[i].b && 0.2f > testX[i].g && 0.2f > testX[i].r)
                            {
                                testX[i] = Color.Lerp(testX[i], overLay[i], UnityEngine.Random.Range(switcher, switcher - 0.40f));
                            }
                        }
                        EditorUtility.DisplayProgressBar("Painting", "Placing Pixels", 1);
                        maskTexture.SetPixels(((int)mousePos.x - 70) % 512, 512 - (int)mousePos.y - 84, 64, 64, testX);
                        newTexture.SetPixels(((int)mousePos.x - 70) % 512, 512 - (int)mousePos.y - 84, 64, 64, testX);
                        maskTexture.Apply();
                        EditorUtility.ClearProgressBar();
                    }
                }
            }
            if (Event.current.type == EventType.mouseMove)
            {

            }
            EditorGUILayout.EndHorizontal();

                
        }
        void OnEnable()
        {
            Texture2D tempTex = Texture2D.blackTexture;
            tempTex.Resize(512, 512);
            newTexture = tempTex;
            brush1Tex = AssetDatabase.LoadAssetAtPath("Assets/Tree Pack/Mountain_1.png", typeof(Texture2D)) as Texture2D;
            Texture2D tenpTex = AssetDatabase.LoadAssetAtPath("Assets/Tree Pack/heiggtmapbg.jpg", typeof(Texture2D)) as Texture2D;
            maskTexture = new Texture2D(512, 512);
            maskTexture.SetPixels(tenpTex.GetPixels());
            cursorTex = AssetDatabase.LoadAssetAtPath("Assets/Tree Pack/mountain_Cursor.png", typeof(Texture2D)) as Texture2D;
            maskTexture.Apply();
            newTexture.Apply();

        }
        void temp(TerrainGenerator currentGenerator, Terrain mapObject, TerrainFromTexture newTerrain)
        {
            if (GUILayout.Button("Terrain From Texture"))
            {
                newTerrain.ApplyHeightmap();
                if (GUILayout.Button("Generate Terrain", EditorStyles.toolbarButton))
                {
                    UnityEngine.Object x = mapObject;
                    currentGenerator.activeTerrain = x as GameObject;
                    currentGenerator.Begin();
                }
                mapObject = EditorGUILayout.ObjectField("Terrain Object", mapObject, typeof(Terrain), true) as Terrain;

                currentGenerator.Tiling = EditorGUILayout.FloatField("Tiling", currentGenerator.Tiling);
                currentGenerator.mapHeight = EditorGUILayout.IntField("Height", currentGenerator.mapHeight);
                if (GUILayout.Button("Generate Mountain", EditorStyles.toolbarButton))
                {
                    currentGenerator.GenerateNoise(mapObject);
                }
            }
        }
    }
}
