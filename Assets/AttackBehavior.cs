using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class AttackColliderTimelineInfo {
    public HumanBodyBones bone;
    public float colliderStartTimeStamp = 0.2f;
    public float colliderEndTimeStamp = 0.3f;
}
public class AttackBehavior : StateMachineBehaviour
{
    public AttackColliderTimelineInfo[] attackColliderTimelineInfos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var timeline in attackColliderTimelineInfos)
        {
            if (stateInfo.normalizedTime > timeline.colliderStartTimeStamp && stateInfo.normalizedTime < timeline.colliderEndTimeStamp)
                animator.GetBoneTransform(timeline.bone).GetComponent<SphereCollider>().enabled = true;
            else
                animator.GetBoneTransform(timeline.bone).GetComponent<SphereCollider>().enabled = false;
        }
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
