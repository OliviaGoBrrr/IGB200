using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private Canvas canvas;

    [SerializeField] private TMP_Text roundText;
    [SerializeField] private string roundsString;
    [SerializeField] private TMP_Text actionsText;
    [SerializeField] private string actionsString;

    private GameManager gameManager;
    

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Start()
    {
        UpdateOnRoundAdvance();
    }

    public void UpdateOnRoundAdvance()
    {
        roundText.SetText(roundsString + " " + gameManager.roundCount);
        actionsText.SetText(gameManager.maxActions + "/" + gameManager.maxActions + " " + actionsString);
    }

    private void OnEnable()
    {
        GameManager.OnRoundAdvanced += UpdateOnRoundAdvance;
    }

    private void OnDisable()
    {
        GameManager.OnRoundAdvanced -= UpdateOnRoundAdvance;
    }
}
