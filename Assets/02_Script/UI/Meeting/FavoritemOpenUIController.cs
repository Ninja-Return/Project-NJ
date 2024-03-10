using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FavoritemOpenUIController : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text item_1Text, item_2Text;

    public void Setting(string playerName, List<AttachedItem> items)
    {

        nameText.text = playerName;

        item_1Text.text = items[0].ToString();
        item_2Text.text = items[1].ToString();

        StartCoroutine(Delay());

    }

    private IEnumerator Delay()
    {

        yield return new WaitForSeconds(4f);
        transform.TVEffect(false);

    }

}
