using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance;

    [Header("UI")]
    public GameObject walletPopup;            // panel that shows collected items
    public TextMeshProUGUI popupText;         // text inside popup
    public TextMeshProUGUI walletText_legacy; // optional running text (can be null)

    private readonly Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        if (walletPopup) walletPopup.SetActive(false);
        UpdateLegacyText();
    }

    // simple, non-animated add
    public void AddReward(string food)
    {
        if (!items.ContainsKey(food)) items[food] = 0;
        items[food]++;
        UpdateLegacyText();
        if (walletPopup && walletPopup.activeSelf) RefreshPopupText();
    }

    // Click the wallet to open/close the list
    public void TogglePopup()
    {
        if (!walletPopup) return;
        bool show = !walletPopup.activeSelf;
        walletPopup.SetActive(show);
        if (show) RefreshPopupText();
    }

    public void HidePopup()
    {
        if (!walletPopup) return;
        walletPopup.SetActive(false);
    }

    private void RefreshPopupText()
    {
        if (!popupText) return;
        popupText.text = "<b>My Rewards</b>\n";
        foreach (var kvp in items)
            popupText.text += $"{kvp.Key}: {kvp.Value}\n";
        if (items.Count == 0) popupText.text += "(empty)";
    }

    private void UpdateLegacyText()
    {
        if (!walletText_legacy) return;
        walletText_legacy.text = "Wallet:\n";
        foreach (var kvp in items)
            walletText_legacy.text += $"{kvp.Key}: {kvp.Value}\n";
    }
}
