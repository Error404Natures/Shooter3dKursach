using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class PickupAttachment : MonoBehaviour
{
    // ��� ������, ������� ����� ���������
    public WeaponAttachment.AttachmentType attachmentType;

    // ������ �� ������ ���������� �������
    public WeaponAttachment weaponAttachment;

    // ������� ��� ������� ������
    public KeyCode pickupKey = KeyCode.T; // ������� "E" �� ���������

    void Update()
    {
        // ��������� ������� �������
        if (Input.GetKeyDown(pickupKey))
        {
            // ������������� ����� �� ������
            weaponAttachment.SetAttachment(attachmentType);

            // ������� ������ ������ �� �����
            Destroy(gameObject);
        }
    }
}


