using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;

[CreateAssetMenu(menuName = "SO/Setting/Channel3DSetting")]
public class Channel3DSettingSO : ScriptableObject
{

    [SerializeField, Tooltip("��û�Ÿ�")] private int audibleDistance = 32;
    [SerializeField, Tooltip("���̵� ����")] private float audioFadeIntensityByDistance = 1.0f;
    [SerializeField, Tooltip("�Ÿ��� ���� ��")] private AudioFadeModel audioFadeModel = AudioFadeModel.InverseByDistance;
    [SerializeField, Tooltip("��ȭ �Ÿ�")] private int conversationalDistance = 1;

    public Channel3DProperties Get3DSetting()
    {

        return new Channel3DProperties(audibleDistance, conversationalDistance, audioFadeIntensityByDistance, audioFadeModel);

    }

}