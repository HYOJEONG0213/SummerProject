using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDetailContainer : MonoBehaviour
{
    [Header("Stage 1")]
    public List<GameObject> stage1_start = new List<GameObject>();
    public List<GameObject> stage1_normal = new List<GameObject>();
    public List<GameObject> stage1_box = new List<GameObject>();
    public List<GameObject> stage1_event = new List<GameObject>();
    public List<GameObject> stage1_secret = new List<GameObject>();
    public List<GameObject> stage1_end = new List<GameObject>();
    [Header("Stage 2")]
    public List<GameObject> stage2_start = new List<GameObject>();
    public List<GameObject> stage2_normal = new List<GameObject>();
    public List<GameObject> stage2_box = new List<GameObject>();
    public List<GameObject> stage2_event = new List<GameObject>();
    public List<GameObject> stage2_secret = new List<GameObject>();
    public List<GameObject> stage2_end = new List<GameObject>();
    [Header("Stage 3")]
    public List<GameObject> stage3_start = new List<GameObject>();
    public List<GameObject> stage3_normal = new List<GameObject>();
    public List<GameObject> stage3_box = new List<GameObject>();
    public List<GameObject> stage3_event = new List<GameObject>();
    public List<GameObject> stage3_secret = new List<GameObject>();
    public List<GameObject> stage3_end = new List<GameObject>();
    [Header("Stage 4")]
    public List<GameObject> stage4_start = new List<GameObject>();
    public List<GameObject> stage4_normal = new List<GameObject>();
    public List<GameObject> stage4_box = new List<GameObject>();
    public List<GameObject> stage4_event = new List<GameObject>();
    public List<GameObject> stage4_secret = new List<GameObject>();
    public List<GameObject> stage4_end = new List<GameObject>();
    [Header("Stage 5")]
    public List<GameObject> stage5_start = new List<GameObject>();
    public List<GameObject> stage5_normal = new List<GameObject>();
    public List<GameObject> stage5_box = new List<GameObject>();
    public List<GameObject> stage5_event = new List<GameObject>();
    public List<GameObject> stage5_secret = new List<GameObject>();
    public List<GameObject> stage5_end = new List<GameObject>();

    public static RoomDetailContainer instance;

    private void Awake() {
        if(instance != null)
            Destroy(instance);
        instance = this;
    }

    public GameObject GetRoom(int stage, RoomType type) {
        switch (type) {
            case RoomType.start:
                switch (stage) {
                    case 1:
                        return stage1_start[GetRandomInt(ref stage1_start)];
                    case 2:
                        return stage2_start[GetRandomInt(ref stage2_start)];
                    case 3:
                        return stage3_start[GetRandomInt(ref stage3_start)];
                    case 4:
                        return stage4_start[GetRandomInt(ref stage4_start)];
                    case 5:
                        return stage5_start[GetRandomInt(ref stage5_start)];
                }
                break;
            case RoomType.end:
            case RoomType.boss:
                switch (stage) {
                    case 1:
                        return stage1_end[GetRandomInt(ref stage1_end)];
                    case 2:
                        return stage2_end[GetRandomInt(ref stage2_end)];
                    case 3:
                        return stage3_end[GetRandomInt(ref stage3_end)];
                    case 4:
                        return stage4_end[GetRandomInt(ref stage4_end)];
                    case 5:
                        return stage5_end[GetRandomInt(ref stage5_end)];
                }
                break;
            case RoomType.secret:
                switch (stage) {
                    case 1:
                        return stage1_secret[GetRandomInt(ref stage1_secret)];
                    case 2:
                        return stage2_secret[GetRandomInt(ref stage2_secret)];
                    case 3:
                        return stage3_secret[GetRandomInt(ref stage3_secret)];
                    case 4:
                        return stage4_secret[GetRandomInt(ref stage4_secret)];
                    case 5:
                        return stage5_secret[GetRandomInt(ref stage5_secret)];
                }
                break;
            case RoomType.box:
                switch (stage) {
                    case 1:
                        return stage1_box[GetRandomInt(ref stage1_box)];
                    case 2:
                        return stage2_box[GetRandomInt(ref stage2_box)];
                    case 3:
                        return stage3_box[GetRandomInt(ref stage3_box)];
                    case 4:
                        return stage4_box[GetRandomInt(ref stage4_box)];
                    case 5:
                        return stage5_box[GetRandomInt(ref stage5_box)];
                }
                break;
            case RoomType.normal:
                switch (stage) {
                    case 1:
                        return stage1_normal[GetRandomInt(ref stage1_normal)];
                    case 2:          
                        return stage2_normal[GetRandomInt(ref stage1_normal)];
                    case 3:          
                        return stage3_normal[GetRandomInt(ref stage1_normal)];
                    case 4:         
                        return stage4_normal[GetRandomInt(ref stage1_normal)];
                    case 5:         
                        return stage5_normal[GetRandomInt(ref stage1_normal)];
                }
                break;
            case RoomType._event:
                switch (stage) {
                    case 1:
                        return stage1_event[GetRandomInt(ref stage1_event)];
                    case 2:
                        return stage2_event[GetRandomInt(ref stage2_event)];
                    case 3:
                        return stage3_event[GetRandomInt(ref stage3_event)];
                    case 4:
                        return stage4_event[GetRandomInt(ref stage4_event)];
                    case 5:
                        return stage5_event[GetRandomInt(ref stage5_event)];
                }
                break;
        }
        Debug.LogError($"号 持失 神嫌{stage} / {type.ToString()}");
        return null;
    }

    public int GetRandomInt(ref List<GameObject> pool) {
        return Random.Range(0,pool.Count);
    }
}
