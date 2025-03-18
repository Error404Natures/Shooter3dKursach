using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class PickupAttachment : MonoBehaviour
{
    // Тип обвеса, который можно подобрать
    public WeaponAttachment.AttachmentType attachmentType;

    // Ссылка на скрипт управления оружием
    public WeaponAttachment weaponAttachment;

    // Клавиша для подбора обвеса
    public KeyCode pickupKey = KeyCode.T; // Клавиша "E" по умолчанию

    void Update()
    {
        // Проверяем нажатие клавиши
        if (Input.GetKeyDown(pickupKey))
        {
            // Устанавливаем обвес на оружие
            weaponAttachment.SetAttachment(attachmentType);

            // Удаляем объект обвеса из сцены
            Destroy(gameObject);
        }
    }
}


