using System.Collections;
using UnityEngine;

public class StoryModeManager : MonoBehaviour
{

    [SerializeField] private AudioSource startSource;



    private IEnumerator WaitStartSourceEnd()
    {

        yield return new WaitUntil(() => startSource.isPlaying);

    }

}
