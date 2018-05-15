using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBehaviour : MonoBehaviour {
    public static TurnBehaviour instance;

    public delegate void TurnDelegate();
    public TurnDelegate EnemyTurn, PlayerTurn, PhaseTurn;

    private enum State
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

    private State state;
    private Phase phase;
    private TurnDelegate turn;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple turn behaviours.");
        instance = this;
    }

    private void Start()
    {
        turn = PlayerTurn;
    }

    public void NextTurn()
    {
        state = state == State.Player ? State.Enemy : State.Player;
        switch(state)
        {
            case State.Player:
                turn = PlayerTurn;
                break;
            case State.Enemy:
                turn = EnemyTurn;
                break;
        }
    }

    public void NextPhase()
    {
        switch (phase)
        {
            case Phase.Start:
                turn?.Invoke();
                turn = PhaseTurn;
                phase = Phase.Action;
                break;
            case Phase.Action:
                turn?.Invoke();
                turn = PhaseTurn;
                phase = Phase.End;
                break;
            case Phase.End:
                turn?.Invoke();
                PhaseTurn = turn = null;
                phase = Phase.Start;
                NextTurn();
                break;
        }
    }
}
