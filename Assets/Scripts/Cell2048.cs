using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell2048 : MonoBehaviour
{
    public Cell2048 right;
    public Cell2048 down;
    public Cell2048 left;
    public Cell2048 up;
    public Fill2048 fill;
    // [SerializeField] GameObject fillPrefab;


    public int saveValue;

    private void Start()
    {
        // yield return new WaitForSeconds(0.1f);
        // foreach (string skipCell in GameController.skipCell)
        // {
        //     if (this.gameObject.name == "Cell (" + skipCell + ")")
        //     {
        //         this.gameObject.SetActive(false);
        //         Destroy(this.gameObject);
        //     }
        // }
    }

    private void OnEnable()
    {
        GameController.slide += OnSlide;
    }
    private void OnDisable()
    {
        GameController.slide -= OnSlide;
    }

    private void OnSlide(string whatWasSent)
    {
        Cell2048 currentCell = this;

        // Debug.Log(this.name + " " + this.saveValue);
        if (whatWasSent == "w")
        {
            if (up != null) return;

            SlideUp(currentCell);
        }
        if (whatWasSent == "d")
        {
            if (right != null) return;
            // Cell2048 currentCell = this;
            SlideRight(currentCell);
        }
        if (whatWasSent == "s")
        {
            if (down != null) return;
            // Cell2048 currentCell = this;
            SlideDown(currentCell);
        }
        if (whatWasSent == "a")
        {
            if (left != null) return;
            // Cell2048 currentCell = this;
            SlideLeft(currentCell);
        }
        GameController.ticker++;

        if (PlayerPrefs.GetString("mode") == "classic")
        {
            if (PlayerPrefs.GetString("nxn") == "3x3")
            {
                if (GameController.ticker == 3 && GameController.isMove)
                {
                    GameController.instance.SpawnFill();
                    GameController.isMove = false;
                    SoundManager.Instance.PlayMoveSound();
                }
            }
            else if (PlayerPrefs.GetString("nxn") == "4x4")
            {
                if (GameController.ticker == 4 && GameController.isMove)
                {
                    GameController.instance.SpawnFill();
                    GameController.isMove = false;
                    SoundManager.Instance.PlayMoveSound();
                }
            }
            else if (PlayerPrefs.GetString("nxn") == "5x5")
            {
                if (GameController.ticker == 5 && GameController.isMove)
                {
                    GameController.instance.SpawnFill();
                    GameController.isMove = false;
                    SoundManager.Instance.PlayMoveSound();
                }
            }
        }
        else if (PlayerPrefs.GetString("mode") == "challent")
        {
            if (whatWasSent == "w" || whatWasSent == "s")
            {
                if (GameController.ticker == GameController.instance.MaxD && GameController.isMove)
                {
                    GameController.instance.SpawnFill();
                    GameController.isMove = false;
                    SoundManager.Instance.PlayMoveSound();
                }
            }
            else if (whatWasSent == "a" || whatWasSent == "d")
            {
                if (GameController.ticker == GameController.instance.MaxN && GameController.isMove)
                {
                    GameController.instance.SpawnFill();
                    GameController.isMove = false;
                    SoundManager.Instance.PlayMoveSound();
                }
            }
        }
        else
        {
            if (GameController.ticker == 4 && GameController.isMove)
            {
                GameController.instance.SpawnFill();
                GameController.isMove = false;
                SoundManager.Instance.PlayMoveSound();
            }
        }
    }
    void SlideUp(Cell2048 currentCell)
    {
        if (currentCell.down == null)
        {
            return;
        }
        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.down;
            while (nextCell.down != null && nextCell.fill == null)
            {
                nextCell = nextCell.down;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.SetParent(currentCell.transform, true);
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
                else if (currentCell.down.fill != nextCell.fill)
                {
                    nextCell.fill.transform.SetParent(currentCell.down.transform, true);
                    currentCell.down.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.down;
            while (nextCell.down != null && nextCell.fill == null)
            {
                nextCell = nextCell.down;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.SetParent(currentCell.transform, true);
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                GameController.isMove = true;
                SlideUp(currentCell);
            }
        }
        if (currentCell.down == null) return;
        SlideUp(currentCell.down);
    }
    void SlideRight(Cell2048 currentCell)
    {
        if (currentCell.left == null)
        {
            return;
        }
        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.left;
            while (nextCell.left != null && nextCell.fill == null)
            {
                nextCell = nextCell.left;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.SetParent(currentCell.transform, true);
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
                else if (currentCell.left.fill != nextCell.fill)
                {

                    nextCell.fill.transform.SetParent(currentCell.left.transform, true);
                    currentCell.left.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.left;
            while (nextCell.left != null && nextCell.fill == null)
            {
                nextCell = nextCell.left;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.SetParent(currentCell.transform, true);
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                GameController.isMove = true;
                SlideRight(currentCell);

            }
        }
        if (currentCell.left == null) return;
        SlideRight(currentCell.left);
    }
    void SlideDown(Cell2048 currentCell)
    {
        if (currentCell.up == null)
        {
            return;
        }

        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.up;
            while (nextCell.up != null && nextCell.fill == null)
            {
                nextCell = nextCell.up;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.SetParent(currentCell.transform, true);
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
                else if (currentCell.up.fill != nextCell.fill)
                {

                    nextCell.fill.transform.SetParent(currentCell.up.transform, true);
                    currentCell.up.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.up;
            while (nextCell.up != null && nextCell.fill == null)
            {
                nextCell = nextCell.up;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.SetParent(currentCell.transform, true);
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                GameController.isMove = true;
                SlideDown(currentCell);

            }
        }
        if (currentCell.up == null) return;
        SlideDown(currentCell.up);
    }

    void SlideLeft(Cell2048 currentCell)
    {
        if (currentCell.right == null)
        {
            return;
        }

        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.right;
            while (nextCell.right != null && nextCell.fill == null)
            {
                nextCell = nextCell.right;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.SetParent(currentCell.transform, true);
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
                else if (currentCell.right.fill != nextCell.fill)
                {

                    nextCell.fill.transform.SetParent(currentCell.right.transform, true);
                    currentCell.right.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController.isMove = true;
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.right;
            while (nextCell.right != null && nextCell.fill == null)
            {
                nextCell = nextCell.right;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.SetParent(currentCell.transform, true);
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                GameController.isMove = true;
                SlideLeft(currentCell);
            }
        }
        if (currentCell.right == null) return;
        SlideLeft(currentCell.right);
    }
    public bool CheckCell()
    {
        if (fill == null) return true;
        if (up != null)
        {
            if (up.fill == null) return true;
            if (up.fill.value == fill.value) return true;
        }
        if (down != null)
        {
            if (down.fill == null) return true;
            if (down.fill.value == fill.value) return true;
        }
        if (right != null)
        {
            if (right.fill == null) return true;
            if (right.fill.value == fill.value) return true;
        }
        if (left != null)
        {
            if (left.fill == null) return true;
            if (left.fill.value == fill.value) return true;
        }
        return false;
    }

    public void undo()
    {
        // Debug.Log(this.name + " la: " + this.saveValue);
        if (this.saveValue == 0)
        {
            if (this.fill != null)
            {
                Destroy(this.fill.gameObject);
                this.fill = null;
            }
        }
        else
        {
            // Debug.Log("vao else");
            if (this.fill == null)
            {
                // Debug.Log("vao roi");
                GameObject tempFill = Instantiate(GameController.instance.fillPrefab, this.transform);
                Canvas canvas = tempFill.GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = tempFill.AddComponent<Canvas>();
                }

                canvas.overrideSorting = true;
                canvas.sortingOrder = 1000;
                Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
                this.GetComponent<Cell2048>().fill = tempFillComp;
                tempFillComp.ShowSpawnEffect();
                tempFillComp.FillValueUpdate(this.saveValue);
            }
            else
            {
                this.fill.FillValueUpdate(this.saveValue);
            }
        }
    }

    public void saveStatus()
    {
        if (this.fill == null)
        {
            // Debug.Log(this.name);
            this.saveValue = 0;
        }
        else
        {
            this.saveValue = this.fill.value;
            Debug.Log(this.name);
        }
    }
}
