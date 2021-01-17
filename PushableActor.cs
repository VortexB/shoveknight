using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableActor : BoardActor
{
    [SerializeField] AutoTile fallenFloorTile;
    [SerializeField] AutoTile woodFloorTile;
    bool fallen = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override bool BasicMove(Vector2Int pos)
    {
        if (Board.GetTile(pos) == null)
        {
            return false;
        }
        if (Board.GetTile(pos) != woodFloorTile && Board.GetTile(pos) != fallenFloorTile)
        {
            return false;
        }
        var x= base.BasicMove(pos);
        if (!Board.GetTile(pos).Equals(fallenFloorTile) && x && fallen == true)
        {
            LevelController.Instance.RemainingPushable++;
            fallen = false;
            anim.SetBool("Alive", true);
            Board.MoveActor(pos, pos, this);

        }
        return x;
    }
    public bool Push(Vector2Int pos)
    {

        if (!Board.isValidCell(pos)) return false;
        if (Board.GetTile(pos) == null)
        {
            return false;
        }
        if (Board.GetTile(pos) != woodFloorTile && Board.GetTile(pos) != fallenFloorTile)
        {
            return false;
        }
        var val = base.BasicMove(pos);

        if (Board.GetTile(pos).Equals(fallenFloorTile))
        {
            LevelController.Instance.RemainingPushable--;
            fallen = true;
            anim.SetBool("Alive", false);
            StartCoroutine(Camera.main.GetComponent<CamShake>().Shake(0.15f, 0.1f));
            Board.RemoveActor(Position);

        }
        AudioController.instance.PlaySound("push");
        return val;
    }

    protected override void MoveOnGrid()
    {
        StartCoroutine(SlideMoveOnGrid());
    }

    IEnumerator SlideMoveOnGrid()
    {
        Vector2 startPos = transform.position;
        int i = 0;

        while (i < 1000)
        {
            i++;
            yield return new WaitForFixedUpdate(); ;
            transform.position = Vector3.MoveTowards(transform.position, (Vector3Int)Position + new Vector3(0.5f, 0.5f, 0), Vector3.Distance(transform.position, (Vector3Int)Position + new Vector3(0.5f, 0.5f, 0)) / 100);
        }
    }
}
