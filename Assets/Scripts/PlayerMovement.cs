using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    //задаем данные
    public float speed = 12f;
    public float gravity = -9.82f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMasck;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        //проверка на землю
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMasck);
        //сброс скорости
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //получаем и сохран€ем значени€
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //создаем движение по вектору

        Vector3 move = transform.right * x + transform.forward * z;

        //собственно само перемещение игрока
        controller.Move(move * speed * Time.deltaTime);

        //проверка может ли игрок прыгать
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //прыжок
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //короче когда перс падает мы примен€ем силу т€жести
        velocity.y += gravity * Time.deltaTime;

        //¬ыполнение прыжка
        controller.Move(velocity * Time.deltaTime);

        //это пока затычка потом пригодитьс€!!!!!
        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            //используем потом!!!
        }
        else 
        {
            isMoving = false;
            //используем потом!!!
        }

        lastPosition = gameObject.transform.position;
    }
}
