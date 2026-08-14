using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    private CharacterStateMachine stateMachine;
    private CharacterCombat characterCombat;
    private InputHandler inputHandler;
    private CharacterMovement_iso characterMov_iso;
    public CharacterStateType currentState { get; private set; }

    private T FetchComponent<T>() where T : Component
    {
        var component = GetComponent<T>();
        if (component == null)
             Debug.LogWarning($"{typeof(T).Name} not found on {gameObject.name}");
        return component;
    }

    void Awake()
    {
        stateMachine = FetchComponent<CharacterStateMachine>();
        inputHandler = FetchComponent<InputHandler>();
        characterCombat = FetchComponent<CharacterCombat>();
        characterMov_iso = FetchComponent<CharacterMovement_iso>();
    }
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnEnable()
    {
        if (stateMachine != null)
        {
            stateMachine.OnStateChanged += UpdateCurrentState;
        }
        if (inputHandler != null)
        {
            inputHandler.OnMoveInput += HandleMoveInput;
            inputHandler.OnSpellRequested += HandleSpellRequested;
            inputHandler.OnSpellGetKeyUp += HandleSpellGetKeyUp;
        }
        if (characterCombat != null)
        {
            characterCombat.OnSpellEnded += HandleSpellEnded;
            characterCombat.OnCombatStateChange += HandleCombatStateChange; 
        }
    }

    private void HandleCombatStateChange(CharacterStateType type)
    {
        stateMachine.ChangeState(type);
    }

    private void HandleSpellGetKeyUp(Spell_data data)
    {
        characterCombat.HandleSpellRequest(data, InterruptFlag.KeyUp);
    }

    private void HandleSpellEnded(Spell_data data)
    {
        if (characterMov_iso.inputVector != Vector2.zero)
            stateMachine.ChangeState(CharacterStateType.Running);
        else
            stateMachine.ChangeState(CharacterStateType.Iddle);
    }

    private void HandleSpellRequested(Spell_data data)
    {
            characterCombat.HandleSpellRequest(data, InterruptFlag.KeyDown);
    }

    private void HandleMoveInput(Vector2 vector)
    {
        characterMov_iso.SetInput(vector);
        if (vector.magnitude > 0.1f)
        {
            characterCombat.HandleSpellRequest(null, InterruptFlag.Movement);
        }
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
        // Debug.Log("State Changed to :" + currentState.ToString());
    }

}