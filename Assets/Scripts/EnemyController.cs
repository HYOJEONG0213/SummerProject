//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////https://www.youtube.com/watch?v=EFiGwM_jGgo
////��.. MovingObject �� ������ �ϴ� �ϴ� ���

//public class EnemyController : MovingObject
//{
//    public int level; 
//    public int hp;  
//    public int sheild;   //���� ����
//    public int exp;     
//    public int power;   //���� ���ݷ�
//    public int speed;   //������ �̵��ӵ�

//    public int inter_MoveWaitTime;    //���ð�
//    private float current_interMWT;

//    public string attackSound;  //���� �Ҹ�

//    private Vector2 PlayerPos;  //�÷��̾��� ��ǥ��

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
//        current_interMWT -= Time.deltaTime;     //1�ʸ�ŭ 1�� ����

//        if (current_interMWT <= 0)
//        {
//            current_interMWT = inter_MoveWaitTime;

//            RandomDirection();

//            if (base.CheckCollsion()) {     //�����̷��� ���⿡ ���𰡰� �ִٸ�
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
