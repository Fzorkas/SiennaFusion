using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(WrapTile))]
public class WrapTileEditor : Editor
{
    Rect baseRect;

    SerializedProperty flag;

    WrapTile tile;
    float wdiv;
    float spaceTrack;
    bool BaseTileEdit, CenterInternTileEdit, EdgeInternTileEdit, AceptedEdit;

    public void OnEnable()
    {
        flag = serializedObject.FindProperty("flags");
        baseRect = new Rect(10, 50, 50, 50);
    }

    public override void OnInspectorGUI()
    {
        tile = (WrapTile)target;
        wdiv = ((EditorGUIUtility.currentViewWidth - 30) / 3);
        spaceTrack = 0;
        baseRect.width = wdiv;
        baseRect.height = wdiv;
        baseRect.y = 50;

        // recreation of key parts from base inspector
        GUILayout.BeginHorizontal();
        GUILayout.Label("Sprite");
        tile.sprite = (Sprite)EditorGUILayout.ObjectField(tile.sprite, typeof(Sprite), false, GUILayout.Width(EditorGUIUtility.currentViewWidth/2));
        GUILayout.EndHorizontal();
        tile.color = EditorGUI.ColorField(new Rect(EditorGUIUtility.currentViewWidth/2 - 5, 70, EditorGUIUtility.currentViewWidth/2, 20), tile.color );
        GUILayout.Label("Color");

        /*serializedObject.Update();
        EditorGUILayout.PropertyField(flag);
        serializedObject.ApplyModifiedProperties();*/
        
        baseRect.y += 40;
        spaceTrack += baseRect.y - (50 + spaceTrack);

        //Base Tiles Foldout (Dropdown)
        BaseTileEdit = EditorGUILayout.Foldout(BaseTileEdit, "Base Tiles");
        baseRect.y += 18;
        spaceTrack += baseRect.y - (50 + spaceTrack);
        if (BaseTileEdit)
        {
            // main set of 9 tiles 
            baseTitle("Base border Tiles and center");
            DrawOutside(6, 14, 10);
            baseRect.y += wdiv;
            DrawOutside(7, 15, 11);
            baseRect.y += wdiv;
            DrawOutside(5, 13, 9);

            // single width tiles
            baseRect.y += 20 + wdiv;
            baseTitle("Base single Width Horizontl Tiles");
            DrawOutside(4, 12, 8);
            baseRect.y += 10 + wdiv;
            baseTitle("Base single Width Vertical Tiles");
            baseTitle("Left(Top) - Right(Bottom)");
            DrawOutside(2, 3, 1);
            baseRect.y += 10 + wdiv;
            baseTitle("Base single Tile");
            baseRect.x = 10;
            tile.outside[0] = (Sprite)EditorGUI.ObjectField(baseRect, tile.outside[0], typeof(Sprite), false);
            baseRect.y += wdiv;
            baseTitle("");
        }

        //Center Internal Conner Tiles Foldout (Dropdown)
        CenterInternTileEdit = EditorGUILayout.Foldout(CenterInternTileEdit, "Central Internal Conner Tiles");
        baseRect.y += 18;
        spaceTrack += baseRect.y - (50 + spaceTrack);
        if (CenterInternTileEdit)
        {
            // main set of 9 tiles
            baseTitle("internal facing corner Tiles and center");
            DrawCenterInternal(13, 9, 11);
            baseRect.y += wdiv;
            DrawCenterInternal(12, 0, 3);
            baseRect.y += wdiv;
            DrawCenterInternal(14, 6, 7);

            // single width tiles
            baseRect.y += 20 + wdiv;
            baseTitle("diagnal interior corner Tiles");
            baseRect.x = 10;
            tile.interiorMain[5] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[5], typeof(Sprite), false);
            baseRect.x += wdiv;
            tile.interiorMain[10] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[10], typeof(Sprite), false);


