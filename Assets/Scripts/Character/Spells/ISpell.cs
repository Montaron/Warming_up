using System;
using System.Collections;
using UnityEngine;
public interface ISpell 
{ 
    bool Validate(GameObject caster, SpellFateToken token);
    void SpellEnd();
    IEnumerator StartSpell(GameObject caster);
}