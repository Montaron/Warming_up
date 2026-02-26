using UnityEngine;

public abstract class CharacterState
{
    protected CharacterStateMachine stateMachine;
    protected char_mov_iso player_mov;
    public abstract CharacterStateType stateType {get;}

    protected CharacterState(CharacterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.player_mov = stateMachine.Player_Mov;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void HandleInput() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}

public enum CharacterStateType 
{
    Iddle,
    Running,
    Attacking
}

