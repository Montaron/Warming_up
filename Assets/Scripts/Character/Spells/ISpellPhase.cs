using System.Collections;
using UnityEngine;
public interface ISpellPhase 
{ 
    bool Validate(GameObject caster);
    void OnPhaseStart(GameObject caster);
    IEnumerator OnPhaseUpdate(GameObject caster); 
    void OnPhaseEnd(GameObject caster);
}