using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Modified {
    public class ToggleGroup : UnityEngine.UI.ToggleGroup {
        public delegate void ChangedEventHandler (Toggle newActive);

        public event ChangedEventHandler OnChange;
        protected override void Start () {
            int count = 0;
            foreach (Transform transformToggle in gameObject.transform) {
                transformToggle.name = $"{count++}";
                var toggle = transformToggle.gameObject.GetComponent<Toggle> ();
                toggle.onValueChanged.AddListener ((isSelected) => {
                    if (!isSelected) {
                        return;
                    }
                    var activeToggle = Active ();
                    DoOnChange (activeToggle);
                });
            }
        }
        public Toggle Active () {
            return ActiveToggles ().FirstOrDefault ();
        }

        protected virtual void DoOnChange (Toggle newactive) {
            var handler = OnChange;
            if (handler != null) handler (newactive);
        }
    }
}