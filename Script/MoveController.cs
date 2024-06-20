using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    BoxCollider2D box2d;
    Animator anim;
    Camera camMain;
    Vector3 moveDir;
    float veticalVelocity = 0f;
    [Header("이동및점프")]
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [SerializeField] bool isJump;
    [SerializeField] float groundCheckLength;
    [SerializeField] bool isGround;//확인하는용도
    [Header("환경설정")]
    [SerializeField] bool showGroundCheck;
    [SerializeField] Color colorGroundCheck;
    [Header("벽 점프")]
    [SerializeField] bool touchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f;//타이머
    [Header("대시")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashspeed = 20.0f;
    float dashTimer = 0.0f;//타이머
    TrailRenderer dashEffect;
    [SerializeField] private float dashCoolTime = 2f;
    float dashCoolTimer = 0.0f;
    [Header("대시UI")]
    [SerializeField] GameObject objDashCoolTime;
    [SerializeField] Image imgFill;
    [SerializeField] TMP_Text TextCoolTime;


    //대시 이펙트
    [SerializeField] KeyCode dashkey;

    

    void Start()
    {
        camMain = Camera.main;

    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        dashEffect = GetComponent<TrailRenderer>();
        dashEffect.enabled = false;
        initUI();
    }

    // Update is called once per frame
    void Update()
    {
        dash();
        checkGrounded();
        moving();
        checkAim();
        jump();
        checkGravity();
        doAnim();
        checkTimer();
    }
    private void initUI()
    {
        objDashCoolTime.SetActive(false);
        imgFill.fillAmount = 0;
        TextCoolTime.text = "";
    }
    private void dash()
    {
        //대시키코드
        if (dashTimer==0.0f&&dashCoolTimer==0.0f&&(Input.GetKeyDown(KeyCode.LeftShift)
            ||Input.GetKeyDown(KeyCode.F)))
        {
            dashCoolTimer = dashCoolTime;
            dashTimer = dashTime;
            veticalVelocity = 0;
            dashEffect.enabled = true;
/*            if (transform.localScale.x >0)//왼쪽  
            {
                rigid.velocity = new Vector2(-dashspeed, veticalVelocity);
            }
            else//오른쪽
          
            {
                rigid.velocity = new Vector2(dashspeed, veticalVelocity);
            }
*/
//삼중식
            rigid.velocity = new Vector2(transform.localScale.x > 0 ? -dashspeed : dashspeed, veticalVelocity);
        }
    }
    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }
        
    }
    /*    private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = false;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall= true; 
            }
        }*/
    public void TriggerEnter(HitBox.ehitboxtype _type,Collider2D _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            touchWall = true;
        }
    }
    public void TriggerExit(HitBox.ehitboxtype _type, Collider2D _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            touchWall = false;
        }
    }
    // Start is called before the first frame update
    
    private void checkGrounded()
    {
        isGround = false;
        if (veticalVelocity>0f)
        {
            return;
        }
        RaycastHit2D hit =
        //Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));
        Physics2D.BoxCast(box2d.bounds.center, box2d.bounds.size, 0f, Vector2.down, 0.05f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        
    }
    private void checkTimer()
    {
        if (wallJumpTimer>0.0f)
        {
            wallJumpTimer -= Time.deltaTime;
            if(wallJumpTimer < 0.0f)
            {
                wallJumpTimer = 0.0f;
            }
        }
        if (dashTimer>0.0f)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0.0f)
            {
                dashTimer = 0.0f;
                dashEffect.enabled = false;
                dashEffect.Clear();
            }
        }
        if (dashCoolTimer > 0.0f)
        {
            
            if (!objDashCoolTime.activeSelf)
            {
                objDashCoolTime.SetActive(true);
            }
            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer < 0.0f)
            {
                dashCoolTimer = 0.0f;
                objDashCoolTime.SetActive(false);
            }
            imgFill.fillAmount = 1 - dashCoolTimer / dashCoolTime;
        }   TextCoolTime.text = dashCoolTimer.ToString("F1"); 
    }

    private void moving()
    {
        if (wallJumpTimer >0.0f||dashTimer>0.0f)
        {
            return;
        }
        moveDir.x = Input.GetAxisRaw("Horizontal")*moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }
    private void checkAim()
    {
        Vector3 scale = transform.localScale;
        if (moveDir.x < 0 && scale.x !=1.0f)
        {
            scale.x = 1.0f;
            transform.localScale = scale;
        }
        else if (moveDir.x > 0 && scale.x != -1.0f)
        {
            scale.x = -1.0f;
            transform.localScale = scale;
        }
        Vector2 mouseWorldPos = camMain.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos= mouseWorldPos - playerPos;
        Vector3 playerscale = transform.localScale;
        if (fixedPos.x>0&& playerscale.x != -1.0f)
        {
            playerscale.x = -1.0f;
            transform.localScale= playerscale;
        }
        else if (fixedPos.x < 0 && playerscale.x != 1.0f)
        {
            playerscale.x = 1.0f;

        }
        transform.localScale = playerscale;
    }
    private void jump()
    {
        if (isGround&& Input.GetKeyDown(KeyCode.Space))
        {
            
            rigid.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);//지긋이 밀고싶을때
            
        }

        if (isGround == false)
        {
            if(touchWall && moveDir.x!=0f && Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
    }
    private void checkGravity()
    {
        if (dashTimer >0.0f)
        {
            return;
        }
        if (isWallJump)
        {
            isWallJump = false;
            Vector2 dir = rigid.velocity;
            dir.x *= -1f;
            rigid.velocity = dir;
            veticalVelocity = jumpForce * 0.5f;
            wallJumpTimer = wallJumpTime;
        }
        else if (!isGround)
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
