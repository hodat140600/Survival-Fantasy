using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class ChapterPause : MonoBehaviour
    {
        public GameObject pausePrefabs;
        public GameObject pausePanel;

        public void ReloadPause()
        {
            Vector3 pausePanelPosition = this.pausePanel.transform.position;
            Transform pauseTransform = pausePanel.transform.parent;
            Destroy(pausePanel);
            pausePanel = Instantiate(pausePrefabs, pausePanelPosition, Quaternion.identity, pauseTransform).gameObject;
        }
    }
