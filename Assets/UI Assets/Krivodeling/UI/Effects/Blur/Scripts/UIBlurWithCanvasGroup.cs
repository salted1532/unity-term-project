using UnityEngine;

namespace Krivodeling.UI.Effects.Examples
{
    public class UIBlurWithCanvasGroup : MonoBehaviour
    {
        private UIBlur _uiBlur;
        private CanvasGroup _canvasGroup;
        public bool isPause = false;
        public GameObject Blur;

        private void Start()
        {
            SetComponents();
            

            _uiBlur.OnBeginBlur.AddListener(OnBeginBlur);
            _uiBlur.OnBlurChanged.AddListener(OnBlurChanged);
            _uiBlur.OnEndBlur.AddListener(OnEndBlur);
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
                _canvasGroup.alpha = 1;
            }
            else
            {
                Blur.GetComponent<UIBlur>().Intensity = 0f;
                _canvasGroup.blocksRaycasts = false;
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
