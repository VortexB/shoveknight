using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardActor : MonoBehaviour
{
    protected const float centerConst = 0.5f;
    [SerializeField] protected BoardData Board;
    //[SerializeField] protected ErrorText ErrorText = null;

    //stats
    protected Vector2Int startingPos= new Vector2Int(-1, -1);

    private Vector2Int position;
    public Vector2Int Position 
    {
        get=>position;

        set
        {
            if (Board.GetActor(value) == null|| Board.GetActor(value)==this)
            {
                Board.MoveActor(position, value, this);
                position = value;
                MoveOnGrid();
            }
        }
    }
    public virtual void StartUp()
    {
        if (Board == null) Board = FindObjectOfType<BoardData>();
        
    }
    public void ForceMove(Vector2Int pos)
    {
        Position = pos;
    }
    public virtual bool BasicMove(Vector2Int pos)
    {
        if (Board.isValidCell(pos))
        {
            Vector2Int oldPos = Position;
            Position = pos;
            if (Position == oldPos)
            {
                return false;
            }
            return true;
        }
        return false;
    }   
    protected virtual void MoveOnGrid()
    {
        transform.position = new Vector3(Position.x + centerConst, Position.y + centerConst);
    }
    public virtual void BackInTime(Vector2Int pos)
    {
        BasicMove(pos);
    }
}
