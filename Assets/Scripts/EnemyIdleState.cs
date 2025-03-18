using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f;

    Transform player;

    public float detectionAreaRadius = 18f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //патрулирование
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPotroling", true);
        }

        //переходим в режим преследования если игрок в радиусе обнаружения
        float distanceFromPlyer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlyer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }

    }
}
