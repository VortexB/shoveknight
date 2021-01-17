using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] string Options = "Options";
    [SerializeField] Volume Volume;
    [SerializeField] float colorRange=100;
    [SerializeField] TextMeshProUGUI levelNum;
    ColorAdjustments col;
    public static LevelController Instance;
    public Animator anim;
    public GameObject Portal;
    public GameObject Charged;
    public Vector2Int startPos;
    


    public int RemainingPushable 
    {
        set
        {
            remainingPushable = value;
            if (remainingPushable == 0)
            {
                Portal.GetComponent<Animator>().SetBool("Push",true);
            }
            else
            {
                Portal.GetComponent<Animator>().SetBool("Push", false);
            }
            if (remainingCollectable == 0)
            {
                Portal.GetComponent<Animator>().SetBool("Collect", true);
            }
            else
            {
                Portal.GetComponent<Animator>().SetBool("Collect", false);
            }
            if (remainingCollectable == 0 && remainingPushable == 0)
            {
                Charged.SetActive(true);
            }
            else
            {
                Charged.SetActive(false);

            }

        }
        get =>remainingPushable;
        
    }
    public void Pause()
    {
        PauseMenu.SetActive(true);
    }
    public void UnPause()
    {
        PauseMenu.SetActive(false);
    }
    private int remainingPushable;

    public int RemainingCollectable
    {
        set
        {
            remainingCollectable = value;
            if (remainingCollectable == 0)
            {
                Portal.GetComponent<Animator>().SetBool("Collect", true);
            }
            else
            {
                Portal.GetComponent<Animator>().SetBool("Collect", false);
            }
            if (remainingPushable == 0)
            {
                Portal.GetComponent<Animator>().SetBool("Push",true);
            }
            else
            {
                Portal.GetComponent<Animator>().SetBool("Push", false);
            }
            if (remainingCollectable == 0 && remainingPushable == 0)
            {
                Charged.SetActive(true);
            }
            else
            {
                Charged.SetActive(false);

            }


        }
        get => remainingCollectable;

    }
    private int remainingCollectable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        remainingPushable = 0;
        remainingCollectable = 0;

        //rp
        
        Volume.profile.TryGet(out col);
        ClampedFloatParameter f = new ClampedFloatParameter(Random.Range(-colorRange, colorRange), -180, 180);
        col.hueShift.SetValue(f);
        if(levelNum!=null)
            levelNum.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        
    }
    
    public void PlaceStart(Vector2Int pos)
    {
        startPos = pos;
        Portal.transform.position = pos + new Vector2(0.5f, 0.5f);
        StartCoroutine(StartZoomOut());

    }
    public void IsLevelComplete()
    {
        if(RemainingCollectable == 0 && RemainingPushable == 0)
        {
            StartCoroutine(StartZoomIn());
            AudioController.instance.PlaySound("win");
        }
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(FadeLoad(index));
    }
    public void NextLevel()
    {
        StartCoroutine(FadeLoad(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public int GetLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void RestartLevel()
    {
        LoadLevel(GetLevel());
    }
    public void LoadOptions()
    {
        SceneManager.LoadSceneAsync(Options, LoadSceneMode.Additive);
    }
    public void UnloadOptions()
    {
        SceneManager.UnloadSceneAsync(Options);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator FadeLoad(int index)
    {
        if (anim == null) { SceneManager.LoadScene(index); yield break; }

        anim.SetTrigger("StartFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
    IEnumerator StartZoomOut()
    {
        Camera main = Camera.main;
        var orgPos = main.transform.position;
        var orgZoom = main.orthographicSize;
        int i= 0;
        main.orthographicSize = 0;
        main.transform.position = (Vector3Int)startPos+ new Vector3(0.5f,0.5f,-10);
        while (i < 100)
        {
            i++;
            yield return new WaitForFixedUpdate();
            main.orthographicSize += orgZoom / 100;
            main.transform.position=Vector3.MoveTowards(main.transform.position, orgPos, Vector3.Distance((Vector3Int)startPos + new Vector3(0.5f, 0.5f, -10), orgPos)/100);
        }
    }
    IEnumerator StartZoomIn()
    {
        Camera main = Camera.main;
        StartCoroutine(main.GetComponent<CamShake>().Shake(5f, 0.3f));

        var orgPos = main.transform.position;
        var orgZoom = main.orthographicSize;
        int i = 0;
        while (i < 100)
        {

            i++;
            yield return new WaitForFixedUpdate();
            main.orthographicSize -= orgZoom / 100;
            main.transform.position = Vector3.MoveTowards(main.transform.position, (Vector3Int)startPos + new Vector3(0.5f, 0.5f, -10), Vector3.Distance((Vector3Int)startPos + new Vector3(0.5f, 0.5f, -10), orgPos) / 100);
        }
        NextLevel();
    }
}
