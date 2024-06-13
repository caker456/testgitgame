using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveController : MonoBehaviour
{
    [Header("이동및점프")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    Animator anim;
    Vector3 moveDir;
    float veticalVelocity = 0f;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    
    [SerializeField] float groundCheckLength;
    [SerializeField] bool isGround;//확인하는용도
    [Header("환경설정")]
    [SerializeField] bool showGroundCheck;
    [SerializeField] Color colorGroundCheck;
    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
       
  
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        moving();
    }
    private void checkGrounded()
    {
        if(gameObject.CompareTag("Player") == true)
        {

        }
        RaycastHit2D hit =
        Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));
        if (hit)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        
    }
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal")*moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }
}
