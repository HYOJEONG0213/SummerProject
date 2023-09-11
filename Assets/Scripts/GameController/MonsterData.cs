//몬스터 정보를 플레이어에게 주기위함이었는데 일단 폐기

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
//    public void InitializeMonsters(GameObject[] monsters)   //혹시모르니 배열 초기화
//    {
//        Monster = monsters;
//    }

//    public float getMonsterAttackPower(int monsterIndex)  //몬스터 공격력 가져오기
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
