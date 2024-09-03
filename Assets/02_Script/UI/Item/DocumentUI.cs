using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DocumentUI : MonoBehaviour
{
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private TMP_Text detailText;
    [SerializeField] private TMP_Text pageText;

    private string[] details;
    private int pageCnt;

    public void SetDocument(string subject, string[] details)
    {
        subjectText.text = subject;
        this.details = details;
    }

    public bool NextPage()
    {
        return SetPage(pageCnt + 1);
    }

    private bool SetPage(int nowPage)
    {
        if (nowPage > MaxPage())
        {
            pageCnt = 0;
            return false;
        }

        pageCnt = nowPage;
        pageText.text = $"{nowPage}/{MaxPage()}";
        detailText.text = details[pageCnt - 1];

        return true;
    }

    private int MaxPage()
    {
        return details.Length;
    }
}
