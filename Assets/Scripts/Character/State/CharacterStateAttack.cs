using UnityEngine;

public class CharacterStateAttack : CharacterState
{
    public CharacterStateAttack(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override CharacterStateType stateType => CharacterStateType.Attacking;
    public override void Enter()
    {
        // Debug.Log(stateType);
    }
    public override void Exit()
    {
    }

    public override void HandleInput()
    {
    }
}