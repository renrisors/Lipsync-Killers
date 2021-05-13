using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    [HelpURL("https://bennykok.gitbook.io/rhythm-game-starter/hierarchy-overview/stats-system")]
    public class StatsSystem : MonoBehaviour
    {
        [Comment("Responsible for advance stats' config and events.", order = 0)]
        [Title("Hit Level Config", false, 2, order = 1)]
        [Tooltip("Config the hit distance difference for each level, such as Perfect,Ok etc")]
        public HitLevelList levels;

        [Title("Events", 2)]
        [CollapsedEvent]
        public StringEvent onComboStatusUpdate;
        [CollapsedEvent]
        public StringEvent onScoreUpdate;
        [CollapsedEvent]
        public StringEvent onMaxComboUpdate;
        [CollapsedEvent]
        public StringEvent onMissedUpdate;

        #region RUNTIME_FIELD
        [NonSerialized] public int combo;
        [NonSerialized] public int maxCombo;
        [NonSerialized] public int missed;
        [NonSerialized] public int score;
        #endregion

        //Used to check combo and display mensage
        public GameObject[] comboFrases = new GameObject[5];
        public StarHandler stars;

        //SFX
        private GameObject ComboSFX;
        private GameObject arrasaSFX;
        private GameObject fabulosoSFX;
        private GameObject impecavelSFX;
        private GameObject yesGirlSFX;
        

        [Serializable]
        public class HitLevelList : ReorderableList<HitLevel> { }

        [Serializable]
        public class HitLevel
        {
            public string name;
            public float threshold;
            [HideInInspector]
            public int count;
            public float scorePrecentage = 1;
            public StringEvent onCountUpdate;
        }

        public void AddMissed(int addMissed)
        {
            missed += addMissed;
            onMissedUpdate.Invoke(missed.ToString());

            for (int i = 0; i < 5; i++)
            {
                comboFrases[i].SetActive(false);
            }
        }

        void Start()
        {
            UpdateScoreDisplay();
            ComboSFX = GameObject.Find("ComboSFX");
            arrasaSFX = GameObject.Find("ArrasaSFX");
            fabulosoSFX = GameObject.Find("FabulosoSFX");
            impecavelSFX = GameObject.Find("ImpecavelSFX");
            yesGirlSFX = GameObject.Find("YesGirlSFX");


            for (int i = 0; i < 5; i++)
            {
                comboFrases[i].SetActive(false);
            }
        }

        public void AddCombo(int addCombo, float deltaDiff, int addScore)
        {
            // Check if the Frenesi Is happening and if Is return the addScore modified
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            addScore = player.GetComponent<Actions>().Frenesi(addScore);

            // Debug.Log(combo);
            // print(deltaDiff);
            combo += addCombo;
            if (combo > maxCombo)
            {
                maxCombo = combo;
                onMaxComboUpdate.Invoke(maxCombo.ToString());
            }

            player.GetComponent<Actions>().GetCombo(combo);

            //Check if the player is making a combo and display mensage
            if (combo < 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    comboFrases[i].SetActive(false);
                }
            }
            else if (combo == 1)
            {
                comboFrases[0].SetActive(true);
                score += (int)(addScore * 0.5);
                yesGirlSFX.GetComponent<AudioSource>().Play();
            }
            else if (combo == 6)
            {
                comboFrases[0].SetActive(false);
                comboFrases[1].SetActive(true);
                score += (int)(addScore * 1);
                arrasaSFX.GetComponent<AudioSource>().Play();
            }
            else if (combo == 11)
            {
                comboFrases[1].SetActive(false);
                comboFrases[2].SetActive(true);
                score += (int)(addScore * 1.2);
                ComboSFX.GetComponent<AudioSource>().Play();
            }
            else if (combo == 16)
            {
                comboFrases[2].SetActive(false);
                comboFrases[3].SetActive(true);
                score += (int)(addScore * 1.7);
                fabulosoSFX.GetComponent<AudioSource>().Play();
            }
            else if (combo == 21)
            {
                comboFrases[3].SetActive(false);
                comboFrases[4].SetActive(true);
                score += (int)(addScore * 2);
                impecavelSFX.GetComponent<AudioSource>().Play();
            }

            //carregar o especial
            player.GetComponent<Actions>().ChargeSpecial();

            for (int i = 0; i < levels.values.Count; i++)
            {
                var x = levels.values[i];
                if (deltaDiff <= x.threshold)
                {
                    x.count++;
                    score += (int)(addScore * x.scorePrecentage);
                    x.onCountUpdate.Invoke(x.count.ToString());
                    UpdateScoreDisplay();
                    onComboStatusUpdate.Invoke(x.name);
                    // print(x.name);
                    return;
                }
            }

            //When no level matched
            onComboStatusUpdate.Invoke("");

        }

        public void UpdateScoreDisplay()
        {
            onScoreUpdate.Invoke(score.ToString());
        }

        public void Update()
        {
            stars.score = score;
        }
    }
}