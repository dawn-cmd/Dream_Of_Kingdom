using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHP;
    protected Animator animator;
    protected virtual void Awake() {
        animator = GetComponent<Animator>();
    }
}
