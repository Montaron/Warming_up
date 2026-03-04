using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public CharacterMovement_iso Player_Mov { get; private set; }
    public Animator Animator { get; private set; }

    public CharacterStateIddle idleState { get; private set; }
    public CharacterStateRunning runningState { get; private set; }
    public CharacterStateAttack attackingState { get; private set; }

    public CharacterState previousState { get; private set; }
    public CharacterState currentState { get; private set; }
    public Action<CharacterStateType> OnStateChanged;
    private Dictionary<CharacterStateType, CharacterState> stateMap;
    // Start is called before the first frame update
    void Awake()
    {
        Animator = GetComponent<Animator>();
        Player_Mov = GetComponent<CharacterMovement_iso>();
        idleState = new CharacterStateIddle(this);
        runningState = new CharacterStateRunning(this);
        attackingState = new CharacterStateAttack(this);
        stateMap = new Dictionary<CharacterStateType, CharacterState>
        {
            { idleState.stateType, idleState },
            { runningState.stateType, runningState },
            { attackingState.stateType, attackingState }
        };

    }
    void Start()
    {
        ChangeState(idleState.stateType);
        previousState = idleState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.HandleInput();
    }

    private CharacterState GetStateFromType(CharacterStateType type)
    {
        if (stateMap.TryGetValue(type, out CharacterState state))
            return state;
        throw new ArgumentOutOfRangeException(nameof(type), $"No state found for type {type}");
    }

    public void ChangeState(CharacterStateType newStateType)
    {
        if (currentState == null)
        {
            currentState = GetStateFromType(newStateType);
            currentState.Enter();
            return;
        }
        if (currentState.stateType == newStateType)
            return;
        currentState?.Exit();
        previousState = currentState;
        currentState = GetStateFromType(newStateType);
        OnStateChanged?.Invoke(currentState.stateType);
        currentState.Enter();
    }
}
