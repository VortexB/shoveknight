using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardData : MonoBehaviour
{
    [SerializeField] private bool isGenerated;
    [SerializeField] private TileUnitData[] TileUnitDataArray;
    [SerializeField] public Vector2 Position;
    [SerializeField] public Vector2Int BoardSize;
    [SerializeField] public Vector2 CellSize;
    [SerializeField] public AutoTile DefaultTile;
    [SerializeField] private Tilemap map;
    [SerializeField] private Grid TileMapGrid;

    List<BoardActor> RemovedActors;
    private GameObject NodeMaster;
    private GameObject[,] NodeArray;
    float nodeOffSetX;
    float nodeOffSetY;

    Vector2 MaxCoord;
    Vector2 MinCoord;
    public Vector2 offset;

    private void Awake()
    {
        RemovedActors = new List<BoardActor>();
    }
    private void Start()
    {
        if (isGenerated)
        {
            RuntimeCalculations();
        }
    }
   
    void RuntimeCalculations()
    {
        nodeOffSetX = 0.5f + Position.x;
        nodeOffSetY = 0.5f + Position.y;

        MaxCoord = new Vector2(BoardSize.x + Position.x-1, BoardSize.y + Position.y-1);
        MinCoord = new Vector2(Position.x, Position.y);
        offset = (CellSize / 2)+Position;
        
    }
    public void GenerateBoard(bool generateNodes)
    {
        TileUnitDataArray = new TileUnitData[BoardSize.x* BoardSize.y];
        NodeArray = new GameObject[BoardSize.x, BoardSize.y];

        GameObject GridObj = new GameObject("TilemapGrid");
        GridObj.transform.parent = gameObject.transform;
        GridObj.transform.position = Position;
        TileMapGrid=GridObj.AddComponent<Grid>();
        TileMapGrid.cellSize = CellSize;

        GameObject MapObj = new GameObject("Tilemap");
        MapObj.transform.parent = GridObj.transform;
        MapObj.transform.position = Position;
        map = MapObj.AddComponent<Tilemap>();
        MapObj.AddComponent<TilemapRenderer>();


        NodeMaster = new GameObject("Node Master");
        NodeMaster.transform.parent = gameObject.transform;
        RuntimeCalculations();
       
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                if(generateNodes)
                    GenerateNode(x, y);
                GenerateTileUnit(x,y);
                map.SetTile(new Vector3Int(x, y, 0), DefaultTile);                
            }
        }
        isGenerated = true;
    }

    private void GenerateTileUnit(int x, int y)
    {
        TileUnitData Tile = ScriptableObject.CreateInstance(typeof(TileUnitData))as TileUnitData;
        Tile.name="Tile: "+x+","+y;
        Tile.Position = new Vector2Int(x, y);
        Tile.Tile = DefaultTile;
        TileUnitDataArray[x*BoardSize.y+ y] = Tile;
    }

    private void GenerateNode(int x, int y)
    {
        NodeArray[x, y] = new GameObject("Node " + x + "," + y);
        NodeArray[x, y].transform.parent = NodeMaster.transform;
        NodeArray[x, y].transform.position = new Vector2(x + nodeOffSetX, y + nodeOffSetY);
        NodeArray[x, y].layer = LayerMask.NameToLayer("Board Selectables");
        SpriteRenderer tempRender = NodeArray[x, y].AddComponent<SpriteRenderer>();
        tempRender.sprite = Sprite.Create(new Texture2D(10, 10), new Rect(Vector2.zero, new Vector2(10, 10)), new Vector2(0, 0));
        tempRender.color = new Color(0, 255, 0);
        tempRender.sortingOrder = 1;
    }


    //change tile props
    //nodes
    public void ChangeTile_Texture(Vector2 NodePos,AutoTile tile)
    {

        Vector3Int tilePos = new Vector3Int((int)(NodePos.x - nodeOffSetX), (int)(NodePos.y - nodeOffSetY), 0);
        if(tile!=null)
            map.SetTile(tilePos, tile);
        TileUnitDataArray[tilePos.x * BoardSize.y + tilePos.y].Tile = tile;

    }
    public void ChangeTile_Movable(Vector2 NodePos, bool movable)
    {
        Vector3Int tilePos = new Vector3Int((int)(NodePos.x - nodeOffSetX), (int)(NodePos.y - nodeOffSetY), 0);
        TileUnitDataArray[tilePos.x * BoardSize.y + tilePos.y].IsMoveable = movable;

    }
    //non nodes
    
    public void SetTileMovable(Vector2Int pos, bool movable)
    {
        TileUnitDataArray[pos.x * BoardSize.y + pos.y].IsMoveable = movable;

    }
    public void SetTileTexture(Vector2Int pos, AutoTile tile)
    {
        TileUnitDataArray[pos.x * BoardSize.y + pos.y].Tile = tile;
        map.SetTile((Vector3Int)pos, tile);
    }
    
    
    //non const
    public void MoveActor(Vector2Int previousPos, Vector2Int newPos,BoardActor actor)
    {
        if (RemovedActors.Contains(actor))
        {
            RemovedActors.Remove(actor);
        }
        if (TileUnitDataArray[previousPos.x * BoardSize.y + previousPos.y].Actor!=null)
            if(TileUnitDataArray[previousPos.x * BoardSize.y + previousPos.y].Actor.Equals(actor))
                TileUnitDataArray[previousPos.x* BoardSize.y + previousPos.y].Actor = null;
        TileUnitDataArray[newPos.x * BoardSize.y + newPos.y].Actor=actor;

    }
    public void RemoveActor(Vector2Int pos)
    {
        RemovedActors.Add(TileUnitDataArray[pos.x * BoardSize.y + pos.y].Actor);
        TileUnitDataArray[pos.x * BoardSize.y + pos.y].Actor = null;
    }
    

   

    //Data getters
    public AutoTile GetTile(Vector2Int pos)
    {
        return map.GetTile((Vector3Int)pos) as AutoTile;
    }

    public AutoTile GetTile(Vector2 NodePos)
    {
        Vector3Int tilePos = new Vector3Int((int)(NodePos.x - nodeOffSetX), (int)(NodePos.y - nodeOffSetY), 0);
        return map.GetTile(tilePos)as AutoTile;
    }
    public bool IsMovable(Vector2Int pos)
    {
        return TileUnitDataArray[pos.x * BoardSize.y + pos.y].IsMoveable;
       
    }
    public bool IsMovable(Vector2 Nodepos)
    {
        Vector3Int tilePos = new Vector3Int((int)(Nodepos.x - nodeOffSetX), (int)(Nodepos.y - nodeOffSetY), 0);
        return TileUnitDataArray[tilePos.x * BoardSize.y + tilePos.y].IsMoveable;

    }
    
    public BoardActor GetActor(Vector2Int pos)
    {
        return TileUnitDataArray[pos.x * BoardSize.y + pos.y].Actor;
    }

    public bool isValidCell(Vector2Int pos)
    {
        if (pos.x > MaxCoord.x || pos.x < MinCoord.x || pos.y > MaxCoord.y || pos.y < MinCoord.y)
            return false;
        return true;
    }
    public bool isValidCell(Vector2 Nodepos)
    {
        if (Nodepos.x > MaxCoord.x || Nodepos.x < MinCoord.x || Nodepos.y > MaxCoord.y || Nodepos.y < MinCoord.y)
            return false;
        return true;
    }

    public TileUnitData[] SaveBoardInstance()
    {
        var T = new TileUnitData[BoardSize.x * BoardSize.y];
        for(int i= 0; i < T.Length; i++)
        {
            T[i]= ScriptableObject.CreateInstance(typeof(TileUnitData)) as TileUnitData;
            T[i].Position = TileUnitDataArray[i].Position;
            T[i].IsMoveable = TileUnitDataArray[i].IsMoveable;
            T[i].Actor = TileUnitDataArray[i].Actor;
            T[i].Tile = TileUnitDataArray[i].Tile;
        }
        return T;
    }

    public void LoadBoardInstance(TileUnitData[] array)
    {

        TileUnitDataArray = array;
        for (int x = 0; x < BoardSize.x; x++)
            for (int y = 0; y < BoardSize.y; y++)
                if (array[x * BoardSize.y + y].Actor != null) 
                {
                    array[x * BoardSize.y + y].Actor.BackInTime(new Vector2Int(x, y));
                    map.SetTile(new Vector3Int(x, y, 0), TileUnitDataArray[x * BoardSize.y + y].Tile);
                }

    }


      

       

        
    

    
}
