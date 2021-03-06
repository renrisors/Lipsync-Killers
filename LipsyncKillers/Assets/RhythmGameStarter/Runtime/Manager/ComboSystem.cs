﻿using UnityEngine;

namespace RhythmGameStarter
{
    [HelpURL("https://bennykok.gitbook.io/rhythm-game-starter/hierarchy-overview/stats-system")]
    public class ComboSystem : MonoBehaviour
    {
        private bool isShowing;

        [Comment("Handling simple combo events.")]
        // [Header("[Events]")]
        [CollapsedEvent]
        public StringEvent onComboUpdate;
        [CollapsedEvent]
        public BoolEvent onVisibilityChange;

        private StatsSystem statsSystem;

        
        public int combo;

        private void Awake() {
            statsSystem = GetComponent<StatsSystem>();
        }

        private void Start()
        {
            UpdateComboDisplay();
        }

        public void AddCombo(int addCombo, float deltaDiff, int score)
        {
            statsSystem.AddCombo(addCombo, deltaDiff, score);

            combo++;
            

            if (!isShowing)
            {
                isShowing = true;
                onVisibilityChange.Invoke(isShowing);
            }

            UpdateComboDisplay();
        }

        public void BreakCombo()
        {
            statsSystem.AddMissed(1);
            statsSystem.combo = 0;

            onComboUpdate.Invoke("");

            isShowing = false;
            onVisibilityChange.Invoke(isShowing);
        }

        public void UpdateComboDisplay()
        {
            if(statsSystem.combo != 0)
                onComboUpdate.Invoke("Combo X " + statsSystem.combo.ToString());
            else
            {
                onComboUpdate.Invoke("");
            }
        }
    }
}