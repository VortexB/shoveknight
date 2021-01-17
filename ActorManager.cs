using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    [SerializeField] private List<BoardActor> Actors=null;
    public List<BoardActor> GetActors() => Actors;
    [SerializeField] private List<Vector2Int>Positions=null;
    [SerializeField] private BoardData Board;


    public static ActorManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
    }
    void Start()
    {
        foreach(BoardActor actor in Actors)
        {
            actor.StartUp();
            if (actor.GetComponent<PushableActor>() != null)
            {
                LevelController.Instance.RemainingPushable++;
            }
            if (actor.GetComponent<CollectableActor>() != null)
            {
                LevelController.Instance.RemainingCollectable++;
            }
        }
        if (Board == null) Board = FindObjectOfType<BoardData>();
        SetPositions();
        foreach (BoardActor actor in Actors)
        {
            if (actor.GetComponent<PlayerActor>() != null)
            {
                LevelController.Instance.PlaceStart(actor.GetComponent<PlayerActor>().Position);
            }
        }
    }
    public void AddActors(BoardActor actor)=>Actors.Add(actor);
    public void SetPositions(List<Vector2Int> positions) => Positions = positions;
    
    void SetPositions()
    {
        if (Board == null) Board = FindObjectOfType<BoardData>();
        for(int i = 0; i < Actors.Count; i++)
        {
            Actors[i].ForceMove(Positions[i]);
            Board.MoveActor(Actors[i].Position, Positions[i], Actors[i]);
        }
        foreach(BoardActor actor in Actors)
        {
            actor.Position = actor.Position;
        }

    }

}
