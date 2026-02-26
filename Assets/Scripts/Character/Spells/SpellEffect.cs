using UnityEngine;
public abstract class SpellEffect : ScriptableObject
{ 
    public virtual bool Validate(GameObject caster) => true;
    public virtual void OnPhaseStart(GameObject caster) { }
    public  virtual void OnPhaseUpdate(GameObject caster, float deltaTime) { }
    public virtual void OnPhaseEnd(GameObject caster) { }
}