using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private float blendPower = 0.2f;
    [SerializeField] private float blendSpeed = 25f;

    [SerializeField] private Transform moveCamera;

    private const string jumpStr = "Jump";
    private const string fallStr = "Fall";
    private const string blendStr = "MotionSpeed";
    private const string atkStr = "Attack";
    private const string atkBlendStr = "WeaponAnimInd";

    private Animator animator;

    private float animationBlend;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetJumpAnim()
    {
        animator.SetTrigger(jumpStr);
    }

    public void OnLandStarted()
    {
        moveCamera.position -= Vector3.up * 0.25f;
    }

    public void OnLandEnded()
    {
        moveCamera.position += Vector3.up * 0.25f;
    }

    public void SetFallAnim(bool isFalling)
    {
        animator.SetBool(fallStr, isFalling);
    }

    public void SetMotionSpeed(float value)
    {
        animationBlend = Mathf.Lerp(animationBlend, value * blendPower, Time.deltaTime * blendSpeed);
        animator.SetFloat(blendStr, animationBlend);
    }

    public void SetAttackAnim(bool isAttacking)
    {
        animator.SetBool(atkStr, isAttacking);
    }

    public void SetWeaponInd(int ind)
    {
        animator.SetFloat(atkBlendStr, ind);
    }
}