            baseRect.y += 10 + wdiv;
            baseTitle("three interior corner Tiles");
            baseRect.x = 10;
            tile.interiorMain[8] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[8], typeof(Sprite), false);
            baseRect.x += wdiv;
            tile.interiorMain[1] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[1], typeof(Sprite), false);
            baseRect.y += wdiv;
            baseRect.x = 10;
            tile.interiorMain[4] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[4], typeof(Sprite), false);
            baseRect.x += wdiv;
            tile.interiorMain[2] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[2], typeof(Sprite), false);


            baseRect.y += 10 + wdiv;
            baseTitle("Base single Tile");
            baseRect.x = 10;
            tile.interiorMain[15] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[15], typeof(Sprite), false);
            baseRect.y += wdiv;
            baseTitle("");
        }

        // Edge Internal Corner Tiles Foldout (Dropdown)
        EdgeInternTileEdit = EditorGUILayout.Foldout(EdgeInternTileEdit, "Edge Internal Conner Tiles");
        baseRect.y += 18;
        spaceTrack += baseRect.y - (50 + spaceTrack);
        if (EdgeInternTileEdit)
        {
            baseTitle("Horizontal Edge Interior corner Tiles");
            DrawEdgeInternal(4, 5, 6);
            baseRect.y += wdiv;
            DrawEdgeInternal(7, 8, 9);

            baseRect.y += 10 + wdiv;
            baseTitle("Vertical Edge Interior corner Tiles");
            DrawEdgeInternal2Set(10, 13);
            DrawEdgeInternal2Set(11, 14);
            DrawEdgeInternal2Set(12, 15);

            baseRect.y += 10;
            baseTitle("outside corner with Internal corner Tiles");
            DrawEdgeInternal2Set(0, 1);
            DrawEdgeInternal2Set(2, 3);
        }

        // acepted tiels to merger on borders with
        GUILayout.Space(6);
        AceptedEdit = EditorGUILayout.Foldout(AceptedEdit, "Acepted Edge Merge Tiles - (" + tile.accept.Count + ")");
        baseRect.y += 14;
        if (AceptedEdit)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(20))) tile.accept.Add(null);
            GUILayout.Label(" add a new tile to list");
            GUILayout.EndHorizontal();
            baseRect.y += 28;
            for (int i = 0; i < tile.accept.Count; i++)
            {
                GUILayout.BeginHorizontal();
                tile.accept[i] = (Tile)EditorGUILayout.ObjectField(tile.accept[i], typeof(Tile), false);
                bool deleteCheck = false;
                if (GUILayout.Button("-", GUILayout.Width(20))) deleteCheck = true;
                baseRect.y += 20;
                GUILayout.EndHorizontal();

                if (deleteCheck)
                {
                    tile.accept.RemoveAt(i);
                }
                else
                {
                    if (tile.accept[i])
                    {
                        GUI.Box(baseRect, AssetPreview.GetAssetPreview(tile.accept[i].sprite));
                        GUILayout.Space(wdiv);
                        baseRect.y += wdiv;
                    }
                }
            }
            spaceTrack += baseRect.y - (50 + spaceTrack);
        }

        EditorUtility.SetDirty(tile);

        // remove coment slashes to add Original unity GUI to the bottom
        baseRect.y += 30;
        baseTitle("[[ Original GUI ]]");
        base.OnInspectorGUI();
    }

    private void baseTitle(string text)
    {
        GUILayout.Space(baseRect.y - (50 + spaceTrack));
        baseRect.y += 18;
        spaceTrack += baseRect.y - (50 + spaceTrack);
        GUILayout.Label(text); 
    }

    private void DrawOutside(int i1, int i2, int i3)
    {
        baseRect.x = 10;
        tile.outside[i1] = (Sprite)EditorGUI.ObjectField(baseRect, tile.outside[i1], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.outside[i2] = (Sprite)EditorGUI.ObjectField(baseRect, tile.outside[i2], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.outside[i3] = (Sprite)EditorGUI.ObjectField(baseRect, tile.outside[i3], typeof(Sprite), false);
    }

    private void DrawCenterInternal(int i1, int i2, int i3)
    {
        baseRect.x = 10;
        tile.interiorMain [i1] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[i1], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.interiorMain[i2] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[i2], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.interiorMain[i3] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorMain[i3], typeof(Sprite), false);
    }

    private void DrawEdgeInternal(int i1, int i2, int i3)
    {
        baseRect.x = 10;
        tile.interiorOther[i1] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorOther[i1], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.interiorOther[i2] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorOther[i2], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.interiorOther[i3] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorOther[i3], typeof(Sprite), false);
    }
    private void DrawEdgeInternal2Set(int i1, int i2)
    {
        baseRect.x = 10;
        tile.interiorOther[i1] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorOther[i1], typeof(Sprite), false);
        baseRect.x += wdiv;
        tile.interiorOther[i2] = (Sprite)EditorGUI.ObjectField(baseRect, tile.interiorOther[i2], typeof(Sprite), false);
        baseRect.y += wdiv;
    }
}
