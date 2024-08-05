using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public int value;
    public EfffectTargetType targetType;
    public abstract void Execute(CharacterBase from, CharacterBase target);
}
