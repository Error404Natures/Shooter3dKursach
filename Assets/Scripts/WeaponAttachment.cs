using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class WeaponAttachment : MonoBehaviour
{

    // ������ ��������� �������
    public enum AttachmentType { Scope, Silencer, Stock };

    // ������ �������� �������
    public GameObject[] attachments;

    // ������� ��������� �����
    private AttachmentType currentAttachment;

    // ������� ��������� ������
    public void SetAttachment(AttachmentType type)
    {
        // ���������� ��� ������
        foreach (GameObject attachment in attachments)
        {
            attachment.SetActive(false);
        }

        // ������������� ��������� �����
        currentAttachment = type;
        attachments[(int)type].SetActive(true);

    }

    // ������� �������� ������
    public void RemoveAttachment()
    {
        // ���������� ��� ������
        foreach (GameObject attachment in attachments)
        {
            attachment.SetActive(false);
        }
    }
}




