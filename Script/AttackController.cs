using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // �ΰ� �̻� �½���ġ�Ҷ� ���纸�̴� ī�޶��
        // Camera.current
    }
    void Update()
    {
        checkAim();
    }
    private void checkAim()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
    }
}
