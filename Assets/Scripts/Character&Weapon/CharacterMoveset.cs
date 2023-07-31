using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMoveset : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxJumpNum; // 최대 점프 횟수
    [SerializeField] private float jumpPower; // 점프력
    private int curJumpNum; // 남은 점프 횟수
    [SerializeField] private float dashSpeed; // 대시 속도
    private float dashCooltime; // 대시 쿨타임
    

    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        curJumpNum = maxJumpNum;
    }
    public void jump()
    {
        // 점프 - 남은 점프 횟수가 0회 이상이고 스페이스가 눌리면 작동
        if (curJumpNum > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isJump", true);
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                curJumpNum--;
            }
        }
    }

    public void move()
    {
        float inputX = Input.GetAxis("Horizontal");

        // 캐릭터의 속도를 inputX로 정하고 그 속도를 캐릭터에게 적용 
        Vector2 velocity = new Vector2(inputX, rb.velocity.y);
        velocity.x *= movementSpeed;
        rb.velocity = velocity;

        // 캐릭터의 속도가 일정 이하면 애니메이션 변경
        if (Mathf.Abs(rb.velocity.x) < 0.3)
        {
            animator.SetBool("isRun", false);
        }
        else
        {
            animator.SetBool("isRun", true);

        }

        // 캐릭터의 속도에 따라 좌우 반전
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
    }

    public void dash()
    {
        
        // 대시. 속도가 양수면 앞으로 대시, 음수거나 0이면 뒤로 대시
        if(rb.velocity.x > 0)
        {
            Vector2 velocity = new Vector2(Vector2.right.x, rb.velocity.y);
            velocity.x *= dashSpeed;
            rb.velocity = velocity;

        }
        else
        {
            Vector2 velocity = new Vector2(Vector2.left.x, rb.velocity.y);
            velocity.x *= dashSpeed;
            rb.velocity = velocity;
        }

        // curDashCooltime을 대시 쿨타임으로 초기화하고 지나간 시간만큼 계속 curDashCooltime에서 뺀다.  
        // 그래서 curDashCooltime이 0이 될때까지 코루틴은 지속되고 대시는 작동하지 않는다.
        //float startTime = Time.time;
        //for(float curDashCooltime = dashCooltime; curDashCooltime > 0; curDashCooltime -= Time.time - startTime)
        //{
        //    yield return null;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //floor에 닿으면 점프횟수를 충전하고 애니메이션 변경
        if (collision.gameObject.tag == "floor")
        {
            animator.SetBool("isJump", false);
            curJumpNum = maxJumpNum;

        }
    }

}
