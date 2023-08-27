using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// 이 스크립트는 인벤토리 UI가 켜질 때 켜지며 그와 동시에 Weapon 스크립트를 끈다.
public class WeaponDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    private Vector3 startPosition;
    private Vector2 previousPosition;
    private Character character;
    private float dragSpeed = 0.05f;
    private EmptySlotDrop curSlot;

    private void Awake()
    {
      character = GetComponent<Character>();
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

   
    private void OnDisable() // 인벤토리 UI를 끄면 이 스크립트가 꺼지는데, 그 동시에 꺼놨던 Weapon 스크립트를 키고 박스 콜라이더를 끈다.(인벤토리 UI를 키지 않을 때는 콜라이더를 키면 안됨)
    {
        gameObject.GetComponent<Weapon>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData) // 처음 시작한 위치랑, 이전 위치를 드래그 시작된 곳으로 잡는다.
    {
        Debug.Log("OnBeginDrag");
        previousPosition = eventData.position;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData) // 드래그하면서 마우스 커서 위치랑 같은 곳에 두기 => 지금 제대로 실행이 안됨. 고쳐야됨.
    {
        transform.position += (Vector3)(eventData.position - previousPosition) * dragSpeed;
        previousPosition = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        
        if(eventData.pointerEnter == null) //만약 드래그가 끝났을 때 null이면 원래 위치로 복귀
        {
            gameObject.transform.position = startPosition;
        }
        else{

            if (eventData.pointerEnter.tag != "weapon" && eventData.pointerEnter.tag != "Slot") // 만약 드래그가 끝났는데, 무기도 빈슬롯도 아니면 원위치로 복귀
            {
                gameObject.transform.position = startPosition;
            }
           
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 무기끼리 위치를 바꾸고 changeWeapon을 통해 실제 리스트와 Hierarchy에서도 바꾼다. (무기만 드래그 드랍이 가능하므로 무기만 떨어진다고 상정함)
        GameObject dropObject = eventData.pointerDrag;
        dropObject.transform.position = transform.position;  
        transform.position = dropObject.GetComponent<WeaponDragDrop>().startPosition;
        //character.changeWeapon(dropObject, gameObject, );
        Debug.Log("dRop");
    }

    public Vector3 getStartPosition() // 다른 클래스에서 startPosition을 얻을 수 있다.
    {
        return startPosition;
    }
    public void setCurSlot(EmptySlotDrop slot)//도착한 슬롯을 현재 슬롯을 설정한다.
    {
        curSlot = slot;
    }
    public void setPreviousSlotFree() // 다른 빈 슬롯으로 움직이기에 이전에 있던 슬롯의 isOccupied를 false로 만든다.
    {
        curSlot.setIsOccupied(false);
    }

}
