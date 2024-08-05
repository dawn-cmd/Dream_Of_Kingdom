using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null)
        {
            Debug.Log("No target");
            return;
        }
        switch (targetType)
        {
            case EfffectTargetType.Target:
                target.TakeDamage(value);
                Debug.Log($"Execute {value} Damage");
                break;
            case EfffectTargetType.All:
                foreach (var character in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    character.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}
