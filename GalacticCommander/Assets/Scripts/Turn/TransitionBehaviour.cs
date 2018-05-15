using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : MonoBehaviour {
    
    public void Start()
    {
        TurnBehaviour.instance.PlayerTurn += Player;
        TurnBehaviour.instance.EnemyTurn += Enemy;
    }

    public void NextPhase()
    {
        TurnBehaviour.instance.NextPhase();
    }

    private void Player()
    {
        Turn("Player Start");
        TurnBehaviour.instance.PhaseTurn += Action;
    }

    private void Enemy()
    {
        Turn("Enemy Start");
        TurnBehaviour.instance.PhaseTurn += Action;
    }

    private void Action()
    {
        Turn("Action");
        TurnBehaviour.instance.PhaseTurn = End;
    }

    private void End()
    {
        Turn("End");
    }

    private void Turn(string t)
    {
        Debug.Log(t);
    }
}
