using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour, IuiView
{
    public GameObject BlockPrefab;
    public List<block> Level;
    public static LevelGenerator Instance;
    public GameObject root;
    public int Levelwidth;
    public int Levelheihgt;
    public int blocksize;
    public int BombCount;
    //public int BombsFound;
    //public int SecondsPassed;
    //float Timer;
    public Button flagButton;
    public bool Flagstate;
    public Text bombsFoundText;
    public Text TimeText;
    public bool GameIsFinished;
    public Text ResaultText;
    public GameObject Resault;

    public UIPresenter UIpresenter;
    public List<BlockStateDatas> state;

    public string flag { set => bombsFoundText.text = value; }
    string IuiView.Timer { set => TimeText.text = value; }

    // Start is called before the first frame update
    void Start()
    {
        UIpresenter = new UIPresenter(this, BombCount);
        GameIsFinished = false;
        Instance = this;
        Level.Clear();
        for (int i = 0; i < Levelheihgt; i++)
        {
            for (int j = 0; j < Levelwidth; j++)
            {
                GameObject b = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity);
                b.transform.SetParent(root.transform);
                b.transform.localPosition = new Vector3(j * blocksize, -i * blocksize, 0);
                //b.GetComponent<block>().id = Level.Count;
                //b.GetComponent<block>().hasMine = Random.Range(0, 100) > 80;
                b.GetComponent<block>().Init(Level.Count,false);
                Level.Add(b.GetComponent<block>());
            }
        }

        for (int i = 0; i < BombCount; i++)
        {
            int id = Random.Range(0, Level.Count);
            while (Level[id].blockPresenter.HasMine)
                id = Random.Range(0, Level.Count);
            Level[id].blockPresenter.HasMine = true;
        }

        root.transform.localPosition = new Vector3(
            -(Levelwidth * blocksize / 2) + blocksize / 2,
            (Levelwidth * blocksize / 2) - blocksize / 2,
            0);

        //Timer = 0;

        //bombsFoundText.text = string.Format("Flag: {0}/{1}", BombsFound, BombCount);
        UIpresenter.UpdateFlagCount(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Timer += Time.deltaTime;
        //SecondsPassed = Mathf.FloorToInt(Timer);
        UIpresenter.UpdateTimer();
        //TimeText.text = "Timer : " + SecondsPassed.ToString();
    }

    public void restart()
    {
        SceneManager.LoadScene("Minsweeper");
    }

    public void SetFlag()
    {
        flagButton.interactable = false;
        Flagstate = true;
    }
    public void ReleaseFlag()
    {
        flagButton.interactable = true;
        Flagstate = false;
    }

    public void flagcount(int value)
    {
        //BombsFound += value;
        //bombsFoundText.text = string.Format("Flag: {0}/{1}", BombsFound, BombCount);
        UIpresenter.UpdateFlagCount(value);
    }

    public void MineExplude()
    {
        foreach (block bl in Level)
            if (bl.blockPresenter.HasMine)
                bl.Onclick();
        GameIsFinished = true;
        Showresault(true);
    }

    public void Showresault(bool explude)
    {
        Resault.SetActive(true);
        ResaultText.text = (explude) ? "YOU LOSE!" : "YOU WIN!";
    }
    public void CheckAllCells()
    {
        bool allchecked = true;
        foreach (block bl in Level)
            if ((bl.blockPresenter.GetCurrentState() == BlockStateEnum.block)
                || ((bl.blockPresenter.GetCurrentState() == BlockStateEnum.flagged) && !bl.blockPresenter.HasMine))
                allchecked = false;

        if (allchecked)
            Showresault(false);
    }
}

