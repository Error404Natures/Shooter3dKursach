using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 10;

    public float stopChasingDistance = 21f;
    public float attackingDistance = 1f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //инициализация
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, player.transform.position);
        
        //проверка должен ли агент прекратить исследование
        if (distanceFromPlayer <= stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }
        //может ли agent атокавать
        if (distanceFromPlayer <=  attackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
        else if (distanceFromPlayer == attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
