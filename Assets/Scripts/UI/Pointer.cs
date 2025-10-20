using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    private RectTransform pointerRect;
    private Image pointerImage;

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float duration;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float opacityMax = 1.0f;

    [SerializeField] public Sequence pointerSequence;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        pointerRect = GetComponent<RectTransform>();
        pointerImage = GetComponent<Image>();

        pointerImage.DOFade(0, 0);

        startPosition = new Vector3(FindFirstObjectByType<Canvas>().GetComponent<RectTransform>().rect.xMin + 130f, pointerRect.anchoredPosition.y, 0);

        endPosition = new Vector2(FindFirstObjectByType<Canvas>().GetComponent<RectTransform>().rect.center.x, startPosition.y / 2);

        pointerSequence = DOTween.Sequence();

        pointerSequence.Append(pointerImage.DOFade(opacityMax, fadeSpeed))
            .Append(pointerRect.DOAnchorPos(endPosition, duration))
            .Append(pointerImage.DOFade(0, fadeSpeed))
            .Append(pointerRect.DOAnchorPos(startPosition, 0.5f));

        pointerSequence.SetLoops(-1);

        pointerSequence.Pause();
    }
}
