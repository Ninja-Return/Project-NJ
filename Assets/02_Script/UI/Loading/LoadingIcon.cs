using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingIcon : MonoBehaviour
{
    [SerializeField] private RectTransform[] pieces;
    [SerializeField] private float radius;

    private List<RectTransform> rollPiece = new List<RectTransform>();
    private Vector2 center;

    private float[] value;

    private void Start()
    {
        center = new Vector2(Screen.width / 2f, Screen.height / 2f) + (Vector2)transform.localPosition;
        value = new float[pieces.Length];

        StartCoroutine(PieceCor());
    }

    private void Update()
    {
        if (rollPiece.Count == 0) return;

        for (int i = 0; i < rollPiece.Count; i++)
        {
            Vector2 rectTrs = new Vector2();
            rectTrs.x = center.x + Mathf.Sin(value[i]) * radius;
            rectTrs.y = center.y + Mathf.Cos(value[i]) * radius;

            rollPiece[i].position = rectTrs;
        }
    }

    private void CircleMove(int idx)
    {
        DOTween.To(() => value[idx], x => value[idx] = x, 6.4f, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            value[idx] = 0f;
            CircleMove(idx);
        });
    }

    private IEnumerator PieceCor()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            rollPiece.Add(pieces[i]);
            CircleMove(i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
