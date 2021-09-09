using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UnityScriptableSettings {
public class ScriptableSettingSpawner : MonoBehaviour {
    public enum NavigationMode {
        Auto,
        Override
    }
    public NavigationMode navigationMode;
    [Tooltip("If navigation mode is set to override, selecting right (while not on a slider) will select this object.")]
    public Selectable rightSelect;
    [Tooltip("If navigation mode is set to override, selecting left (while not on a slider) will select this object.")]
    public Selectable leftSelect;
    [Tooltip("If navigation mode is set to override, overflowing off the bottom of the list will select this object. Leaving it null will cause it to loop.")]
    public Selectable downSelect;
    [Tooltip("If navigation mode is set to override, overflowing off the top of the list will select this object. Leaving it null will cause it to loop.")]
    public Selectable upSelect;
    public GameObject slider;
    public GameObject dropdown;
    public GameObject groupTitle;
    private Dictionary<ScriptableSetting,Slider> sliders = new Dictionary<ScriptableSetting, Slider>();
    private Dictionary<ScriptableSetting,TMP_Dropdown> dropdowns = new Dictionary<ScriptableSetting, TMP_Dropdown>();
    [SerializeField]
    private ScriptableSettingGroup targetGroup;
    UnityEngine.UI.Selectable GetSelectable(ScriptableSetting option) {
        if (sliders.ContainsKey(option)) {
            return sliders[option];
        }
        if (dropdowns.ContainsKey(option)) {
            return dropdowns[option];
        }
        return null;
    }
    private int mod(int x, int m) { return (x%m + m)%m; }
    public IEnumerator WaitUntilReadyThenStart() {
        yield return LocalizationSettings.InitializationOperation;
        yield return null;
        ScriptableSettingGroup currentGroup = null;
        foreach(ScriptableSetting option in ScriptableSettingsManager.instance.settings) {
            if (targetGroup != null && option.group != targetGroup) {
                continue;
            }
            if (currentGroup != option.group || currentGroup == null) {
                CreateTitle(option.group.localizedName);
                currentGroup = option.group;
            }
            if (option.GetType().IsSubclassOf(typeof(ScriptableSettingSlider)) || option is ScriptableSettingSlider) {
                CreateSlider(option);
                option.onValueChange += (o) => {sliders[o].SetValueWithoutNotify(o.value);};
                continue;
            }
            CreateDropDown(option);
            option.onValueChange += (o) => {dropdowns[o].SetValueWithoutNotify(Mathf.FloorToInt(o.value));};
        }
        if (navigationMode == NavigationMode.Override) {
            int startRange = -1;
            int endRange = -1;
            if (targetGroup != null) {
                for(int i=0;i<ScriptableSettingsManager.instance.settings.Length;i++) {
                    if (targetGroup == ScriptableSettingsManager.instance.settings[i] && startRange == -1) {
                        startRange = i;
                    }
                    if (targetGroup != ScriptableSettingsManager.instance.settings[i] && startRange != -1 && endRange == -1) {
                        endRange = i;
                    }
                }
            } else {
                startRange = 0;
                endRange = ScriptableSettingsManager.instance.settings.Length;
            }
            int len = endRange-startRange;
            // Set up the navigation.
            for (int i=startRange;i<endRange;i++) {
                var option = ScriptableSettingsManager.instance.settings[i];
                int nextOptionIndex = ((i+1-startRange)%len)+startRange;
                int prevOptionIndex = mod(i-1-startRange,len)+startRange;
                var nextOption = ScriptableSettingsManager.instance.settings[nextOptionIndex];
                var prevOption = ScriptableSettingsManager.instance.settings[prevOptionIndex];
                Navigation nav = GetSelectable(option).navigation;
                if (downSelect == null) {
                    nav.selectOnDown = GetSelectable(nextOption);
                } else {
                    nav.selectOnDown = nextOptionIndex == startRange ? downSelect : GetSelectable(nextOption);
                }
                if (upSelect == null) {
                    nav.selectOnUp = GetSelectable(prevOption);
                } else {
                    nav.selectOnUp = prevOptionIndex == endRange ? upSelect : GetSelectable(prevOption);
                }
                if (!sliders.ContainsKey(option)) {
                    if (rightSelect != null) {
                        nav.selectOnRight = rightSelect;
                    }
                    if (leftSelect != null) {
                        nav.selectOnLeft = leftSelect;
                    }
                }
                nav.mode = Navigation.Mode.Explicit;
                GetSelectable(option).navigation = nav;
            }
        }
        LocalizationSettings.SelectedLocaleChanged += StringChanged;
    }
    void Start() {
        StartCoroutine(WaitUntilReadyThenStart());
    }
    public void OnEnable() {
        StartCoroutine(WaitAndThenSelect());
    }
    public IEnumerator WaitAndThenSelect() {
        yield return null;
        ScriptableSetting topOption = ScriptableSettingsManager.instance.settings[0];
        if (sliders.ContainsKey(topOption)) {
            //GetComponentInParent<EventSystem>()?.SetSelectedGameObject(sliders[topOption].gameObject);
            sliders[topOption].Select();
        }
        if (dropdowns.ContainsKey(topOption)) {
            //GetComponentInParent<EventSystem>()?.SetSelectedGameObject(dropdowns[topOption].gameObject);
            dropdowns[topOption].Select();
        }
    }
    public void CreateTitle(LocalizedString group) {
        GameObject title = GameObject.Instantiate(groupTitle, Vector3.zero, Quaternion.identity);
        title.transform.SetParent(this.transform);
        title.transform.localScale = Vector3.one;
        foreach( TMP_Text t in title.GetComponentsInChildren<TMP_Text>()) {
            if (t.name == "Label") {
                t.text = group.GetLocalizedString();
            }
        }
        title.GetComponentInChildren<LocalizeStringEvent>().StringReference = group;
    }
    public void CreateSlider(ScriptableSetting option) {
        GameObject s = GameObject.Instantiate(slider, Vector3.zero, Quaternion.identity);
        s.transform.SetParent(this.transform);
        s.transform.localScale = Vector3.one;
        foreach( TMP_Text t in s.GetComponentsInChildren<TMP_Text>()) {
            if (t.name == "Label") {
                t.text = option.localizedName.GetLocalizedString();
                t.GetComponent<LocalizeStringEvent>().StringReference = option.localizedName;
            }
            if (t.name == "Min") {
                t.text = option.minValue.ToString();
            }
            if (t.name == "Max") {
                t.text = option.maxValue.ToString();
            }
        }
        Slider slid = s.GetComponentInChildren<Slider>();
        slid.minValue = option.minValue;
        slid.maxValue = option.maxValue;
        slid.SetValueWithoutNotify(option.value);
        slid.wholeNumbers = (option as ScriptableSettingSlider).wholeNumbers;
        slid.onValueChanged.AddListener((newValue)=>{option.SetValue(newValue); });
        sliders.Add(option, slid);
    }
    public void CreateDropDown(ScriptableSetting option) {
        GameObject d = GameObject.Instantiate(dropdown, Vector3.zero, Quaternion.identity);
        d.transform.SetParent(this.transform);
        d.transform.localScale = Vector3.one;
        foreach( TMP_Text t in d.GetComponentsInChildren<TMP_Text>()) {
            if (t.name == "Label") {
                //t.text = o.type.ToString();
                t.text = option.localizedName.GetLocalizedString();
                t.GetComponent<LocalizeStringEvent>().StringReference = option.localizedName;
            }
        }
        List<TMP_Dropdown.OptionData> data = new List<TMP_Dropdown.OptionData>();
        if (option.GetType().IsSubclassOf(typeof(ScriptableSettingLocalizedDropdown)) || option is ScriptableSettingLocalizedDropdown) {
            foreach(LocalizedString str in (option as ScriptableSettingLocalizedDropdown).dropdownOptions) {
                data.Add(new TMP_Dropdown.OptionData(str.GetLocalizedString()));
            }
        } else if (option.GetType().IsSubclassOf(typeof(ScriptableSettingDropdown)) || option is ScriptableSettingDropdown) {
            foreach(string str in (option as ScriptableSettingDropdown).dropdownOptions) {
                data.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        TMP_Dropdown drop = d.GetComponentInChildren<TMP_Dropdown>();
        drop.options = data;
        drop.value = Mathf.RoundToInt(option.value);
        drop.SetValueWithoutNotify(Mathf.RoundToInt(option.value));
        drop.onValueChanged.AddListener((newValue) => {option.SetValue(newValue);});
        dropdowns.Add(option, drop);
    }
    private void StringChanged(Locale locale) {
        StopAllCoroutines();
        ScriptableSettingsManager.instance.StartCoroutine(ChangeStrings());
    }
    IEnumerator ChangeStrings() {
        var otherAsync = LocalizationSettings.SelectedLocaleAsync;
        yield return new WaitUntil(()=>otherAsync.IsDone);
        yield return new WaitForSecondsRealtime(0.1f);
        if (otherAsync.Result != null){
            yield return LocalizationSettings.InitializationOperation;
            foreach (ScriptableSetting option in ScriptableSettingsManager.instance.settings) {
                if (dropdowns.ContainsKey(option)) {
                    if (!option.GetType().IsSubclassOf(typeof(ScriptableSettingLocalizedDropdown)) || option is ScriptableSettingLocalizedDropdown) {
                        continue;
                    }
                    dropdowns[option].ClearOptions();
                    for(int i=0;i<(option as ScriptableSettingLocalizedDropdown).dropdownOptions.Length;i++) {
                        while (dropdowns[option].options.Count <= i) {
                            dropdowns[option].options.Add(new TMP_Dropdown.OptionData((option as ScriptableSettingLocalizedDropdown).dropdownOptions[i].GetLocalizedString()));
                        }
                        dropdowns[option].options[i].text = (option as ScriptableSettingLocalizedDropdown).dropdownOptions[i].GetLocalizedString();
                    }
                    dropdowns[option].SetValueWithoutNotify(1);
                    dropdowns[option].SetValueWithoutNotify(0);
                    dropdowns[option].SetValueWithoutNotify(Mathf.FloorToInt(option.value));
                }
            }
        }
    }
}

}