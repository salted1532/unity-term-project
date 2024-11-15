using UnityEngine;

namespace Krivodeling.UI.Effects.Examples
{
    public class UIBlurWithCanvasGroup : MonoBehaviour
    {
        private UIBlur _uiBlur;
        public CanvasGroup _canvasGroup;
        public bool isPause = false;
        public GameObject Blur;

        private void Start()
        {
            SetComponents();
            gameObject.SetActive(true);
        }

        private void SetComponents()
        {
            _uiBlur = GetComponent<UIBlur>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginBlur()
        {
            isPause = !isPause;
            if(isPause==true)
            {
                Blur.GetComponent<UIBlur>().Intensity = 0.5f;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
                _canvasGroup.alpha = 1;
            }
            else
            {
                Blur.GetComponent<UIBlur>().Intensity = 0f;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
                _canvasGroup.alpha = 0;
            }
        }

        private void OnBlurChanged(float value)
        {
            _canvasGroup.alpha = value;
        }

        private void OnEndBlur()
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
