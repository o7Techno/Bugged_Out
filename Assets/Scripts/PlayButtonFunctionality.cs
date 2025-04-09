using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonFunctionality : MonoBehaviour
{

    public TextMeshProUGUI seed;
    public GameObject loadingScreen;
    public void OnClick()
    {
        string seed_text = seed.text[..^1];
        if (!string.IsNullOrWhiteSpace(seed_text))
        {
            int int_seed;
            if (int.TryParse(seed_text, out int_seed))
            {
                WorldData.seed = int_seed;
            }
            else
            {
                WorldData.seed = seed_text.GetHashCode();
            }
        }
        else
        {
            WorldData.seed = Random.Range(-2000000000, 2000000000);
        }
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");

        loadingScreen.SetActive(true);

        while (!WorldData.loaded || !operation.isDone)
        {
            yield return null;
        }
    }

}
