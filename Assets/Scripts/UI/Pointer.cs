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

        startPosition = pointerRect.anchoredPosition;

        endPosition = new Vector2(Screen.width / 2 + startPosition.x, startPosition.y);

        Debug.Log(pointerImage.name);

        pointerSequence = DOTween.Sequence();

        pointerSequence.Append(pointerImage.DOFade(opacityMax, fadeSpeed))
            .Append(pointerRect.DOAnchorPos(endPosition, duration))
            .Append(pointerImage.DOFade(0, fadeSpeed))
            .Append(pointerRect.DOAnchorPos(startPosition, 0.5f));

        pointerSequence.SetLoops(-1);

        pointerSequence.Pause();
    }
}
