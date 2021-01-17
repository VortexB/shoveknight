using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableActor : BoardActor
{
    bool collected = false;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Collect()
    {

        Board.RemoveActor(Position);
            LevelController.Instance.RemainingCollectable--;
            collected = true;
        anim.SetBool("Alive", false);
        AudioController.instance.PlaySound("key");


    }

    public override void BackInTime(Vector2Int pos)
    {
        if(Board.GetActor(Position) == this && collected)
        {
            Board.MoveActor(pos, pos, this);
            LevelController.Instance.RemainingCollectable++;
            collected = false;
            anim.SetBool("Alive", true);

        }
    }
}
