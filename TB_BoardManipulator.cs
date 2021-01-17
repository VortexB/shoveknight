using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TB_BoardManipulator : EditorWindow
{
    static TB_BoardManipulator window;
    Vector2 scrollPos;
    bool[] dropdowns = new bool[1];
    //add new data storage here
    AutoTile cellTile;
    bool isMovable;
    bool blocksVision;

    [MenuItem("Window/Board Manipulator")]
    public static void ShowWindow()
    {
        window = GetWindow<TB_BoardManipulator>("Board Manipulator");
        window.minSize = new Vector2(300, 100);
    }
    private void OnGUI()
    {
        if (window != null) scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(window.position.width), GUILayout.Height(window.position.height));

        GUILayout.Label("Open Generate Board", EditorStyles.boldLabel);

        if (GUILayout.Button("New Board"))
        {

            FocusWindowIfItsOpen<TB_BoardManipulator_NewBoard>();
            GetWindow<TB_BoardManipulator_NewBoard>("Board Generator");
        }
        GUILayout.Space(10);
        GUILayout.Label("Selected Nodes:", EditorStyles.boldLabel);
        if (dropdowns.Length < Selection.gameObjects.Length)
        {
            bool[] temp = new bool[Selection.gameObjects.Length];
            for (int i = 0; i < dropdowns.Length; i++)
            {
                temp[i] = dropdowns[i];
            }
            dropdowns = temp;
        }
        if (GUILayout.Button("Expand Toggle"))
        {
            for (int i = 0; i < dropdowns.Length; i++) dropdowns[i] = !dropdowns[i];
        }
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (Selection.gameObjects[i].layer.Equals(LayerMask.NameToLayer("Board Selectables")))
            {
                dropdowns[i] = EditorGUILayout.Foldout(dropdowns[i], Selection.gameObjects[i].name);
                if (dropdowns[i])
                {
                    BoardData data = Selection.gameObjects[i].GetComponentInParent<BoardData>();
                    Vector2 pos = Selection.gameObjects[i].transform.position;
                    string output = "";
                    output += "-Tile:" + data.GetTile(pos).ToString() + "\n";
                    if (data.IsMovable(pos)) output += "-Movable" + "\n";

                    GUILayout.Label(output);
                    //if(data.IsMovable(pos)) GUILayout.Label("Tile: true");
                    //else GUILayout.Label("Tile: false");

                }
                GUILayout.Space(3);
            }
        }

        if (Selection.gameObjects.Length > 0)
        {
            if (Selection.gameObjects[0].layer.Equals(LayerMask.NameToLayer("Board Selectables")))
            {
                BoardData data = Selection.gameObjects[0].GetComponentInParent<BoardData>();

                GUILayout.Label("Manipulate", EditorStyles.boldLabel);
                cellTile = EditorGUILayout.ObjectField("Tile", cellTile, typeof(AutoTile), true) as AutoTile;
                if (GUILayout.Button("Replace"))
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        if (obj.layer.Equals(LayerMask.NameToLayer("Board Selectables")))
                        {
                            data.ChangeTile_Texture(obj.transform.position, cellTile);
                        }
                    }
                }
                GUILayout.Space(3);
                GUILayout.Label("Movable?");
                isMovable = EditorGUILayout.Toggle(isMovable);
                if (GUILayout.Button("Replace"))
                {
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        if (obj.layer.Equals(LayerMask.NameToLayer("Board Selectables")))
                        {
                            data.ChangeTile_Movable(obj.transform.position, isMovable);
                        }
                    }
                }
                GUILayout.Space(3);
                GUILayout.Label("Blocks Vision?");
                blocksVision = EditorGUILayout.Toggle(blocksVision);
                if (GUILayout.Button("Replace"))
                {
                    
                }


            }
        }
        else GUILayout.Label("No nodes selected");
        if (window != null) EditorGUILayout.EndScrollView();

    }
}
public class TB_BoardManipulator_NewBoard : EditorWindow
{
    string Boardname = "Board";
    Vector2 Boardposition;
    Vector2Int Boardsize = new Vector2Int(10, 10);
    Vector2 cellsize = new Vector2Int(1, 1);
    AutoTile tile;
    private void OnGUI()
    {
        GUILayout.Label("Generate Board", EditorStyles.boldLabel);
        Boardname = EditorGUILayout.TextField("Name:", Boardname);
        Boardposition = EditorGUILayout.Vector2Field("Position", Boardposition);
        Boardsize = EditorGUILayout.Vector2IntField("Board Size", Boardsize);
        cellsize = EditorGUILayout.Vector2Field("Cell Size", cellsize);
        tile = EditorGUILayout.ObjectField("Default Tile", tile, typeof(AutoTile), true) as AutoTile;
        if (GUILayout.Button("Generate"))
        {
            if (tile == null) Debug.Log("Need Tile");
            else GenerateBoard(Boardname, Boardposition, Boardsize, cellsize, tile);
        }
    }

    private void GenerateBoard(string name, Vector2 position, Vector2Int Boardsize, Vector2 cellsize, AutoTile tile)
    {
        GameObject Board = new GameObject();
        Board.name = name;
        BoardData data = Board.AddComponent<BoardData>();
        data.Position = position;
        data.BoardSize = Boardsize;
        data.CellSize = cellsize;
        data.DefaultTile = tile;
        data.GenerateBoard(true);


    }
}
