using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TutorialSystem : NetworkBehaviour
{

    [System.Serializable]
    private class TutorialPanel
    {

        [SerializeField] private TMP_Text text;
        public Transform trm;

        public void SetUpText(string text)
        {

            this.text.text = text;

        }

        public void Show(string text)
        {

            trm.TVEffect(true);
            SetUpText(text);

        }

        public void Release()
        {

            trm.TVEffect(false);

        }

    }

    [System.Serializable]
    private class TutorialText
    {

        public string key;
        [TextArea] public string text;

    }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private TutorialPanel panel;
    [SerializeField] private Transform startPos;
    [SerializeField] private List<TutorialText> textList;

    private Dictionary<string, string> textContainer = new();

    private PlayerController player;

    private void Awake()
    {
        
        foreach(var item in textList)
        {

            textContainer.Add(item.key, item.text);

        }

    }

    private void Start()
    {
        
        player = Instantiate(playerController, startPos.position, Quaternion.identity);
        player.NetworkObject.SpawnAsPlayerObject(OwnerClientId);

        panel.Show("Start");

    }

    public void ShowText(string key)
    {

        if(textContainer.TryGetValue(key, out var text))
        {

            panel.Show(text);

        }

    }

}
