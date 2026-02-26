using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    [SerializeField] private Transform weapon_hand;
    [SerializeField] private GameObject weapon;
    [SerializeField] private SpellPhase chargeSpell;
    private GameObject currentWeapon;

    private CharacterStateMachine stateMachine;
    public CharacterStateType currentState { get; private set; }

    void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogWarning("stateMachine not referenced");
        }
    }
        void Start()
    {
        
        if (weapon != null && weapon_hand != null)
        {
            currentWeapon = Instantiate(weapon, weapon_hand);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
            currentWeapon.transform.localScale = Vector3.one;
        }
    }

    private void UpdateCurrentState(CharacterStateType type)
    {
        currentState = type;
        Debug.Log("State Changed to :" + currentState.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right click detected, casting charge spell.");
            StartCoroutine(chargeSpell.ExecuteSpell(gameObject, result => Debug.Log("Spell execution result: " + result)));
        }
    }

    private void OnEnable()
    {
        stateMachine.OnStateChanged += UpdateCurrentState;
    }
    private void OnDisable()
    {
        stateMachine.OnStateChanged -= UpdateCurrentState;
    }
} 