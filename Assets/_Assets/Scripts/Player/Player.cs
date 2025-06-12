using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    public TextMeshProUGUI playerHP;

    public GameObject gameOverUI;
    public bool isDead;

    public string menuSceneName = "MainMenu";

    private void Start()
    {
        playerHP.text = $"Health : {HP}";
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            print("Player is dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player took damage");
            StartCoroutine(BloodyScreenEffect());
            playerHP.text = $"Health : {HP}";
            SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;

        playerHP.gameObject.SetActive(false);

        if (GetComponent<ScreenFader>() != null)
        {
            GetComponent<ScreenFader>().StartFade();
        }

        StartCoroutine(ShowGameOverUIAndLoadMenu());
    }

    private IEnumerator ShowGameOverUIAndLoadMenu()
    {
        yield return new WaitForSeconds(2f);

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(menuSceneName);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen != null && !bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        bloodyScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}