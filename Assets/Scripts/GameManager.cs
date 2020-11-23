using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public GameUI UIManager;
    public Player[] players;
    public Board board;
    public StateMachine stateMachine;
    [SerializeField] ConstructionData[] UIExcludedConstructions;
    public Func<Construction, bool> constructionExclusionLambda;

    [Header("Day/Night Cycle")]
    public float dayLength = 30f;
    public float nightIntensity = 0;
    [SerializeField] Material[] nightGlowMats;
    Dictionary<Material, Color> initialColors = new Dictionary<Material, Color>();

    public int currentPlayer = 1;
    public static Player CurrentPlayer => GameManager.manager.players[GameManager.manager.currentPlayer];

    private void Start()
    {
        //constructionExclusionLambda = x => foreach(ConstructionData d in UIExcludedConstructions) if(x == )
        if (manager == null) manager = this;
        else Destroy(this);
        foreach (Material m in nightGlowMats) initialColors.Add(m, m.GetColor("_EmissionColor"));
        stateMachine.ChangeState<CGSMainMenu>();
    }

    private void Update()
    {
        //nightIntensity = Mathf.Clamp01(Mathf.Abs(Mathf.Sin(Mathf.PI * Time.time / GameManager.manager.dayLength)));
        //foreach(KeyValuePair<Material, Color> m in initialColors) m.Key.SetColor("_EmissionColor", m.Value * nightIntensity);
    }

    private void OnApplicationQuit()
    {
        foreach (KeyValuePair<Material, Color> m in initialColors) m.Key.SetColor("_EmissionColor", m.Value * 1f);
    }

    public void StartGame()
    {
        stateMachine.ChangeState<CGSInitial>();
    }

    public void AdvanceTurn()
    {
        stateMachine.ChangeState<CGSBotTurn>();
    }

    public void WinGame(int player)
    {
        stateMachine.ChangeState<CGSWin>();
    }

    public void LoseGame(int player)
    {
        stateMachine.ChangeState<CGSLose>();
    }
}
