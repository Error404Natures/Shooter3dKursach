using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    //„увствительность мыши
    public float mouseSensitivity = 500f;

    //поворот по ос€м ’ и ”

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // Start is called before the first frame update
    void Start()
    {
        //Block Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //получаем данные от движени€ мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //поворачиваем по оси ’
        xRotation -= mouseY;

        //ограничение поворота
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //тоже самое, но по оси Y
        yRotation += mouseX;

        //примен€ем повороты к намем преобразованию
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
