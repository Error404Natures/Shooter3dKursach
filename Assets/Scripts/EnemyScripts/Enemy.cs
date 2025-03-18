using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    /*���� ����� ��������� ���������� ����� � ����, 
     * ������� ��� ��������,
     * �������� � ���������.
     */

    //���������� ��������
    //�������� ����� � [SerializeField] �����
    //��� �� �� ����� ������������� ��� ����� ���������
    //�� � ������ ������ ��������� ��� �� ��������
    [SerializeField] private int HP = 100;

    //������ �� ��������� Animator,
    //������� ��������� ���������� �����

    private Animator animator;
    //����� �� ��������� NavMeshAgent,
    //������� ������������ ��� ��������� ����� �� �����.

    private NavMeshAgent navAgent;
    //��������� ����� �� ����
    public bool isDead;
    
    //���������� ��� ������ ����
    private void Start()
    {
        //����� �� �������� ������ �� ����������
        //������� ����������� � ���� �� �������
        //��� � ������
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    /*���� ����� ����������, ����� ���� �������� ����. 
     * �� ��������� �������� HP �� ���������� ���������� �����. 
     * ���� �������� ���������� ������ ��� ����� ����, 
     * ���� ��������� �������,
     * ����������� �������� ������ ("Die"),
     * ��������������� ���� isDead � true, � ������ ����� ��������� �� �����.
     * ���� �������� �������� ������ ����, 
     * ����������� �������� ��������� ����� ("Damage"), 
     * � ���� isDead ������������ � false.
     */
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        //����������
        if (HP <= 0)
        {
            animator.SetTrigger("Die");
            isDead = true;
            Destroy(gameObject);
        }
        else
        {
            isDead = false;
            animator.SetTrigger("Damage");
        }
    }
    //���� ����� ����� ��� ���������� ��������� � ���������
    private void OnDrawGizmos()
    {
        //������� ���� �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
        
        //���� �������������
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        //���� ����������� �������������
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f);

    }
}
