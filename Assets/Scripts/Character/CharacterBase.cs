using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHP;
    public IntVariable hp;
    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }
    protected Animator animator;
    private bool isDead;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        hp.maxValue = maxHP;
        CurrentHP = MaxHP;
    }
    public virtual void TakeDamage(int damage)
    {
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            //TODO: Die
            isDead = true;
            return;
        }
        CurrentHP -= damage;
        Debug.Log($"{name} took {damage} damage. Current HP: {CurrentHP}");
    }
}
