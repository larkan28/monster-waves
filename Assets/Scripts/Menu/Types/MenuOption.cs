using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : Menu
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonApply;
    [SerializeField] private TMP_Dropdown dropdownResolution;
    [SerializeField] private TMP_Dropdown dropdownQuality;
    [SerializeField] private Slider sliderSound;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private TMP_Text textSound;
    [SerializeField] private TMP_Text textMusic;
    [SerializeField] private Toggle toggleFullScreen;

    private void OnEnable()
    {
        buttonBack.onClick.AddListener(ButtonBack);
        buttonApply.onClick.AddListener(ButtonApply);
        sliderSound.onValueChanged.AddListener(OnSoundChanged);
        sliderMusic.onValueChanged.AddListener(OnMusicChanged);
    }

    private void OnDisable()
    {
        buttonBack.onClick.RemoveListener(ButtonBack);
        buttonApply.onClick.RemoveListener(ButtonApply);
        sliderSound.onValueChanged.RemoveListener(OnSoundChanged);
        sliderMusic.onValueChanged.RemoveListener(OnMusicChanged);
    }

    public override void Show()
    {
        base.Show();
        RefreshValues();
    }

    private void RefreshValues()
    {
        Resolution[] resolutions = Screen.resolutions;
        dropdownResolution.ClearOptions();

        List<string> options = new();

        foreach (var resolution in resolutions)
        {
            if (resolution.refreshRateRatio.numerator == 60)
                options.Add(resolution.width + "x" + resolution.height);
        }

        string currResolution = ResToStr(Screen.currentResolution);

        dropdownResolution.AddOptions(options);
        dropdownResolution.value = options.FindIndex(x => x.Equals(currResolution));

        dropdownQuality.ClearOptions();
        dropdownQuality.AddOptions(QualitySettings.names.ToList());
        dropdownQuality.value = QualitySettings.GetQualityLevel();

        sliderSound.value = Mathf.Pow(10f, GameSound.Instance.SoundVolume / 20);
        sliderMusic.value = Mathf.Pow(10f, GameSound.Instance.MusicVolume / 20);

        OnSoundChanged(sliderSound.value);
        OnMusicChanged(sliderMusic.value);

        toggleFullScreen.isOn = Screen.fullScreen;
    }

    private void ApplyChanges()
    {
        string[] resolution = dropdownResolution.options[dropdownResolution.value].text.Split("x");

        int w = int.Parse(resolution[0]);
        int h = int.Parse(resolution[1]);

        Screen.SetResolution(w, h, toggleFullScreen.isOn);
        GameSound.Instance.SoundVolume = Mathf.Log10(sliderSound.value) * 20f;
        GameSound.Instance.MusicVolume = Mathf.Log10(sliderMusic.value) * 20f;
        QualitySettings.SetQualityLevel(dropdownQuality.value);

        RefreshValues();
    }

    private string ResToStr(Resolution resolution)
    {
        return resolution.width + "x" + resolution.height;
    }

    private void OnMusicChanged(float value)
    {
        textMusic.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    private void OnSoundChanged(float value)
    {
        textSound.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    private void ButtonBack()
    {
        m_manager.Back();
    }

    private void ButtonApply()
    {
        ApplyChanges();
    }
}
