using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Transform handtr;
    [SerializeField] GameObject objThrowWeapon;
    [SerializeField] Transform trsWeapon;
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
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos;
        float angle = Quaternion.FromToRotation(
            transform.localScale.x< 0?Vector3.right:Vector3.left
            , fixedPos).eulerAngles.z;
        handtr.rotation = Quaternion.Euler(0, 0, angle);
        

    }
    private void checkCreate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            createWeapon();    
        }
    }
    private void createWeapon()
    {
       //GameObject go = Instantiate(objThrowWeapon); 
    }
}
