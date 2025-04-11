using System.Collections;
using TMPro;
using UnityEngine;
using UniRx;

public class UIGold : MonoBehaviour
{
    public TMP_Text kGold;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => Mng.data != null);
        Mng.data.gold.Subscribe(_p => kGold.text = $"{_p:#,##0} G");
    }
}
