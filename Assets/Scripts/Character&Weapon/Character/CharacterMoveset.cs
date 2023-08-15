using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMoveset : MonoBehaviour
{
    private Character character;
    private CharacterController controller;
    private Animator animator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce; // 점프력
    private float velocityY; // Y축 속도
    private float velocityX; // X축 속도


    [SerializeField] private float dashSpeed; // 대시 속도
    [SerializeField] private float dashCooltime; // 대시 쿨타임
    private bool isDash; // 대시를 하고 있는지 보여주는 수치
    [SerializeField] private float dashDuration; // 대시 지속시간
    private float dashTimer; // 대시가 지금까지 얼마나 지속되었는지 보여주는 시간
    private float leftDashTime; // 대시가 가능한 때까지 남은 시간

    private const float gravitationalAcceleration = 9.81f; // 중력가속도
    private Vector3 cubeSize; // 바닥과의 충돌을 감지하는 큐브의 크기
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    
        velocityY = 0;
        cubeSize = new Vector3(controller.radius * 2, 0.2f, 1f);
        leftDashTime = 0;
        isDash = false;
        
    }
 
   

    public void move()
    {
        //Debug.Log(leftDashTime);
        // 대시 쿨타임을 게속 줄임
        leftDashTime -= Time.deltaTime;

        // 입력
        float inputX = Input.GetAxis("Horizontal");

        //그냥 움직임
        velocityX = inputX * movementSpeed;
      
       
        // 대시기능
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash && leftDashTime <= 0)
        {
            isDash = true;
            dashTimer = 0f;
        }

        if (isDash) //대시를 하고 있음
        {
            // 대시하는 동안 계속 시간을 잼 => 이 시간이 dashDuration을 넘으면 더이상 대시를 안함
            dashTimer += Time.deltaTime;
            // 바라보는 방향에 따라 대시의 방향 바꿈
            if(transform.right == Vector3.right)
            {
                velocityX = dashSpeed;
            }
            else
            {
                velocityX = -dashSpeed;
            }
            // 위에서 말했듯이 대시를 지속하고 있는 시간이 dashDuration을 넘으면 dash를 끝냄. 그리고 쿨타임을 돌림
            if(dashTimer >= dashDuration)
            {
                isDash = false;
                leftDashTime = dashCooltime;
            }
        }

        // 캐릭터의 속도를 inputX로 정하고 그 속도를 캐릭터에게 적용 
        controller.Move(new Vector3(velocityX * Time.deltaTime, 0, 0));

        if (Mathf.Abs(velocityX) > 0.2)
        {
            animator.SetBool("isRun", true);
            if(velocityX > 0)
            {
                transform.right = new Vector3(1, 0 ,0);
            }
            else if(velocityX < 0)
            {
                transform.right = new Vector3(-1, 0, 0);
            }
           
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    public void jumpWithGravity()
    {
        //Debug.Log(velocityY);
        
        // 박스 캐스트와 충돌한 콜라이더까지의 거리가 0.2보다 크면 점프로 인식
        if (isGrounded() > 0.2f)
        {
            animator.SetBool("isJump", true);
            velocityY -= gravitationalAcceleration * Time.deltaTime * 0.1f;
        }
        else 
        {
            animator.SetBool("isJump", false);
           
            // 점프하고 착지하면 velocityY가 음수로 되있음 -> 음수면 0으로 만들기
            if(velocityY < 0)
            {
                velocityY = 0;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocityY = jumpForce;
            }

        }

        // 대시를 하고 있다면 중력의 영향을 안받음
        if (isDash)
        {
            velocityY = 0;
        }

        controller.Move(new Vector3(0, velocityY, 0));
    }


    public GameObject interaction(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "consumable":
                return collider.gameObject;
            case "weapon":

                break;
        }
        return null;
    }

    public float isGrounded()
    {
        RaycastHit hit;
        Physics.BoxCast(transform.position, cubeSize / 2, Vector3.down, out hit, transform.rotation, 10f );
        
        // 충돌한 콜라이더가 없다면 => 점프로 인식 (100이면 위에 있는 조건문 때문에 점프로 인식함)
        if(hit.collider == null)
        {
            return 100f;
        }
        //충돌한 콜라이더가 consumable 또는 weapon 이면 => 점프로 인식
        else if ( hit.collider.gameObject.tag == "consumable" || hit.collider.gameObject.tag == "weapon") 
        {
            return 100f;
        }
        else // 충돌한 콜라이더가 있다면 => 콜라이더와의 거리 리턴
        {
            
            return hit.distance;
        }
         

    }

    public float getJumpForce()
    {
        return jumpForce;
    }
    
   

}
