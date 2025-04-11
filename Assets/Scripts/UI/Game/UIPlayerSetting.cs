using EnumDef;
using UnityEngine;

public class UIPlayerSetting : MonoBehaviour
{
    public UIAbility ability;
    public UIInventory inventory;

    public void Init()
    {
        ability = GetComponentInChildren<UIAbility>(true);
        inventory = GetComponentInChildren<UIInventory>(true);
    }

    private void OnEnable()
    {
        Mng.play.state = GameState.Pause;
    }

    private void OnDisable()
    {
        Mng.play.state = GameState.Playing;
    }
}
