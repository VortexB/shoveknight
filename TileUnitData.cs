using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "GridTileDataArray", menuName = "GridArray/Tile")]
public class TileUnitData: ScriptableObject
{
    public Vector2Int Position = new Vector2Int(0,0);
    public bool IsMoveable = true;
    public BoardActor Actor = null;
    public AutoTile Tile= null;
    [HideInInspector] public int pathfindingIterationValue=0; 


}
