using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8
//����cs�� ���� ������ cs ��������. �ϴ� ����

public class Monster : MonoBehaviour
{
    public float currentHp = 1;
    public float moveSpeed = 5f;    //���� �̵� �ӵ�
    public bool moveDirection = true;
    public float jumpPower = 10;
    public float atkCoolTime = 3f;
    public float atkCoolTimeCalc = 3f;

    public int sheild = 40;   //���� ����
    public string[] propertyType = { "water", "fire", "holy", "darkness"};
    public int[] propertyValue = { 1, 2, 3, 4 };
    public int exp = 1;
    public int power = 1;   //���� ���ݷ�
    private float playerAttackPower;    //�÷��̾� ���ݷ�
    private float playerHealthPoint;    //�÷��̾� hp

    public bool isHit = false;  //isHit: �÷��̾�� ��Ʈ �Ǿ��°�
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;

    protected Rigidbody rb;
    protected CapsuleCollider capsuleCollider;
    public GameObject hitBoxCollider;
    public Animator Anim;
    public LayerMask layerMask;     //�÷����� ������ ���̾��ũ

    private SpriteRenderer[] sprites;

    protected void Awake()  // �ʿ��� ������Ʈ ��������
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Anim = GetComponent<Animator>();

        StartCoroutine(CalcCoolTime());
        StartCoroutine(ResetCollider());
        sprites = GetComponentsInChildren<SpriteRenderer>();

        PlayerData playerData = PlayerData.Instance;
        playerAttackPower = playerData.Player.GetComponent<Character>().getAttackPower();
    }

    IEnumerator ResetCollider()         // �ݶ��̴� ����
                                        // TakeDemage�� ���� ���Ͱ� �÷��̾�� ��Ʈ�� �� ��Ʈ�ڽ��� off�� �Ǵµ�, �̰� �ٽ� ���ִ� ����
    {
        while (true)
        {
            yield return null;
            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.5f);
                hitBoxCollider.SetActive(true);
                isHit = false;
            }
        }
    }
    IEnumerator CalcCoolTime()  //���� ������ ��Ÿ�� ���
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                atkCoolTimeCalc -= Time.deltaTime;
                if (atkCoolTimeCalc <= 0)
                {
                    atkCoolTimeCalc = atkCoolTime;
                    canAtk = true;
                }
            }
        }
    }


    public bool IsPlayingAnim(string AnimName)      //�ִϸ��̼� ���� �Լ�
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
        {
            return true;
        }
        return false;
    }
    public void MyAnimSetTrigger(string AnimName)
    {
        if (!IsPlayingAnim(AnimName))
        {
            Anim.SetTrigger(AnimName);
        }
    }

    protected void MonsterFlip()
    {
        MonsterDirRight = !MonsterDirRight;

        Vector3 thisScale = transform.localScale;
        if (MonsterDirRight)
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
            moveDirection = false;
        }
        else
        {
            thisScale.x = Mathf.Abs(thisScale.x);
            moveDirection = true;
        }
        transform.localScale = thisScale;
        rb.velocity = Vector2.zero;
    }

    protected bool IsPlayerDir()    //���Ͱ� �÷��̾� �������� ���ϰ� �ִ��� üũ�ϴ� �Լ�
    {
        if (PlayerData.Instance.Player != null)
        {
            if (transform.position.x < PlayerData.Instance.Player.transform.position.x ? MonsterDirRight : !MonsterDirRight)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    protected void GroundCheck()    //�÷��̾ �ٴ��� üũ�ϴ� �Լ�
    {
        float capsuleHeight = capsuleCollider.height * 0.5f - capsuleCollider.radius; // ĸ�� ������ ��
        Vector3 capsuleBottomCenter = transform.position - Vector3.up * capsuleHeight;

        if (Physics.CheckCapsule(capsuleBottomCenter, capsuleBottomCenter + Vector3.up * capsuleHeight * 2, capsuleCollider.radius, layerMask))
        //if (Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.size, 0, Vector2.down, 0.05f, layerMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }


    public void TakeDamage(float dam)     //�÷��̾����׼� �������� �Ծ����� hp ��� � �ִϸ��̼���?
    {
        currentHp -= dam;   //������ �ְ�
        isHit = true;

        if (currentHp <= 0) //����
        {
            Debug.Log("Monster Dead");
            Destroy(gameObject);
        }
        else    //�˹� ��Ű��
        {
            MyAnimSetTrigger("idle");
            rb.velocity = Vector3.zero;
            if(transform.position.x > PlayerData.Instance.Player.transform.position.x)
            {
                rb.velocity = new Vector3(10f, 0, 0);
            }
            else
            {
                rb.velocity = new Vector3(-10f, 0, 0);
            }
        }
        hitBoxCollider.SetActive(false);    // ���Ͱ� �÷��̾�� ��Ʈ�� �� ��Ʈ�ڽ��� off��Ű��
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if ( collision.transform.CompareTag ("PlayerWeapon"))
        {
            Debug.Log("���� ���� ����");
            TakeDamage (playerAttackPower);
            //StartCoroutine(FlashSprites()); //���� �����Ÿ��� ȿ��
        }

        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            //MonsterFlip();
            //Debug.Log("���Ͱ� �÷��̾�� �ε��ƽ��ϴ�!");
        }

        if (collision.transform.CompareTag("Monster"))
        {
            Monster monsterComponent = collision.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                MonsterFlip();
                Debug.Log("���ͳ��� �ε��ƽ��ϴ�!");
            }
        }
        if (collision.transform.CompareTag("Player"))
        {
            PlayerData playerData = PlayerData.Instance;
            playerHealthPoint = playerData.Player.GetComponent<Character>().getHealthPoint();   //�÷��̾� hp �����µ�

            playerHealthPoint -= power;     //ü�� ���� ����ŭ ���
            playerData.Player.GetComponent<Character>().healthPoint = playerHealthPoint;
            Debug.Log("���� �� �÷��̾� hp: "+ playerHealthPoint);
        }


        }

    private IEnumerator FlashSprites()
    {
        foreach (SpriteRenderer SR in sprites)
        {
            SR.color = new Color(1f, 1f, 1f, 0.4f); //���� ������ �ѵ��� �����Ÿ���
        }
        yield return new WaitForSeconds(1f);

        foreach (SpriteRenderer SR in sprites)
        {
            SR.color = new Color(1f, 1f, 1f, 1f);
        }
    }

        public float getAttackPower(int monsterIndex) // ĳ������ ���ݷ��� �����ϴ� �Լ�
    {
        return power;
    }
}

