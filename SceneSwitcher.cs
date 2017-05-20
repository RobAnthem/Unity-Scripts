using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace SpaceRace
{
    public class SceneSwitcher : MonoBehaviour
    {
        public bool timedSwitch;
        public float switchTime;
        private float currentTime;
        public int destination;
        public enum ChangeType { None, Time, SceneId, Next, Previous, Restart }
        public ChangeType switchType;
        public bool AddClickDelegate;
        void OnEnable()
        {
            if (switchType == ChangeType.Next)
            {
                gameObject.GetComponent<Button>().onClick.AddListener(Next);
            }
            else if (switchType == ChangeType.Previous)
            {
                gameObject.GetComponent<Button>().onClick.AddListener(Previous);
            }
            else if (switchType == ChangeType.Restart)
            {
                gameObject.GetComponent<Button>().onClick.AddListener(Restart);
            }
            else if (switchType == ChangeType.SceneId)
            {
                gameObject.GetComponent<Button>().onClick.AddListener(SwitchScene);
            }
            else if (switchType == ChangeType.Time)
            {
                currentTime = switchTime;
                timedSwitch = true;
            }
        }
        void OnDisable()
        {
            if (switchType == ChangeType.Next)
            {
                gameObject.GetComponent<Button>().onClick.RemoveListener(Next);
            }
            else if (switchType == ChangeType.Previous)
            {
                gameObject.GetComponent<Button>().onClick.RemoveListener(Previous);
            }
            else if (switchType == ChangeType.Restart)
            {
                gameObject.GetComponent<Button>().onClick.RemoveListener(Restart);
            }
            else if (switchType == ChangeType.SceneId)
            {
                gameObject.GetComponent<Button>().onClick.RemoveListener(SwitchScene);
            }
            else if (switchType == ChangeType.Time)
            {
                timedSwitch = false;
            }
        }
        void Update()
        {
            if (timedSwitch)
            {
                UpdateTime(Time.deltaTime);
            }
        }
        void UpdateTime(float time)
        {
            currentTime -= time;
            if (currentTime < 0)
            {
                timedSwitch = false;
                SwitchScene();
            }
        }
        public void SwitchScene()
        {
            SceneManager.LoadSceneAsync(destination);
        }
        public void Restart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        public void Next()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex +1);
        }
        public void Previous()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}