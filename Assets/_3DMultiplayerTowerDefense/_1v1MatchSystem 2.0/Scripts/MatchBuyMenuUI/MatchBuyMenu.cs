using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchSystem
{
    public class MatchBuyMenu : MonoBehaviour
    {
        public static MatchBuyMenu Instance;

        public GameObject UICanvas;
        public GameObject BackgroundPanel;
        public GameObject ButtonHolder;

        public bool MenuOpen;

        private SelectionManager SM;
        private bool check = true;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            SM = SelectionManager.Instance;
        }

        private void Update()
        {
            if(MenuOpen && check)
            {
                Debug.Log("OpenMENUANIMATION ");

                StartCoroutine(OpenMenuAnim(0.5f));
                check = false;
            }
            else if(!MenuOpen && !check)
            {
                Debug.Log("CloseMENUANIMATION");

                StartCoroutine(CloseMenuAnim(0.5f));
                check = true;
            }
        }

        public IEnumerator OpenMenuAnim(float animSpeed)
        {
            UICanvas.SetActive(true);

            iTween.ScaleTo(BackgroundPanel, new Vector3(1, 1, 1), animSpeed);
            yield return new WaitForSeconds(animSpeed);
        }
        public IEnumerator CloseMenuAnim(float animSpeed)
        {
            iTween.ScaleTo(BackgroundPanel, new Vector3(0, 0, 0), animSpeed);
            yield return new WaitForSeconds(animSpeed);

            UICanvas.SetActive(false);
        }
    }
}