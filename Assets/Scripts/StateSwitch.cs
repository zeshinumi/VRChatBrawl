using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSwitch : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
			Character chr = animator.GetComponent<Character>();
			chr.mob.StateSwitch(stateInfo);
			if(stateInfo.IsName("Idle")) {
				chr.IsIdle();
			} else if(stateInfo.IsName("Land")) {
				chr.mob.isGrounded = true;
				chr.isIdle = false;
			} else if(stateInfo.IsName("Jump")) {
				chr.mob.isGrounded = false;
				chr.isIdle = false;
			} else {
				chr.isIdle = false;
			}
		}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
			Character chr = animator.GetComponent<Character>();
			if(stateInfo.IsName("LoadAvatar")) {
				chr.IsLoading = false;
			}
	}

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
