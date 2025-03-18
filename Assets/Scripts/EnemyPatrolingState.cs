using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 21f;
    public float patrolSpeed = 2f;

    List<Transform> waypointList = new List<Transform>(); 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�������������
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0;

        //���� �� ������ �����
        GameObject wayPointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in wayPointCluster.transform)
        {
            waypointList.Add(t);
        }

        Vector3 nextPosition = waypointList[Random.Range(0, waypointList.Count)].position;
        agent.SetDestination(nextPosition);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�������� ������ �� agent � ����� ��������
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointList[Random.Range(0, waypointList.Count)].position);
        }
        //������� � ����� ��������
        timer += Time.deltaTime;
        if(timer > patrolingTime)
        {
            animator.SetBool("isPotroling", false);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer <= detectionArea)
        {
            animator.SetBool("isChasing", true);
        }

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //��������� agent
        agent.SetDestination(agent.transform.position);
    }
}
