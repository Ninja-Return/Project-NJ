using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : HandItemRoot
{
    [Header("Document")]
    [SerializeField] private DocumentUI documentPrefab;

    [Header("Text")]
    [SerializeField] private string subject;
    [SerializeField] private string[] details;

    private DocumentUI documentUI;

    private void Start()
    {
        documentUI = FindObjectOfType<DocumentUI>();

        if (documentUI == null)
        {
            documentUI = Instantiate(documentPrefab, Vector3.zero, Quaternion.identity);
        }

        documentUI.SetDocument(subject, details);
        documentUI.gameObject.SetActive(false);
    }

    public override void DoUse()
    {
        bool visable = documentUI.NextPage();

        documentUI.gameObject.SetActive(visable);
        PlayerManager.Instance.localController.Active(!visable);
    }
}
