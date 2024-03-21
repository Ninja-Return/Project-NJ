using UnityEngine;

public class SceneSelectManager : MonoBehaviour
{

    public static SceneSelectManager Instance { get; private set; }

    public string sceneName = SceneList.OfficeMap;

    private void Awake()
    {

        Instance = this;

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
