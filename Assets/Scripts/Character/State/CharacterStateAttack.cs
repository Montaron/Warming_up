using UnityEngine;

public class CharacterStateAttack : CharacterState
{
    public CharacterStateAttack(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override CharacterStateType stateType => CharacterStateType.Attacking;
    public override void Enter()
    {
        stateMachine.Animator.SetTrigger("Attack");
        Debug.Log(stateType);
    }
    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        if (player_mov.attackFinished)
        {
            stateMachine.ChangeState(stateMachine.previousState); // Return to previous state after attack
        }
    }
}