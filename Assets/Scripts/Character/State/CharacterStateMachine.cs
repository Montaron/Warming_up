using System;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public char_mov_iso Player_Mov { get; private set; }
    public Animator Animator { get; private set; }

    public CharacterStateIddle idleState { get; private set; }
    public CharacterStateRunning runningState { get; private set; }
    public CharacterStateAttack attackingState { get; private set; }

    public CharacterState previousState { get; private set; }
    private CharacterState currentState;

    public Action<CharacterStateType> OnStateChanged;
    // Start is called before the first frame update
    void Awake()
    {
        Animator = GetComponent<Animator>();
        Player_Mov = GetComponent<char_mov_iso>();
        idleState = new CharacterStateIddle(this);
        runningState = new CharacterStateRunning(this);
        attackingState = new CharacterStateAttack(this);
    }
    void Start()
    {
        ChangeState(idleState);
        previousState = idleState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.HandleInput();
    }

    public void ChangeState(CharacterState newState)
    {
        if (newState == null)
        {
            Debug.LogWarning("Trying to change to a null state.");
            return;
        }
        if (currentState == newState)
            return;
        currentState?.Exit();
        previousState = currentState;
        currentState = newState;
        OnStateChanged?.Invoke(currentState.stateType);
        currentState.Enter();
    }
}
