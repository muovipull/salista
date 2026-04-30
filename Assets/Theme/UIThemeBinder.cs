using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIThemeBinder : MonoBehaviour
{
    public AppTheme teema;
    public bool isBackground; // Valitse onko tämä tausta vai teksti

    void OnEnable()
    {
        ThemeEvents.OnThemeChanged += UpdateVisuals;
        UpdateVisuals();
    }

    void OnDisable()
    {
        ThemeEvents.OnThemeChanged -= UpdateVisuals;
    }

    void UpdateVisuals()
    {
        if (GetComponent<Image>() != null)
            GetComponent<Image>().color = isBackground ? teema.tausta : teema.tekstivari;

        if (GetComponent<TextMeshProUGUI>() != null)
            GetComponent<TextMeshProUGUI>().color = teema.tekstivari;
    }
}