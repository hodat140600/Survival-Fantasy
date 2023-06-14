using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class DialogManager : MonoBehaviour
{
    public DialogPanel dialogHero;
    public DialogPanel dialogBoss;
    public int index=0;
    public List<DialogInfo> dialogInfos;

    private void Start()
    {
        SetActionDialog(dialogInfos[index], GetDialogPanel(dialogInfos[index].dialogPanel));
    }

    public void SetActionDialog(DialogInfo dialogInfo, DialogPanel dialogPanel)
    {
        dialogPanel.dialogTxt = dialogInfo.text;
        dialogPanel.buttonContinue.GetComponent<Button>().onClick.RemoveAllListeners();
        dialogPanel.buttonContinue.GetComponent<Button>().onClick.AddListener(() => 
        {
            dialogPanel.gameObject.SetActive(false);
            ActionOnDialog(dialogInfo.isEnd); 
        });
        dialogPanel.gameObject.SetActive(true);
    }

    private void EndDialogClick()
    {
        Time.timeScale = 1;
        SceneManager.UnloadScene((int)SceneIndex.Cutscene);
        LoadingScene.Instance.LoadScene();
    }
    private void ContinueDialogClick()
    {
        if (dialogInfos[index ].canStop)
        {
            gameObject.SetActive(false);
            SendMessage(dialogInfos[index].typeEvent);
        }
        index++;
        SetActionDialog(dialogInfos[index], GetDialogPanel(dialogInfos[index].dialogPanel));
    }
    private void ActionOnDialog(bool check)
    {
        switch (check) { 
            case true:
                EndDialogClick();
                break;
            case false:
                ContinueDialogClick();
                break;
        }
    }
    private DialogPanel GetDialogPanel(TypeDialog typeDialog)
    {
        DialogPanel dialogPanel = null;
        switch (typeDialog)
        {
            case TypeDialog.Hero:
                dialogPanel = dialogHero;
                break;
            case TypeDialog.Boss:
                dialogPanel = dialogBoss;
                break;
        }
        return dialogPanel;
    }
    void SendMessage(TypeEvent typeEvent)
    {
        switch (typeEvent)
        {
            case TypeEvent.SpawnMinion:
                MessageBroker.Default.Publish(new SpawnMinionEvent());
                break;
            case TypeEvent.SummonBoss:
                MessageBroker.Default.Publish(new SummonBossEvent());
                break;
        }
        
    }
}

[System.Serializable]
public class DialogInfo
{
    public TypeDialog dialogPanel;
    [TextArea(3, 10)]
    public string text;
    public bool canStop;
    [ConditionalHide("canStop", true)]
    public TypeEvent typeEvent;
    public bool isEnd;
}
public enum TypeDialog
{
    Hero,
    Boss
}
public enum TypeEvent
{
    SpawnMinion,
    SummonBoss
}