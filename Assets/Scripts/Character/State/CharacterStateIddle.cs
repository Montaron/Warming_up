using UnityEngine;

public class CharacterStateIddle : CharacterState
{
    public CharacterStateIddle(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override CharacterStateType stateType => CharacterStateType.Iddle;
    public override void Enter()
    {
        Debug.Log(stateType);
        stateMachine.Animator.SetFloat("Speed", -1f); 
    }
    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        if (player_mov.isAttacking)
        {
            stateMachine.ChangeState(stateMachine.attackingState);
        }

        if (player_mov.inputVector.magnitude > 0.1f)
        {
            stateMachine.ChangeState(stateMachine.runningState); // Placeholder for actual movement state
        } 
    }
}