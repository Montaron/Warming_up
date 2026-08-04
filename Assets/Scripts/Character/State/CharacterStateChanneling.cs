using UnityEngine;

public class CharacterStateChanneling : CharacterState
{
    public CharacterStateIddle(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override CharacterStateType stateType => CharacterStateType.Channeling;
    public override void Enter()
    {
        stateMachine.Animator.SetFloat("Speed", -1f); 
        player_mov.OrientCharacter();
    }
    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        player_mov.OrientCharacter();
    }
}