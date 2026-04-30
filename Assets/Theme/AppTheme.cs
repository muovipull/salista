using UnityEngine;

[CreateAssetMenu(fileName = "UusiTeema", menuName = "Teemat/AppTheme")]
public class AppTheme : ScriptableObject
{
    public Color Musta;
    public Color valkoinen;
    public Color tausta;
    public Color tekstivari;
    public Color input_alku;
    public Color input_loppu;
    public Color poistu;
    public Color seuraava_hyväksy;
    public Color sininen_valikko;
    public Color violetti_valikko;
    public Color pinkki_valikko;
    public Color turkoosi_valikko;
    public Material yhteinenMateriaali;

}

public static class ThemeEvents
{
    public delegate void ThemeChanged();
    public static event ThemeChanged OnThemeChanged;

    public static void TriggerThemeUpdate()
    {
        OnThemeChanged?.Invoke();
    }
}