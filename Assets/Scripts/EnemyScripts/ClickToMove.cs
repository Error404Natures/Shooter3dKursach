using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
