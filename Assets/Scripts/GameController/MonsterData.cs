//���� ������ �÷��̾�� �ֱ������̾��µ� �ϴ� ���

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MonsterData : MonoBehaviour
//{
//    public static MonsterData Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = FindObjectOfType<MonsterData>();
//                if (instance == null)
//                {
//                    var instanceContainer = new GameObject("MonsterData");
//                    instance = instanceContainer.AddComponent<MonsterData>();
//                }
//            }
//            return instance;
//        }
//    }
//    private static MonsterData instance;

//    public GameObject[] Monster;
//    private float monsterAttackPower;
//    public void InitializeMonsters(GameObject[] monsters)   //Ȥ�ø𸣴� �迭 �ʱ�ȭ
//    {
//        Monster = monsters;
//    }

//    public float getMonsterAttackPower(int monsterIndex)  //���� ���ݷ� ��������
//    {
//        if (monsterIndex >= 0 && monsterIndex < Monster.Length)
//        {
//            Monster monsterComponent = Monster[monsterIndex].GetComponent<Monster>();
//            if (monsterComponent != null)
//            {
//                return monsterComponent.getAttackPower();
//            }
//        }
//        return 0; 
//    }
//}
