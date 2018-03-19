using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WrapTile : Tile {
    public List<Tile> accept;
    public Sprite[] outside = new Sprite[16];
    public Sprite[] interiorMain = new Sprite[16];
    public Sprite[] interiorOther = new Sprite[16];

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // recheck for tile and all surouning
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int npos = new Vector3Int(position.x + x, position.y + y, position.z);
                tilemap.RefreshTile(npos);
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        CheckSprite(position, tilemap, ref tileData);
    }

    public void CheckSprite(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Debug.Log("sigh");

        bool upPos = CheckPositon(position + Vector3Int.up, tilemap);
        bool downPos = CheckPositon(position + Vector3Int.down, tilemap);
        bool rightPos = CheckPositon(position + Vector3Int.right, tilemap);
        bool leftPos = CheckPositon(position + Vector3Int.left, tilemap);

        int val1 = 0; // selector for outside tile list
        int val2 = 0; // for main indise tiles

        if (upPos) val1 += 1;   
        if (downPos) val1 += 2;
        if (rightPos) val1 += 4;
        if (leftPos) val1 += 8;

        tileData.sprite = null;

        if (val1 == 15)
        {
            Val2Set(ref val2, upPos, downPos, rightPos, leftPos, position, tilemap);
            tileData.sprite = interiorMain[val2];
        }
        else if(val1 == 6 || val1 == 5 || val1 == 10 || val1 == 9) // L bends
        {
            if (val1 == 6 && !CheckPositon(position + Vector3Int.down + Vector3Int.right, tilemap)) tileData.sprite = interiorOther[0];
            if (val1 == 10 && !CheckPositon(position + Vector3Int.down + Vector3Int.left, tilemap)) tileData.sprite = interiorOther[1];
            if (val1 == 5 && !CheckPositon(position + Vector3Int.up + Vector3Int.right, tilemap)) tileData.sprite = interiorOther[2];
            if (val1 == 9 && !CheckPositon(position + Vector3Int.up + Vector3Int.left, tilemap)) tileData.sprite = interiorOther[3];
        }
        else if (val1 == 7 || val1 == 11 || val1 == 13 || val1 == 14) // T intersections
        {
            Val2Set(ref val2, upPos, downPos, rightPos, leftPos, position, tilemap);
            
            if (val1 == 14)
            {
                if (val2 == 4) tileData.sprite = interiorOther[4];
                if (val2 == 0) tileData.sprite = interiorOther[5];
                if (val2 == 2) tileData.sprite = interiorOther[6];
            }
            if (val1 == 13)
            {
                if (val2 == 8) tileData.sprite = interiorOther[7];
                if (val2 == 0) tileData.sprite = interiorOther[8];
                if (val2 == 1) tileData.sprite = interiorOther[9];
            }
            if (val1 == 7)
            {
                if (val2 == 1) tileData.sprite = interiorOther[10];
                if (val2 == 0) tileData.sprite = interiorOther[11];
                if (val2 == 2) tileData.sprite = interiorOther[12];
            }
            if (val1 == 11)
            {
                if (val2 == 8) tileData.sprite = interiorOther[13];
                if (val2 == 0) tileData.sprite = interiorOther[14];
                if (val2 == 4) tileData.sprite = interiorOther[15];
            }
        }

        if (tileData.sprite == null)
        {
            tileData.sprite = outside[val1];
        }
    }

    // checks if positon contains a acepted tile
    public bool CheckPositon(Vector3Int position, ITilemap tilemap) 
    {
        Tile tile = tilemap.GetTile(position) as Tile;
        return (accept.Contains(tile) || this == tile);
    }

    // run if statements to set uf val2 when needed
    public void Val2Set(ref int val2, bool upPos, bool downPos, bool rightPos, bool leftPos, Vector3Int position, ITilemap tilemap)
    {
        if (upPos && rightPos && CheckPositon(position + Vector3Int.up + Vector3Int.right, tilemap))
            val2 += 1;
        if (downPos && rightPos && CheckPositon(position + Vector3Int.down + Vector3Int.right, tilemap))
            val2 += 2;
        if (upPos && leftPos && CheckPositon(position + Vector3Int.up + Vector3Int.left, tilemap))
            val2 += 8;
        if (downPos && leftPos && CheckPositon(position + Vector3Int.down + Vector3Int.left, tilemap))
            val2 += 4;
    }

#if UNITY_EDITOR
    [MenuItem("Custom Addons/Tiles/New/WrapTile")]
    public static void CreateTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Wrap Tile", "NewWrapTile", "asset", "Save WrapTile", "Assets");
        if (path == "")
        {
            return;
        }
        WrapTile newTile = ScriptableObject.CreateInstance<WrapTile>();
        newTile.accept = new List<Tile>();
        AssetDatabase.CreateAsset(newTile, path);
    }
#endif
}
