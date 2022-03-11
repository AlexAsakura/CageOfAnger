using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public Animator animator;
    protected Rigidbody rigidbody;
    protected CapsuleCollider capsule;
    protected Vector3 moveDir;
    [Range(0.1f,10f)]
    public float speed = 1f; 
    [Range(0.1f,10f)]
    public float freq = 1f;
    [Range(.1f, .5f)]
    public float amplitude = 1f;
    public float movementSpeed = 1f;
    public float phase = 1f;

    public float rotSpeed = 10f;
    float looksideX = 1;
    float looksideZ = 1;
    AnimatorStateInfo StateInfo;
    float walkSpeed = 1f;
    // Start is called before the first frame update
    protected void GetCommonComponents()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    protected void FighterUpdate()
    {
        StateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (StateInfo.IsName("Knocked Out"))
            return;
        float moving = Mathf.Abs(moveDir.x) + Mathf.Abs(moveDir.z);
        animator.SetFloat("Forward", moving, 0.2f, Time.deltaTime);
        if (StateInfo.IsName("Grounded.FightStanceGrounded") || StateInfo.IsName("Grounded.FightStanceGroundedCloser"))
        {
            animator.applyRootMotion = false;
            float sinusoidalSpeed = speed + Mathf.Sin(Time.time * freq + phase) * amplitude;
            if (moving > 10e-3f)
                walkSpeed = Mathf.Lerp(walkSpeed, 1f, Time.deltaTime * 20f);//merge
            else
                walkSpeed = Mathf.Lerp(walkSpeed, 0f, Time.deltaTime * 20f);//sta pe loc

            rigidbody.velocity = moveDir * sinusoidalSpeed * walkSpeed * movementSpeed;
        }
        else {//celelalte stateuri au root motion 
            animator.applyRootMotion = true;
            rigidbody.velocity = animator.deltaPosition / Time.deltaTime;
        }

    }

    protected void ApplyRootRotation(Vector3 lookDir)
    {
        if (lookDir.magnitude < 10e-3f || StateInfo.IsName("Knocked Out"))
            return;
        transform.forward = Vector3.Slerp(transform.forward, lookDir.normalized, Time.deltaTime * rotSpeed);
    }
}
