//�÷��̾ �������� ���ݹ޴� ��ũ��Ʈ.. �ϴ� ����!

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public GameObject hitBoxCollder;
    bool invincible;    //���������� �ƴ��� Ȯ���� ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Monster") || collision.transform.CompareTag("Projectile"))
        {
            if (!invincible)
            {
                StartCoroutine(InvincibleEffect());
                Debug.Log("�÷��̾� hp ����");
            }

            playerMovement.rb.velocity = Vector2.zero;
            playerMovement.isHit = true;
            Invoke("isHitReset", 0.5f); //�÷��̾��� ������ Ǯ���ֱ�
            hitBoxCollder.SetActive(false);

            if (!playerMovement.IsPlayingAnim("Attack"))
            {
                player.Movement.MyAnimSetTrigger("Hurt");
            }
        }
    }

    void isHitReset()
    {
        playerMovement.isHit = false;  
    }

    IEnumerator InvincibleEffect()  //����ȿ�� ����
    {
        invincible = true;  //����
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
*/