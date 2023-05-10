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

    void Update()
    {
        // Анимация движения влево
        if (Input.GetKey(KeyCode.A))
            animator.SetBool("LeftMove", true);
        else 
            animator.SetBool("LeftMove", false);
        // Анимация движения вправо
        if (Input.GetKey(KeyCode.D))
            animator.SetBool("RightMove", true);
        else 
            animator.SetBool("RightMove", false);
        // Сохраняем количество монет и обновляем текстовое поле с количеством монет
        PlayerPrefs.SetInt("coins", coin);
        coinText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    // Обработчик событий триггера
    private void OnTriggerEnter(Collider other)
    {
        // Увеличиваем количество монет, если объект с тегом "Coin"
        if (other.gameObject.tag == "Coin")
        {
            coin += 1;
        }

        // Активируем экран "Game Over", если объект с тегом "Planet"
        if (other.gameObject.tag == "Planet")
        {
            gameOver.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Сброс игры и перезагрузка сцены
    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
