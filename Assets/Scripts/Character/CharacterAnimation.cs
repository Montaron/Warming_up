using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterManager characterManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterManager = GetComponent<CharacterManager>();
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }
}
public enum SpellAnimationType
{
    AnimationStart,
    AnimationLoop,
    AnimationEnd
}