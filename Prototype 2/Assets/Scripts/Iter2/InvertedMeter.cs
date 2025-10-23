using UnityEngine;
using UnityEngine.UI;

public class InvertedMeter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;

    [Header("Stats")]
    [SerializeField] private float maxMP = 100f;
    [SerializeField] private float decayPerSecond = 10f;
    [SerializeField] private float decayDelaySeconds = 0.5f;

    public float CurrentMP { get; private set; }
    public bool Frozen {  get; private set; }

    private float decayBlockedUntil;

    void Awake()
    {
        CurrentMP = 0;
        Frozen = false;
        UpdateUI();
    }

    void Update()
    {
        Decay();
    }

    public void Regen(float amount)
    {
        CurrentMP = Mathf.Clamp(CurrentMP + amount, 0f, maxMP);
        decayBlockedUntil = Time.time + decayDelaySeconds;
        UpdateUI();
        CheckFrozen();
    }

    private void Decay()
    {
        if (Time.time >= decayBlockedUntil && CurrentMP > 0)
        {
            CurrentMP = Mathf.Clamp(CurrentMP - (decayPerSecond * Time.deltaTime), 0f, maxMP);
            UpdateUI();
        }
        CheckFrozen();
    }

    void UpdateUI()
    {
        if (!fillImage) return;
        float pct = (maxMP <= 0f) ? 0f : CurrentMP / maxMP;
        fillImage.fillAmount = pct;
    }

    void CheckFrozen()
    {
        if (CurrentMP > 0 == Frozen)
        {
            Frozen = !Frozen;
        }
    }
}