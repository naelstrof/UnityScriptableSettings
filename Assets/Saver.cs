using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScriptableSettings;

public class Saver : MonoBehaviour {
    private void Start() {
        StartCoroutine(SaveAfterTime());
    }

    IEnumerator SaveAfterTime() {
        while (isActiveAndEnabled) {
            yield return new WaitForSeconds(4f);
            SettingsManager.Save();
        }
    }
}
