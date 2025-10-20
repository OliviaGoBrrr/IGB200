using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    private RectTransform pointerRect;
    private Image pointerImage;

    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float duration;
    [SerializeField] private float fadeSpeed;

    [SerializeField] public Sequence pointerSequence;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        pointerRect = GetComponent<RectTransform>();
        pointerImage = GetComponent<Image>();
        startPosition = pointerRect.localPosition;

        Debug.Log(pointerImage.name);

        pointerSequence = DOTween.Sequence();

        pointerSequence.Append(pointerImage.DOFade(1, fadeSpeed))
            .Append(pointerRect.DOAnchorPos(endPosition, duration))
            .Append(pointerImage.DOFade(0, fadeSpeed))
            .Append(pointerRect.DOAnchorPos(startPosition, 0.5f));

        pointerSequence.SetLoops(-1);

        pointerSequence.Pause();
    }
}
