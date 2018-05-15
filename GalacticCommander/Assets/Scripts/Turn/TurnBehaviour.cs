using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBehaviour : MonoBehaviour {
    public static TurnBehaviour instance;

    public delegate void TurnDelegate();
    public TurnDelegate EnemyTurn, PlayerTurn, ActionPhase, EndPhase;

    public TurnEnum Turn
    {
        get { return turn; }
    }

    public enum TurnEnum
    {
        Player,
        Enemy
    }
    private enum Phase
    {
        Start,
        Action,
        End
    }

    private TurnEnum turn;
    private Phase phase;
    private TurnDelegate startTurn;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple turn behaviours.");
        instance = this;
    }

    private void Start()
    {
        startTurn = PlayerTurn;
    }

    public void NextTurn()
    {
        turn = turn == TurnEnum.Player ? TurnEnum.Enemy : TurnEnum.Player;
        switch(turn)
        {
            case TurnEnum.Player:
                startTurn = PlayerTurn;
                break;
            case TurnEnum.Enemy:
                startTurn = EnemyTurn;
                break;
        }
    }

    public void NextPhase()
    {
        switch (phase)
        {
            case Phase.Start:
                startTurn?.Invoke();
                startTurn = null;
                phase = Phase.Action;
                break;
            case Phase.Action:
                ActionPhase?.Invoke();
                ActionPhase = null;
                phase = Phase.End;
                break;
            case Phase.End:
                EndPhase?.Invoke();
                EndPhase = null;
                phase = Phase.Start;
                NextTurn();
                break;
        }
    }
}
