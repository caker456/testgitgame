using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // 두개 이상 온스위치할때 현재보이는 카메라는
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
