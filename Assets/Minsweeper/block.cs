using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BlockStateEnum { empty, block, flagged, mine, one, two, three, four, five, six, seven, eight }

[System.Serializable]
public class BlockStateDatas
{
    public BlockStateEnum state;
    public Sprite image;
}

public class block : MonoBehaviour, IBlockView
{
    public Image image;
    public BlockPresenter blockPresenter;

    public Sprite sprite { set => image.sprite = value; }

    public void Init(int _id, bool _hasbomb)
    {
        blockPresenter = new BlockPresenter(this, _id, _hasbomb);
    }

    public void Onclick()
    {
        blockPresenter.CheckState(LevelGenerator.Instance.Level);
        LevelGenerator.Instance.CheckAllCells();
    }
}














public class BlockModel
{
    public int id;
    public BlockStateEnum CurrentState;
    public bool hasMine;

    public BlockModel(int _id, bool _hasbomb)
    {
        CurrentState = BlockStateEnum.block;
        id = _id;
        hasMine = _hasbomb;
    }

    public void ChackState(int MineNear)
    {
        if (CurrentState == BlockStateEnum.empty)
            return;
        if (CurrentState == BlockStateEnum.mine)
            return;


        if (LevelGenerator.Instance.Flagstate)
        {
            LevelGenerator.Instance.ReleaseFlag();
            if (CurrentState == BlockStateEnum.block)
            {
                CurrentState = BlockStateEnum.flagged;
                LevelGenerator.Instance.flagcount(+1);
                return;
            }
            else if (CurrentState == BlockStateEnum.flagged)
            {
                CurrentState = BlockStateEnum.block;
                LevelGenerator.Instance.flagcount(-1);
                return;
            }

        }

        if (CurrentState == BlockStateEnum.flagged)
            return;


        if (hasMine)//explude
        {
            CurrentState = BlockStateEnum.mine;
            //image.color = Color.red;
            LevelGenerator.Instance.MineExplude();
        }
        else
        {
            if (MineNear == 0)//no mine
            {
                CurrentState = BlockStateEnum.empty;

                int w = LevelGenerator.Instance.Levelwidth;
                int length = LevelGenerator.Instance.Level.Count;
                if ((id - w >= 0) && (id % w > 0)) LevelGenerator.Instance.Level[id - w - 1].Onclick(); //up left
                if ((id - w >= 0)) LevelGenerator.Instance.Level[id - w].Onclick();//up
                if ((id - w >= 0) && (id % w < w - 1)) LevelGenerator.Instance.Level[id - w + 1].Onclick();//up right
                if ((id % w > 0)) LevelGenerator.Instance.Level[id - 1].Onclick();//left
                if ((id % w < w - 1)) LevelGenerator.Instance.Level[id + 1].Onclick();//right
                if ((id + w < length) && (id % w > 0)) LevelGenerator.Instance.Level[id + w - 1].Onclick();//down left
                if ((id + w < length)) LevelGenerator.Instance.Level[id + w].Onclick();//down
                if ((id + w < length) && (id % w < w - 1)) LevelGenerator.Instance.Level[id + w + 1].Onclick();//downright
            }
            else
            {
                CurrentState = (BlockStateEnum)MineNear + 3;
            }
        }

    }

    public int MinInNear(List<block> Allblocks)
    {
        int w = LevelGenerator.Instance.Levelwidth;
        int length = Allblocks.Count;
        int count = 0;
        Debug.LogFormat("ul={0}  u={1}  ur={2}  l={3}  r={4}  dl={5}  d={6}  dr={7}    id={8}", id - w - 1, id - w, id - w + 1, id - 1, id + 1, id + w - 1, id + w, id + w + 1, id);

        count += (id - w >= 0) && (id % w > 0) ? Allblocks[id - w - 1].blockPresenter.HasMine ? 1 : 0 : 0;//up left
        count += (id - w >= 0) ? Allblocks[id - w].blockPresenter.HasMine ? 1 : 0 : 0;//up
        count += (id - w >= 0) && (id % w < w - 1) ? Allblocks[id - w + 1].blockPresenter.HasMine ? 1 : 0 : 0;//up right
        count += (id % w > 0) ? Allblocks[id - 1].blockPresenter.HasMine ? 1 : 0 : 0;//left
        count += (id % w < w - 1) ? Allblocks[id + 1].blockPresenter.HasMine ? 1 : 0 : 0;//right
        count += (id + w < length) && (id % w > 0) ? Allblocks[id + w - 1].blockPresenter.HasMine ? 1 : 0 : 0;//down left
        count += (id + w < length) ? Allblocks[id + w].blockPresenter.HasMine ? 1 : 0 : 0;//down
        count += (id + w < length) && (id % w < w - 1) ? Allblocks[id + w + 1].blockPresenter.HasMine ? 1 : 0 : 0;//downright
        return count;
    }
}

public interface IBlockView
{
    Sprite sprite { set; }
}

public class BlockPresenter
{
    BlockModel BlockModel;
    private IBlockView blockView;


    public bool HasMine
    {
        set => BlockModel.hasMine = value;
        get { return BlockModel.hasMine; }
    }

    public int Id
    {
        get { return BlockModel.id; }
    }

    public BlockPresenter(IBlockView view, int _id, bool _hasbomb)
    {
        blockView = view;
        BlockModel = new BlockModel(_id, _hasbomb);
        updatestate();
    }

    public void CheckState(List<block> Allblocks)
    {
        int _MineNear = BlockModel.MinInNear(Allblocks);
        BlockModel.ChackState(_MineNear);
        updatestate();
    }

    public BlockStateEnum GetCurrentState()
    {
        return BlockModel.CurrentState;
    }

    public void updatestate()
    {
        blockView.sprite = LevelGenerator.Instance.state.Find(x => x.state == GetCurrentState()).image;
    }
}