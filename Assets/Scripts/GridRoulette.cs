using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridRoulette : MonoBehaviour
{
    [Header("Slots (your Slot_* images)")]
    public Image[] slots;              // each Slot is an Image (background)

    [Header("Glow highlight (single)")]
    public RectTransform highlight;    // Image with glow sprite (disabled at start)

    [Header("Background sprite")]
    public Sprite normalBG;            // constant rectangle used by every slot

    [Header("Spin Tuning")]
    public int laps = 3;
    public float minDelay = 0.05f;
    public float maxDelay = 0.22f;

    [Header("Icon Fit")]
    [Range(0.3f, 0.95f)]
    public float iconScalePercent = 0.8f;

    bool isSpinning;
    int currentIndex = -1;
    Image highlightImg;

    void Awake()
    {
       
        if (highlight)
        {
            highlightImg = highlight.GetComponent<Image>();
            if (highlightImg) highlightImg.enabled = false; // off until spin
        }
    }

  

    

    public void Spin()
    {
        if (isSpinning || slots == null || slots.Length == 0 || highlight == null) return;
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;

        int total = slots.Length;
        int targetIndex = Random.Range(0, total);
        int steps = laps * total + targetIndex;
        int lastIndex = 0;
        if (highlightImg) highlightImg.enabled = true;

        for (int step = 0; step < steps; step++)
        {
            currentIndex = (currentIndex + 1 + total) % total;

            // snap & stretch glow to active slot
            AttachHighlightTo(slots[currentIndex].rectTransform);
            lastIndex = currentIndex;
            float t = (float)step / steps;
            float delay = Mathf.Lerp(minDelay, maxDelay, t);
            yield return new WaitForSeconds(delay);
        }

        // final snap to winner
        AttachHighlightTo(slots[lastIndex].rectTransform);

        // winner name = FoodIcon sprite name if present, else slot name
        string rewardName = GetIconName(slots[lastIndex]);
        Wallet.Instance.AddReward(rewardName);
        Debug.Log("Won: " + rewardName);

        // leave glow showing on the winner
        isSpinning = false;
    }

    void AttachHighlightTo(RectTransform target)
    {
        highlight.SetParent(target, false);
        highlight.anchorMin = Vector2.zero;
        highlight.anchorMax = Vector2.one;
        highlight.pivot = new Vector2(0.5f, 0.5f);
        highlight.anchoredPosition = Vector2.zero;
        highlight.offsetMin = Vector2.zero;
        highlight.offsetMax = Vector2.zero;
        highlight.SetAsLastSibling();
    }

    string GetIconName(Image slot)
    {
        var iconTr = slot.transform.Find("FoodIcon") as RectTransform;
        var icon = iconTr ? iconTr.GetComponent<Image>() : null;
        if (icon && icon.sprite) return icon.sprite.name;
        return slot.name;
    }

    
}
