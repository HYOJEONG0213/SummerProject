//플레이어가 몬스터한테 공격받는 스크립트.. 일단 유기!

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    public GameObject hitBoxCollder;
    bool invincible;    //무적중인지 아닌지 확인할 변수

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
                Debug.Log("플레이어 hp 깎임");
            }

            playerMovement.rb.velocity = Vector2.zero;
            playerMovement.isHit = true;
            Invoke("isHitReset", 0.5f); //플레이어의 움직임 풀어주기
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

    IEnumerator InvincibleEffect()  //무적효과 관련
    {
        invincible = true;  //무적
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
*/