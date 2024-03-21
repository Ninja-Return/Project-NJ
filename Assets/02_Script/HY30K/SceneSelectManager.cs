using UnityEngine;

public class SceneSelectManager : MonoBehaviour
{

    public static SceneSelectManager Instance { get; private set; }

    public string sceneName;

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        sceneName = SceneList.OfficeMap;

    }

    public void TunnelMap()
    {

        sceneName = SceneList.TunnelMap;

    }

    public void OfficeMap()
    {

        sceneName = SceneList.OfficeMap;

    }

}
