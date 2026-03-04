using System;
using System.Collections;
using UnityEngine;
public interface ISpellPhase 
{ 
    bool Validate(GameObject caster);
    void OnInit(GameObject caster, SpellFateToken cancelationToken);
    void OnPhaseStart(GameObject caster);
    IEnumerator OnPhaseUpdate(GameObject caster);
    void OnPhaseEnd(GameObject caster);
}