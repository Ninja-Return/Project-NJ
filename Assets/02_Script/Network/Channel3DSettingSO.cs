using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;

[CreateAssetMenu(menuName = "SO/Setting/Channel3DSetting")]
public class Channel3DSettingSO : ScriptableObject
{

    [SerializeField, Tooltip("가청거리")] private int audibleDistance = 32;
    [SerializeField, Tooltip("페이드 강도")] private float audioFadeIntensityByDistance = 1.0f;
    [SerializeField, Tooltip("거리별 음성 모델")] private AudioFadeModel audioFadeModel = AudioFadeModel.InverseByDistance;
    [SerializeField, Tooltip("대화 거리")] private int conversationalDistance = 1;

    public Channel3DProperties Get3DSetting()
    {

        return new Channel3DProperties(audibleDistance, conversationalDistance, audioFadeIntensityByDistance, audioFadeModel);

    }

}