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

    [SerializeField] private float movementSpeed; // 이동속도
    [SerializeField] private float jumpForce; // 점프력
    private float velocityY; // Y축 속도
    private float velocityX; // X축 속도


    [SerializeField] private float dashSpeed; // 대시 속도
    [SerializeField] private float dashCooltime; // 대시 쿨타임
    private bool isDash; // 대시를 하고 있는지 보여주는 수치
    [SerializeField] private float dashDuration; // 대시 지속시간
    private float dashTimer; // 대시가 시작하고 나서 현재까지 얼마나 지속되었는지 보여주는 시간
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
 
   

    public void move() // 본 함수는 Character 스크립트의 Update()에서 호출되기 때문에 함수 내용은 매 프레임마다 실행된다.
    {
     
        // 대시 쿨타임을 게속 줄임 
        leftDashTime -= Time.deltaTime;

        // 입력
        float inputX = Input.GetAxis("Horizontal");

        //그냥 움직임
        velocityX = inputX * movementSpeed;
      
       
        // 대시기능 - 왼쪽 쉬프트가 눌리고, 대시를 하지 않고, 남은 대시시간이 0이라 대시가 가능할 때
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
        controller.Move(new Vector3(velocityX * Time.deltaTime, 0, transform.position.z));

        if (Mathf.Abs(velocityX) > 0.2) // x축으로 속도의 절대값이 0.2보다 클 때 => 달리고 있을 때
        {
            animator.SetBool("isRun", true); // 달리고 있는 애니메이션 작동
            if(velocityX > 0) // 속도가 양수라 오른쪽으로 달리고 있다면 캐릭터의 방향으로 오른쪽으로 잡음
            {
                transform.right = new Vector3(1, 0 ,0);
            }
            else if(velocityX < 0) //왼쪽으로 달리면 방향을 왼쪽으로 잡음
            {
                transform.right = new Vector3(-1, 0, 0);
            }
           
        }
        else // 달리지 않으면
        {
            animator.SetBool("isRun", false); // 달리는 애니메이션 끔
        }
    }

    public void jumpWithGravity() // 이 함수도 Character 스크립트의 Update()에서 호출됨
    {
        //Debug.Log(velocityY);
        
        // 박스 캐스트(isGroundede 참고)와 충돌한 콜라이더까지의 거리가 0.2보다 크면 점프로 인식
        if (isGrounded() > 0.2f)
        {
            animator.SetBool("isJump", true); // 점프 애니메이션 작동
            velocityY -= gravitationalAcceleration * Time.deltaTime * 0.1f; // 속도를 조정 - 옆에 0.1은 점프 속도를 조절하기 위한 값
        }
        else // 점프가 아니면
        {
            animator.SetBool("isJump", false); // 점프 애니메이션 끔
           
            // 점프하고 착지하면 velocityY가 음수로 되있음 -> 음수면 0으로 만들기
            if(velocityY < 0)
            {
                velocityY = 0;
            }
            if (Input.GetKeyDown(KeyCode.Space))// 점프를 안하고 있고 스페이스를 누르면
            {
                velocityY = jumpForce; //점프함
            }

        }

        // 대시를 하고 있다면 중력의 영향을 안받음
        if (isDash)
        {
            velocityY = 0;
        }

        controller.Move(new Vector3(0, velocityY, transform.position.z));
    }



    public float isGrounded() // 땅에 닿았는지 체크하는 함수
    {
        RaycastHit hit;
        Physics.BoxCast(transform.position, cubeSize / 2, Vector3.down, out hit, transform.rotation, 10f );
        // 박스를 캐릭터의 발치에 박스를 만들어 땅에 닿는지 확인한다. 뭔가 박스에 닿으면 hit에 저장

        // 충돌한 콜라이더가 없다면 => 점프로 인식 (100이면 jumpWithGravity에 있는 조건문 때문에 점프로 인식함)
        if(hit.collider == null)
        {
            return 100f;
        }
        //충돌한 콜라이더가 consumable 또는 weapon 이면 => 점프로 인식, 얘네를 밟고 올라가면 안되기 때문이다.
        else if ( hit.collider.gameObject.tag == "consumable" || hit.collider.gameObject.tag == "weapon") 
        {
            return 100f;
        }
        else // 충돌한 콜라이더가 있다면 => 콜라이더와의 거리 리턴, 이를 통해 점프를 했는지 안했는지 확인할 수 있다.
        {
            
            return hit.distance;
        }
         

    }
 
    
   

}
