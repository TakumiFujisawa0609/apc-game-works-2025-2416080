using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [Header("éQè∆")]
    [SerializeField] Transform playerVisual;   
    [SerializeField] Animator anim;            
    [SerializeField] Rigidbody2D rb;          

    [Header("ÉAÉjÉÅçƒê∂")]
    public string deathTrigger = "Death";   
    public string deathStateName = "Death";    

    [Header("ëJà⁄ê›íË")]
    public float gameOverDelay = 1.0f;        
    public string gameOverSceneName = "GameOverScene";

    [Header("ìñÇΩÇËí‚é~")]
    public bool disableCollidersOnDeath = true;

    bool isDead;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!playerVisual) playerVisual = transform.Find("PlayerVisual");
        if (playerVisual && !anim) anim = playerVisual.GetComponent<Animator>();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // ì¸óÕ/çsìÆånÇé~ÇﬂÇÈ
        var pc = GetComponent<PlayerController>(); if (pc) pc.enabled = false;
        var pa = GetComponent<PlayerAttack>(); if (pa) pa.enabled = false;
        var ma = GetComponent<MeleeAttack>(); if (ma) ma.enabled = false;
        var ra = GetComponent<RangedAttack>(); if (ra) ra.enabled = false;
        var pop = GetComponent<PlayerOnPlatform>(); if (pop) pop.enabled = false;

        // ï®óùí‚é~
        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false;
        }

        if (disableCollidersOnDeath)
        {
            foreach (var col in GetComponentsInChildren<Collider2D>())
                col.enabled = false;
        }

        if (anim)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            bool played = false;

            if (!string.IsNullOrEmpty(deathTrigger))
            {
                foreach (var p in anim.parameters)
                {
                    if (p.type == AnimatorControllerParameterType.Trigger && p.name == deathTrigger)
                    {
                        anim.ResetTrigger(deathTrigger);
                        anim.SetTrigger(deathTrigger);
                        played = true;
                        break;
                    }
                }
            }

            if (!played && !string.IsNullOrEmpty(deathStateName))
            {
                anim.CrossFade(deathStateName, 0f, 0); 
            }
        }


        StartCoroutine(CoGameOver());
    }

    IEnumerator CoGameOver()
    {
        // ÉQÅ[ÉÄÇÃéûä‘í‚é~Ç…âeãøÇ≥ÇÍÇ»Ç¢ë“ã@
        float t = 0f;
        while (t < gameOverDelay) { t += Time.unscaledDeltaTime; yield return null; }

        var fader = FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            fader.FadeOut(gameOverSceneName);
        }
        else
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}
