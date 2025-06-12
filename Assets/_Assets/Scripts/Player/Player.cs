using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    public TextMeshProUGUI playerHP;

    public GameObject gameOverUI;
    public bool isDead; 

    private void Start()
    {
        playerHP.text = $"Health : {HP}";
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            print("niggas dead");
            PlayerDead();
            isDead = true;

        }
        else
        {
            print("ay");
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

        //die

        GetComponentInChildren<Animator>().enabled = true;
        playerHP.gameObject.SetActive(false);

        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        throw new NotImplementedException();
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            yield return new WaitForSeconds(1f);
            gameOverUI.gameObject.SetActive (true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }

        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }
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
