using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class CHSceneManager : MonoBehaviour
{
    public CanvasGroup Fade_img;
    float fadeDuration = 2; //암전되는 시간

    public static CHSceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static CHSceneManager instance;

    private void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade_img.DOFade(0, fadeDuration)
        .OnStart(() => {
            Loading.SetActive(false);
        })
        .OnComplete(() => {
            Fade_img.blocksRaycasts = false;
        });
    }

    public void ChangeScene(string changeScene)
    {
        Fade_img.alpha = 1f;
        Fade_img.blocksRaycasts = true;
        StartCoroutine("LoadScene", changeScene);

        //Fade_img.DOFade(1, 0.01f)
        //.OnStart(() =>
        //{
        //    Fade_img.blocksRaycasts = true;
        //})
        //.OnComplete(() =>
        //{
        //    StartCoroutine("LoadScene", changeScene); /// 씬 로드 코루틴 실행 ///
        //});
    }
    public GameObject Loading;
    public TextMeshProUGUI Loading_text; //퍼센트 표시할 텍스트

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //로딩 화면을 띄움

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 30)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 30) past_time = 0;
            }
            Loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }
}
