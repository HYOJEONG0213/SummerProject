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
    [SerializeField] private float jumpForce; // ������
    private float velocityY; // Y�� �ӵ�
    private float velocityX; // X�� �ӵ�


    [SerializeField] private float dashSpeed; // ��� �ӵ�
    [SerializeField] private float dashCooltime; // ��� ��Ÿ��
    private bool isDash; // ��ø� �ϰ� �ִ��� �����ִ� ��ġ
    [SerializeField] private float dashDuration; // ��� ���ӽð�
    private float dashTimer; // ��ð� ���ݱ��� �󸶳� ���ӵǾ����� �����ִ� �ð�
    private float leftDashTime; // ��ð� ������ ������ ���� �ð�

    private const float gravitationalAcceleration = 9.81f; // �߷°��ӵ�
    private Vector3 cubeSize; // �ٴڰ��� �浹�� �����ϴ� ť���� ũ��
    

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
        // ��� ��Ÿ���� �Լ� ����
        leftDashTime -= Time.deltaTime;

        // �Է�
        float inputX = Input.GetAxis("Horizontal");

        //�׳� ������
        velocityX = inputX * movementSpeed;
      
       
        // ��ñ��
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash && leftDashTime <= 0)
        {
            isDash = true;
            dashTimer = 0f;
        }

        if (isDash) //��ø� �ϰ� ����
        {
            // ����ϴ� ���� ��� �ð��� �� => �� �ð��� dashDuration�� ������ ���̻� ��ø� ����
            dashTimer += Time.deltaTime;
            // �ٶ󺸴� ���⿡ ���� ����� ���� �ٲ�
            if(transform.right == Vector3.right)
            {
                velocityX = dashSpeed;
            }
            else
            {
                velocityX = -dashSpeed;
            }
            // ������ ���ߵ��� ��ø� �����ϰ� �ִ� �ð��� dashDuration�� ������ dash�� ����. �׸��� ��Ÿ���� ����
            if(dashTimer >= dashDuration)
            {
                isDash = false;
                leftDashTime = dashCooltime;
            }
        }

        // ĳ������ �ӵ��� inputX�� ���ϰ� �� �ӵ��� ĳ���Ϳ��� ���� 
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
        
        // �ڽ� ĳ��Ʈ�� �浹�� �ݶ��̴������� �Ÿ��� 0.2���� ũ�� ������ �ν�
        if (isGrounded() > 0.2f)
        {
            animator.SetBool("isJump", true);
            velocityY -= gravitationalAcceleration * Time.deltaTime * 0.1f;
        }
        else 
        {
            animator.SetBool("isJump", false);
           
            // �����ϰ� �����ϸ� velocityY�� ������ ������ -> ������ 0���� �����
            if(velocityY < 0)
            {
                velocityY = 0;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocityY = jumpForce;
            }

        }

        // ��ø� �ϰ� �ִٸ� �߷��� ������ �ȹ���
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
                break;
            case "weapon":
                
                break;
        }

    }
    public float isGrounded()
    {
        RaycastHit hit;
        Physics.BoxCast(transform.position, cubeSize / 2, Vector3.down, out hit, transform.rotation, 10f );
        
        // �浹�� �ݶ��̴��� ���ٸ� => ������ �ν� (100�̸� ���� �ִ� ���ǹ� ������ ������ �ν���)
        if(hit.collider == null)
        {
            return 100f;
        }
        else // �浹�� �ݶ��̴��� �ִٸ� => �ݶ��̴����� �Ÿ� ����
        {
            return hit.distance;
        }
         

    }

    
   

}
