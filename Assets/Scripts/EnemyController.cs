//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////https://www.youtube.com/watch?v=EFiGwM_jGgo
////음.. MovingObject 또 만들어야 하니 일단 폐기

//public class EnemyController : MovingObject
//{
//    public int level; 
//    public int hp;  
//    public int sheild;   //몬스터 방어력
//    public int exp;     
//    public int power;   //몬스터 공격력
//    public int speed;   //몬스터의 이동속도

//    public int inter_MoveWaitTime;    //대기시간
//    private float current_interMWT;

//    public string attackSound;  //공격 소리

//    private Vector2 PlayerPos;  //플레이어의 좌표값

//    private int random_int;
//    private string direction;

//    private Queue<string> queue;


//    void Start()
//    {
//        queue = new Queue<string>();
//        current_interMWT = inter_MoveWaitTime;

//    }

//    void Update()
//    {
//        current_interMWT -= Time.deltaTime;     //1초만큼 1씩 감소

//        if (current_interMWT <= 0)
//        {
//            current_interMWT = inter_MoveWaitTime;

//            RandomDirection();

//            if (base.CheckCollsion()) {     //움직이려는 방향에 무언가가 있다면
//                return;
//            }

//            base.Move(direction);
//        }
//    }
    
//    private void RandomDirection() {
//        Vector2.Set(0, 0, Vector2.z);
//        random_int = Random.Range(0, 4);
//        switch (random_int) {
//            case 0:
//                Vector2.y = 1f;
//                direction = "UP";
//            case 1:
//                Vector2.y = -1f;
//                direction = "DOWN";
//            case 2:
//                Vector2.x = 1f;
//                direction = "RIGHT";
//            case 3:
//                Vector2.x = -1f;
//                direction = "LEFT";
//        }
//    }
//}
