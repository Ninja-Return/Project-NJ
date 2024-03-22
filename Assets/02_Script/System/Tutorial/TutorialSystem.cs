using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSystem : NetworkBehaviour
{

    private enum SequenceType
    {

        Show,
        Text,
        Relase, 
        ShowObject,
        Delay,
        OffObject

    }

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

    [System.Serializable]
    private class TutorialGameObject
    {

        public string key;
        public GameObject gameObject;

    }

    [System.Serializable]
    private class TutorialSequence
    {

        public SequenceType seqType;
        public string commend;

    }

    [System.Serializable]
    private class TutorialSequenceObject
    {

        public string sequenceKey;
        public TutorialObject tutorialObject;
        public List<TutorialSequence> sequences;

    }

    public static TutorialSystem Instance;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private TutorialPanel panel;
    [SerializeField] private Transform startPos;
    [SerializeField] private List<TutorialText> textList;
    [SerializeField] private List<TutorialGameObject> tutorialObject;
    [Header("Seq"), Space, Space]
    [SerializeField] private List<TutorialSequenceObject> tutorialSequenceObjects;

    private Dictionary<string, string> textContainer = new();
    private Dictionary<string, GameObject> tutorialObjectContainer = new();
    private Dictionary<string, List<TutorialSequence>> tutorialSequenceContainer = new();
    private Dictionary<string, TutorialObject> tutorialSequenceTutorialObject = new();

    private PlayerController player;

    private void Awake()
    {

        Instance = this;
        
        foreach(var item in textList)
        {

            textContainer.Add(item.key, item.text);

        }

        foreach(var item in tutorialObject)
        {

            tutorialObjectContainer.Add(item.key, item.gameObject);

        }

        foreach(var item in tutorialSequenceObjects)
        {

            tutorialSequenceContainer.Add(item.sequenceKey, item.sequences);

        }

        foreach (var item in tutorialSequenceObjects)
        {

            tutorialSequenceTutorialObject.Add(item.sequenceKey, item.tutorialObject);

        }

        player = Instantiate(playerController, startPos.position, Quaternion.identity);
        player.NetworkObject.SpawnAsPlayerObject(OwnerClientId);

    }

    private void Start()
    {
        
        //player = Instantiate(playerController, startPos.position, Quaternion.identity);
        //player.NetworkObject.SpawnAsPlayerObject(OwnerClientId);

        StartSequence("Start");

    }

    public void ShowText(string key)
    {

        if(textContainer.TryGetValue(key, out var text))
        {

            panel.Show(text);

        }

    }

    public void SetUpText(string key)
    {

        if (textContainer.TryGetValue(key, out var text))
        {

            panel.SetUpText(text);

        }

    }

    public void ActiveObject(string key)
    {

        if (tutorialObjectContainer.TryGetValue(key, out var obj))
        {

            obj.transform.TVEffect(obj.transform.localScale.x == 0);

        }

    }

    public void StartSequence(string key)
    {

        if (tutorialSequenceContainer.TryGetValue(key, out var seq))
        {
            TutorialObject tutorialObject = tutorialSequenceTutorialObject[key];
            StartCoroutine(SequenceCo(seq, tutorialObject));

        }

    }

    private IEnumerator SequenceCo(List<TutorialSequence> sequences, TutorialObject obj)
    {

        foreach(var item in sequences)
        {

            switch (item.seqType)
            {
                case SequenceType.Show:
                    ShowText(item.commend);
                    break;
                case SequenceType.Text:
                    SetUpText(item.commend);
                    break;
                case SequenceType.Relase:
                    panel.Release();
                    break;
                case SequenceType.ShowObject:
                    ActiveObject(item.commend);
                    break;
                case SequenceType.Delay:
                    {

                        //float delay = float.Parse(item.commend);
                        //yield return new WaitForSeconds(delay);
                        yield return new WaitWhile(() => { return PressSpace(); });
                    }
                    break;
            }

            //yield return null;

        }
        WinSystem.Instance.WinServerRPC(EnumWinState.Player);
        obj.isTutorialOn = true;

        yield return null;

    }

    private bool PressSpace()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            return false;
        }
        return true;
    }

}
