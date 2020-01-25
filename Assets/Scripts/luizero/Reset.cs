using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
	//v0.003

	public KeyCode resetKey = KeyCode.T;
	int activeSceneIndex;

	void Start()
	{
		activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
	}

	void Update()
	{
		if (Input.GetKeyDown(resetKey))
		{
			ResetScene();
		}
	}
	public void ResetScene()
	{
		SceneManager.LoadScene(activeSceneIndex);
	}
}
