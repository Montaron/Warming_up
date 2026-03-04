using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private CharacterStateMachine stateMachine;
    private CharacterCombat characterCombat;
    private InputHandler inputHandler;
    private CharacterMovement_iso characterMov_iso;
    public CharacterStateType currentState { get; private set; }

    void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogWarning("stateMachine not referenced");
        }
        inputHandler = GetComponent<InputHandler>();
        if (inputHandler == null)
        {
            Debug.LogWarning("inputHandler not referenced");
        }
        characterCombat = GetComponent<CharacterCombat>();
        if (characterCombat == null)
        {
            Debug.LogWarning("CharacterCombat not referenced");
        }
    }
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        stateMachine.OnStateChanged += UpdateCurrentState;
        inputHandler.OnMoveInput += HandleMoveInput;
        inputHandler.OnSpellRequested += HandleSpellRequested;
    }

    private void HandleSpellRequested(Spell_data data)
    {
        if (currentState == CharacterStateType.Iddle)
        {
            stateMachine.ChangeState(CharacterStateType.Attacking);
            characterCombat.CastSpellRequest(data);
        }
    }

    private void HandleMoveInput(Vector2 vector)
    {
        Debug.Log("Received move input: " + vector);
    }

    private void OnDisable()
    {
        stateMachine.OnStateChanged -= UpdateCurrentState;
        inputHandler.OnMoveInput -= HandleMoveInput;
        inputHandler.OnSpellRequested -= HandleSpellRequested;
    }
    private void UpdateCurrentState(CharacterStateType type)
    {
        currentState = type;
        Debug.Log("State Changed to :" + currentState.ToString());
    }

} 