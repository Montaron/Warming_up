using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class InputHandler : MonoBehaviour
{
    [SerializeField] private List<SpellKeybind> keybinds = new();
    private Dictionary<KeyCode, Spell_data> keybindMap;
    private Vector2 lastInput;
    public event Action<Vector2> OnMoveInput;
    public event Action<Spell_data> OnSpellRequested;

    void Awake()
    {
        keybindMap = keybinds.ToDictionary(k => k.key, k => k.spellData);
    }
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (((moveInput != lastInput) && moveInput == Vector2.zero) || moveInput.magnitude > 0.1f)
        {
            OnMoveInput?.Invoke(moveInput);
            lastInput = moveInput;
        }

        foreach (var (key, spell) in keybindMap)
        {
            if (Input.GetKeyDown(key))
            {
                OnSpellRequested?.Invoke(spell);
            }
        }
    }
}
[Serializable]
public class SpellKeybind
{
    public KeyCode key;
    public Spell_data spellData;
}