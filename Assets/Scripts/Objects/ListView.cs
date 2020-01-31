using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListView : MonoBehaviour, IListView {
    public VerticalLayoutGroup layoutGroup;
    public RectTransform rectLayout;

    public IListViewDelegate Delegate {
        get { return listDelegate; }
        set { listDelegate = value; }
    }
    public IListViewDatasource Datasource {
        get { return listDatasource; }
        set { listDatasource = value; }
    }
    public List<ListViewItem> Items { get { return items; } }
    private IListViewDelegate listDelegate;
    private IListViewDatasource listDatasource;
    private List<ListViewItem> items;

    private void OnEnable () {
        ClearListView ();
        CreateList ();
    }
    private void ClearListView () {
        if (items == null) {
            items = new List<ListViewItem> ();
            return;
        }
        if (items != null || items.Count != 0) {
            foreach (ListViewItem item in items) {
                Destroy (item.gameObject);
            }
            items.Clear ();
        }
    }
    private void CreateList () {
        int itemCount = listDatasource.RowCountInList ();
        CalculateSize (itemCount);

        for (int i = 0; i < itemCount; i++) {
            ListViewItem temp = listDatasource.CellOfRow (i);
            temp.transform.SetParent (layoutGroup.transform);
            temp.transform.localScale = Vector3.one;
            temp.ID = i;
            int tempId = i;
            temp.OnSelected = delegate { listDelegate.OnSelectRow (tempId); };
            items.Add (temp);
        }
    }
    private void CalculateSize (int itemCount) {
        float size = 0;
        for (int i = 0; i < itemCount; i++) {
            size += listDatasource.SizeOfRow (i) + layoutGroup.spacing;
        }
        rectLayout.sizeDelta = new Vector2 (rectLayout.sizeDelta.x, size);
    }

}

public interface IListView {
    IListViewDelegate Delegate { get; set; }
    IListViewDatasource Datasource { get; set; }
    List<ListViewItem> Items { get; }
}
public interface IListViewDelegate {
    void OnSelectRow (int id);
}
public interface IListViewDatasource {
    int RowCountInList ();
    ListViewItem CellOfRow (int id);
    float SizeOfRow (int row);
}