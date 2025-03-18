using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class WeaponAttachment : MonoBehaviour
{

    // Список доступных обвесов
    public enum AttachmentType { Scope, Silencer, Stock };

    // Список объектов обвесов
    public GameObject[] attachments;

    // Текущий выбранный обвес
    private AttachmentType currentAttachment;

    // Функция установки обвеса
    public void SetAttachment(AttachmentType type)
    {
        // Сбрасываем все обвесы
        foreach (GameObject attachment in attachments)
        {
            attachment.SetActive(false);
        }

        // Устанавливаем выбранный обвес
        currentAttachment = type;
        attachments[(int)type].SetActive(true);

    }

    // Функция удаления обвеса
    public void RemoveAttachment()
    {
        // Сбрасываем все обвесы
        foreach (GameObject attachment in attachments)
        {
            attachment.SetActive(false);
        }
    }
}




