using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class PanelScoreBoard : MonoBehaviour, IListViewDatasource, IListViewDelegate {
    public ListView listView;
    public Button buttonClose;
    public CanvasGroup canvasGroup;
    public ScoreInfo[] scoresInfo;
    public ScoreboardItem scoreItemPrefab;
    public Color[] skinColors;
    private void OnEnable () {
        Show ();
    }
    private void Start () {
        listView.Datasource = this;
        listView.Delegate = this;
        buttonClose.onClick.AddListener (Hide);
    }

    public void Show () {
        canvasGroup.alpha = 0;
        canvasGroup.LeanAlpha (1, .4f).setOnComplete (() => {

        });
    }
    public void Hide () {
        canvasGroup.LeanAlpha (0, .4f).setOnComplete (() => {
            gameObject.SetActive (false);
            listView.gameObject.SetActive (false);
        });
    }
    public void SetScoreInfo (ScoreInfo[] infos) {
        scoresInfo = infos;
        StartCoroutine (DelayToShowScoreBoard ());
    }
    private IEnumerator DelayToShowScoreBoard () {
        SortInfo ();
        yield return null;
        OpenContent ();
    }
    private void SortInfo () {
        for (int i = 0; i < scoresInfo.Length - 1; i++) {
            for (int j = i + 1; j < scoresInfo.Length; j++) {
                if (scoresInfo[i].score < scoresInfo[j].score) {
                    ScoreInfo temp = scoresInfo[j];
                    scoresInfo[j] = scoresInfo[i];
                    scoresInfo[i] = temp;
                }
            }
        }
    }
    #region LIST VIEW
    public ListViewItem CellOfRow (int id) {
        ScoreboardItem temp = Instantiate (scoreItemPrefab, listView.transform);
        temp.transform.localScale = Vector3.one;
        temp.SetScore (scoresInfo[id]);
        temp.SetScolor (skinColors[scoresInfo[id].skinId]);
        return temp;
    }
    public void OpenContent () {
        listView.gameObject.SetActive (true);
    }
    public void OnSelectRow (int id) {
        Debug.Log ($"select row {id}");
    }

    public int RowCountInList () {
        return scoresInfo.Length;
    }

    public float SizeOfRow (int row) {
        return 100;
    }
    #endregion
}