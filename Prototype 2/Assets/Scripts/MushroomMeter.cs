using UnityEngine;
using UnityEngine.UI;

public class MushroomMeter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;

    [Header("Stats")]
    [SerializeField] private float maxMP = 100f;
    [SerializeField] private float regenPerSecond = 10f;
    [SerializeField] private float regenDelaySeconds = 0.5f;

    public float CurrentMP { get; private set; }

    float regenBlockedUntil;

    void Awake()
    {
        CurrentMP = maxMP / 2;      // start halfway
        UpdateUI();
    }

    void Update()
    {
        // auto-regen when not full and delay expired
        if (Time.time >= regenBlockedUntil && CurrentMP < maxMP)
        {
            CurrentMP += regenPerSecond * Time.deltaTime;
            if (CurrentMP > maxMP) CurrentMP = maxMP;
            UpdateUI();
        }
    }

    public bool TryConsume(float amount)
    {
        if (amount <= 0f) return true;
        if (CurrentMP >= amount)
        {
            CurrentMP -= amount;
            regenBlockedUntil = Time.time + regenDelaySeconds;
            UpdateUI();
            return true;
        }
        // not enough MP
        return false;
    }

    public bool CanAfford(float amount) => CurrentMP >= amount;

    public void Add(float amount)
    {
        CurrentMP = Mathf.Clamp(CurrentMP + amount, 0f, maxMP);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!fillImage) return;
        float pct = (maxMP <= 0f) ? 0f : CurrentMP / maxMP;
        fillImage.fillAmount = pct;
    }
}