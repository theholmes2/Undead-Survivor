using System;
using System.Collections;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharactor;
    public GameObject[] unlockCharactor;
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;
    WaitForSecondsRealtime wait;

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait= new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData")) { Init(); }
        
    }


    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives) {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
        
    }

    private void Start()
    {
        UnlockCharactor();
    }

    void UnlockCharactor()
    {
        for (int index = 0; index < lockCharactor.Length; index++)
        {
            string achiveName = achives[index].ToString();
         
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            
            lockCharactor[index].SetActive(!isUnlock);
            unlockCharactor[index].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime ;
                break;
        }

        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index=0;index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }

    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }

}
