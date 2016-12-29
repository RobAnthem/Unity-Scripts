using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class list_Click : MonoBehaviour {
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnClick(); });

    }

    void OnClick () {

    }
}
