using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputField iterationField;
    public InputField cellField;
    public Dropdown type;


    public void SetValues()
    {
        if (isValid())
        {
            PlayerPrefs.SetInt("iteration", int.Parse(iterationField.text));
            PlayerPrefs.SetInt("cell", int.Parse(cellField.text));
            PlayerPrefs.SetInt("type", int.Parse(type.options[type.value].text));

            Debug.Log("Iteration " + PlayerPrefs.GetInt("iteration"));
            Debug.Log("Cell " + PlayerPrefs.GetInt("cell"));
            Debug.Log("Type " + PlayerPrefs.GetInt("type"));

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private bool isValid()
    {
        return !string.IsNullOrEmpty(iterationField.text) && !string.IsNullOrEmpty(cellField.text);
    }
}
