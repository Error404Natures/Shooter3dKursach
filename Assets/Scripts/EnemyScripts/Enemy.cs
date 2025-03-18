using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    /*Этот класс управляет поведением врага в игре, 
     * включая его здоровье,
     * анимации и навигацию.
     */

    //переменная здоровья
    //ключевое слово в [SerializeField] нужно
    //что бы мы могли редактировать его через инспектор
    //но в других частях программы оно не видилось
    [SerializeField] private int HP = 100;

    //Ссылка на компонент Animator,
    //который управляет анимациями врага

    private Animator animator;
    //сылка на компонент NavMeshAgent,
    //который используется для навигации врага по сцене.

    private NavMeshAgent navAgent;
    //Указывает мертв ли враг
    public bool isDead;
    
    //вызывается при старте игры
    private void Start()
    {
        //здесь мы получаем ссылки на компоненты
        //которые прикреплены к тому же объекту
        //что и скрипт
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    /*Этот метод вызывается, когда враг получает урон. 
     * Он уменьшает значение HP на переданное количество урона. 
     * Если здоровье становится меньше или равно нулю, 
     * враг считается мертвым,
     * запускается анимация смерти ("Die"),
     * устанавливается флаг isDead в true, и объект врага удаляется из сцены.
     * Если здоровье остается больше нуля, 
     * запускается анимация получения урона ("Damage"), 
     * и флаг isDead сбрасывается в false.
     */
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        //пригодится
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
    //этот метод нужен для визуальных подсказок в редакторе
    private void OnDrawGizmos()
    {
        //Красная зона атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
        
        //Зона преследования
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);

        //зона прекращения преследования
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f);

    }
}
