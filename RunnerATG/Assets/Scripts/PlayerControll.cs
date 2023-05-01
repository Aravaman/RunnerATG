using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControll : MonoBehaviour
{
    Animator animator;
    [SerializeField] private TMP_Text coinText;
    private int coin;
    [SerializeField] GameObject gameOver;

    void Start()
    {
        animator = GetComponent<Animator>();
        coin = PlayerPrefs.GetInt("coins");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            animator.SetBool("LeftMove", true);
        else 
            animator.SetBool("LeftMove", false);

        if (Input.GetKey(KeyCode.D))
            animator.SetBool("RightMove", true);
        else 
            animator.SetBool("RightMove", false);

        PlayerPrefs.SetInt("coins", coin);
        coinText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coin += 1;
        }

        if (other.gameObject.tag == "Planet")
        {
            gameOver.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
