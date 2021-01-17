using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : BoardActor
{
    [SerializeField] AutoTile fallenFloorTile;
    Stack<TileUnitData[]> BoardStates;
    private bool paused;
    Animator anim;

    public override void StartUp()
    {
        base.StartUp();
    }
    private void Start()
    {
        BoardStates = new Stack<TileUnitData[]>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!paused)
        {
            if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(new Vector2Int(0, 1));
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                anim.ResetTrigger("Left");
                anim.SetTrigger("Right");
                Move(new Vector2Int(1, 0));
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                anim.ResetTrigger("Right");
                anim.SetTrigger("Left");
                Move(new Vector2Int(-1, 0));

            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(new Vector2Int(0, -1));
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Undo();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelController.Instance.RestartLevel();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                LevelController.Instance.Pause();
                paused = true;
            }
            else
            {
                LevelController.Instance.UnPause();
                paused = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            print(Board.GetActor(new Vector2Int(4,4)));
        }
    }

    private void Undo()
    {
        if (BoardStates.Count != 0)
        {           
            Board.LoadBoardInstance(BoardStates.Pop());
        }
    }

    private void Move(Vector2Int dir)
    {
        if(startingPos == new Vector2Int(-1,-1))
        {
            startingPos = Position;
        }
        var orgPos = Position;
        Vector2Int dest = dir + orgPos;
        if (!Board.isValidCell(dest)) return;
        if (!Board.IsMovable(dest)) return;
        var A= Board.GetActor(dest);
        BoardStates.Push(Board.SaveBoardInstance());
        AudioController.instance.PlaySound("move");


        if (A != null)
        {
            if (A.GetComponent<PushableActor>() != null)
            {
                A.GetComponent<PushableActor>().Push(dest + dir);
                return;
                
            }
            if (A.GetComponent<CollectableActor>() != null)
            {
                A.GetComponent<CollectableActor>().Collect();
                    
            }
        }
        
        BasicMove(dest);

        if(orgPos!=startingPos)
            FallenFloor(orgPos);
        if (Position == startingPos)
        {
            LevelController.Instance.IsLevelComplete();
        }
    }

    private void FallenFloor(Vector2Int pos)
    {
        Board.SetTileMovable(pos, false);
        Board.SetTileTexture(pos, fallenFloorTile);
    }

    protected override void MoveOnGrid()
    {
        StartCoroutine(SlideMoveOnGrid());
    }

    IEnumerator SlideMoveOnGrid()
    {
        Vector2 startPos=transform.position;
        int i = 0;
        
        while (i < 1000)
        {
            i++;
            yield return new WaitForFixedUpdate();
            transform.position = Vector3.MoveTowards(transform.position, (Vector3Int)Position + new Vector3(0.5f, 0.5f,0), Vector3.Distance(transform.position, (Vector3Int)Position + new Vector3(0.5f, 0.5f, 0)) /100);
        }
    }
}
    

