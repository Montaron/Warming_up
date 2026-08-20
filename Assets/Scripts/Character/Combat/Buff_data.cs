using UnityEngine;

public abstract class Buff_data : ScriptableObject
{
    public string buffName;
    public List<BuffEffect> buffEffect; //Here we can use BuffComponent BuffComponent_HoT, BuffComponent_DoT,... each component needs its own RunTimeObject No I think the BuffRUnetime need to handle all the possibilities
}