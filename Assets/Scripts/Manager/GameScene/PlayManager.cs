using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using Unity.VisualScripting;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    static public PlayManager Instance;

    public Player kPlayer;
    public DialogueData kPrologue;
    public List<Spawner> kSpawnerList = new List<Spawner>();

    [HideInInspector]
    public GameState state;

    void Awake()
    {
        Instance = this;
        
        Mng.sound.PlayBGM(BGMType.Main);
    }

    void Start()
    {
        Log.Play("되나?");
        Log.Server("되네?");
        
        kPlayer?.gameObject.SetActive(true);
        StartPrologue();
    }

    void StartPrologue()
    {
        state = GameState.Event;
        Mng.canvas.dialogue.StartDialogue(kPrologue, kPlayer.transform,() =>
        {
            Mng.canvas.dialogue.EndDialogue(GameState.Playing);
            foreach (var spawner in kSpawnerList)
            {
                spawner.gameObject.SetActive(true);
            }
        });
    }

    private GameState lastState;
    void Update()
    {
        if (lastState != state)
        {
//            Debug.Log($"{lastState} -> {state}");
            lastState = state;
        }
        
        TestFunc();
    }

    void TestFunc()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            UIMessageBox.Open("경고", "프로그램을 제대로 하지 않으면 게임이 서비스 되지 않아요.", () => Debug.Log("확인 눌럿다규!"));
        }
    }

    public void MonsterHunt()
    {
        //MonsterData monData = MonststerModle.data;
        MonsterData monData;
        //monData.dropRate
        // if (true)
        // {
        //     var itemData = Mng.table.FindItemData(monData.dropItem);
        //     Mng.data.inventory.Add(itemData);
        // }
        //
        // if(해당 아이템 타입을 입고 있냐 == false)
        //     Mng.data.equp(itemData)
            
    }
}
