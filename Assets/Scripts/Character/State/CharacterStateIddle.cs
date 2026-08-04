using UnityEngine;

public class CharacterStateIddle : CharacterState
{
    public CharacterStateIddle(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override CharacterStateType stateType => CharacterStateType.Iddle;
    public override void Enter()
    {
        // Debug.Log(stateType);
        stateMachine.Animator.SetFloat("Speed", -1f); 
        player_mov.OrientCharacter();
    }
    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        player_mov.OrientCharacter();
        if (player_mov.inputVector.magnitude > 0.1f)
        {
            stateMachine.ChangeState(CharacterStateType.Running); // Placeholder for actual movement state
        } 
    }
}