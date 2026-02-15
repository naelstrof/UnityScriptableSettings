using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct ScriptableSettingString {
    [SerializeField] public LocalizedString localizedString;
    [SerializeField] public string backupString;

    public ScriptableSettingString(string backupString) {
        localizedString = null;
        this.backupString = backupString;
    }
    
    public async Task<string> GetLocalizedStringAsync() {
        if (localizedString is not { IsEmpty: false }) return backupString;
        var str = await localizedString.GetLocalizedStringAsync().Task;
        if (string.IsNullOrEmpty(str)) {
            return backupString;
        }
        return str;
    }

    public string GetLocalizedString() {
        if (localizedString is { IsEmpty: false }) {
            return localizedString.GetLocalizedString();
        }

        return backupString;
    }
    
    public bool TryGetLocalizedLabel(out LocalizedString localizedLabel) {
        if (localizedString is not { IsEmpty: false }) {
            localizedLabel = null;
            return false;
        }
        localizedLabel = localizedString;
        return localizedString != null;
    }
}
