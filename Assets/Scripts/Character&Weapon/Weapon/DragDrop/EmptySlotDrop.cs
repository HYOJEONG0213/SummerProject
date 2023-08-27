using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class EmptySlotDrop : MonoBehaviour, IDropHandler
{
    protected Character character;
    protected bool isOccupied;

   

    private void Awake()
    {
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    public void setIsOccupied(bool value) // 다른 클래스가 isOccupied를 관리할 수 있는 함수
    {
        isOccupied = value;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {

        if (!isOccupied) // 슬롯을 차지한 무기가 없을 때 작동
        {
            //dropObject : 이 슬롯 위에 떨어진 오브젝트, gameObject : 현재 슬롯 오브젝트(빈 슬롯)
            //1. 슬롯 위에 떨어트린 무기를 슬롯 위치로 고정
            //2. 그 무기의 이전에 있던 슬롯의 isOccupied를 false로 설정
            //3. 그 무기의 현재 슬롯을 지금 이 슬롯으로 바꾼다.
            GameObject dropObject = eventData.pointerDrag;
            dropObject.transform.position = transform.position;// 1.
            WeaponDragDrop dropObjectDragDrop = dropObject.GetComponent<WeaponDragDrop>();
            dropObjectDragDrop.setPreviousSlotFree(); // 2. + 현재 무기의 최초 curSlot이 존재하지 않아 함수 작동이 불가능 => 해야할 것 : 인벤토리 UI 들어갈 때 무기에게 curSlot 정해주고 위치 정해야함
            dropObjectDragDrop.setCurSlot(gameObject.GetComponent<EmptyUsingWeaponSlotDrop>());// 3.


        }
    }

}
