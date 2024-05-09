using System.Collections.Generic;
using UnityEngine;

public class LoadCatChoose : MonoBehaviour
{
    [SerializeField] List<GameObject> List_cat;
    [SerializeField] Transform catLeft;
    [SerializeField] Transform catRight;

    private void Start()
    {
        LoadCatLeft();
        LoadCatRight();
    }
    private void LoadCatLeft()
    {
        int index = ManagerChooseCat.Instance.chooseCatLeft;
        Instantiate(List_cat[index], catLeft);
    }
    private void LoadCatRight()
    {
        int index = ManagerChooseCat.Instance.chooseCatRight;
        Instantiate(List_cat[index], catRight);
    }
}
