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
    public Selectable rightSelect;
    public Selectable leftSelect;
    public GameObject slider;
    public GameObject dropdown;
    public GameObject groupTitle;
    private Dictionary<ScriptableSetting,Slider> sliders = new Dictionary<ScriptableSetting, Slider>();
    private Dictionary<ScriptableSetting,TMP_Dropdown> dropdowns = new Dictionary<ScriptableSetting, TMP_Dropdown>();
    [SerializeField]
    private LocalizedString targetGroup;
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
        long currentGroupKey = 0;
        foreach(ScriptableSetting option in ScriptableSettingsManager.instance.settings) {
            if (!targetGroup.IsEmpty && option.group != targetGroup) {
                continue;
            }
            if (currentGroupKey != option.group.TableEntryReference.KeyId) {
                CreateTitle(option.group);
                currentGroupKey = option.group.TableEntryReference.KeyId;
            }
            if (option.GetType().IsSubclassOf(typeof(ScriptableSettingSlider))) {
                CreateSlider(option);
                option.onValueChange += (o) => {sliders[o].SetValueWithoutNotify(o.value);};
                continue;
            }
            CreateDropDown(option);
            option.onValueChange += (o) => {dropdowns[o].SetValueWithoutNotify(Mathf.FloorToInt(o.value));};
        }
        // Set up the navigation.
        for (int i=0;i<ScriptableSettingsManager.instance.settings.Length;i++) {
            var option = ScriptableSettingsManager.instance.settings[i];
            var nextOption = ScriptableSettingsManager.instance.settings[(i+1)%ScriptableSettingsManager.instance.settings.Length];
            var prevOption = ScriptableSettingsManager.instance.settings[mod(i-1,ScriptableSettingsManager.instance.settings.Length)];
            Navigation nav = GetSelectable(option).navigation;
            nav.selectOnDown = GetSelectable(nextOption);
            nav.selectOnUp = GetSelectable(prevOption);
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
        if (option.GetType().IsSubclassOf(typeof(ScriptableSettingLocalizedDropdown))) {
            foreach(LocalizedString str in (option as ScriptableSettingLocalizedDropdown).dropdownOptions) {
                data.Add(new TMP_Dropdown.OptionData(str.GetLocalizedString()));
            }
        } else if (option.GetType().IsSubclassOf(typeof(ScriptableSettingDropdown))) {
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
                    if (!option.GetType().IsSubclassOf(typeof(ScriptableSettingLocalizedDropdown))) {
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