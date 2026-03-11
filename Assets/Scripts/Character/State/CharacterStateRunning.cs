using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterStateRunning : CharacterState
{
    public CharacterStateRunning(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override CharacterStateType stateType => CharacterStateType.Running;
    public override void Enter()
    {
        Debug.Log(stateType);
        stateMachine.Animator.SetFloat("Speed", player_mov.inputVector.magnitude); 
    }

    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        player_mov.MoveCharacter();
        if (player_mov.inputVector.magnitude == 0)
        {
            stateMachine.ChangeState(CharacterStateType.Iddle);
        } 
    }
}