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
    [SerializeField] bool isJump;
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
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        moving();
        jump();
        checkGravity(); 
        doAnim();
    }
    private void checkGrounded()
    {
        isGround = false;
        if (veticalVelocity>0f)
        {
            return;
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
    private void jump()
    {
        if (isGround==true&& Input.GetKeyDown(KeyCode.Space))
        {
            
            rigid.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);//지긋이 밀고싶을때
            
        }
        if (isGround == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)==true)
        {
            isJump = true;
        }
    }
    private void checkGravity()
    {
        if (!isGround)
        {
            veticalVelocity += Physics.gravity.y * Time.deltaTime;

            if (veticalVelocity < -10f)
            {
                veticalVelocity = -10f;
            }
        }
        else if (isJump)
        {
            isJump = false;
            veticalVelocity = jumpForce;
        }
        else if (isGround)
        {
            veticalVelocity = 0;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, veticalVelocity);
    }
    private void doAnim()
    {
        anim.SetInteger("Horizontal",(int)moveDir.x);
        anim.SetBool("isGround",isGround);

    }
}
