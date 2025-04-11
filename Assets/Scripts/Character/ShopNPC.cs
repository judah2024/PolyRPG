using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopNPC : MonoBehaviour
{
    public DialogueData kInteractDialogue;
    public DialogueData kEndDialogue;
    public ShopData kShopData;

    Animator mAnimator;

    private void Awake()
    {
        mAnimator = GetComponentInChildren<Animator>();
    }

    public void OnInteract()
    {
        mAnimator.Play("Talk", 0, 0);

        Mng.canvas.dialogue.StartDialogue(kInteractDialogue, transform, () =>
        {
            Mng.data.OpenShop(kShopData, this);
            Mng.play.state = GameState.Shop;
        });
    }

    public void StartEndDialogue()
    {
        Mng.canvas.dialogue.StartDialogue(kEndDialogue, transform, () =>
        {
            Debug.Log("End Shop NPC");
            Mng.canvas.dialogue.EndDialogue(GameState.Playing);
        });
    }
}
