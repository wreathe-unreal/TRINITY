using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class ACrabFSM : MonoBehaviour, IFSM
{
    [Tooltip("The state used to start the state machine.")]
    public CrabState InitialState;

    private readonly Dictionary<string, CrabState> states = new Dictionary<string, CrabState>();
    private Queue<CrabState> TransitionQueue = new Queue<CrabState>();

    public CrabState CurrentState { get; private set; }
    public CrabState PreviousState { get; private set; }

    public event Action<CrabState, CrabState> OnStateChange;
    public ACrabController CrabController;
    public ATrinityController PlayerController;
    public Animator Animator;
    private bool FSM_RUNNING = false;

    private void Awake()
    {
        InitializeStates();
    }

    private void FixedUpdate()
    {
        Debug.Log(TransitionQueue.Count);
        if (!FSM_RUNNING)
        {
            if (InitialState == null || !InitialState.isActiveAndEnabled)
            {
                Debug.LogError("FSM: Initial state is null or inactive. State machine will not start.");
                enabled = false;
                return;
            }

            StartStateMachine();
        }

        if (!PlayerController)
        {
            PlayerController = FindObjectOfType<ATrinityController>();

            if (!PlayerController)
            {
                return;
            }
        }

        // Check for transitions and update the current state
        ProcessTransitions();

        if (CurrentState != null)
        {
            CurrentState.PreUpdateBehaviour(Time.deltaTime);
            CurrentState.UpdateBehaviour(Time.deltaTime);
            CurrentState.PostUpdateBehaviour(Time.deltaTime);
        }
    }

    private void StartStateMachine()
    {
        FSM_RUNNING = true;
        CurrentState = InitialState;
        CurrentState.EnterBehaviour(0f, null);
    }

    private void ProcessTransitions()
    {
        if (TransitionQueue.Count > 0)
        {
            while (TransitionQueue.Count > 0)
            {
                CrabState nextState = TransitionQueue.Dequeue();

                if (nextState != null && nextState.isActiveAndEnabled)
                {
                    TransitionToState(nextState);
                    break;
                }
            }
        }
    }

    private void TransitionToState(CrabState nextState)
    {
        if (CurrentState == nextState || !CurrentState.CheckExitTransition(nextState))
            return;

        PreviousState = CurrentState;
        CurrentState.ExitBehaviour(Time.deltaTime, nextState);

        OnStateChange?.Invoke(PreviousState, nextState);

        Debug.Log("AI: "  + CurrentState + "=>" + nextState);

        CurrentState = nextState;
        Animator.runtimeAnimatorController = CurrentState.StateAnimController;
        CurrentState.EnterBehaviour(Time.deltaTime, PreviousState);
    }

    public CrabState GetState(string stateName)
    {
        states.TryGetValue(stateName, out CrabState state);
        return state;
    }

    public T GetState<T>() where T : CrabState
    {
        string stateName = typeof(T).Name;
        return GetState(stateName) as T;
    }

    public void EnqueueTransition(string stateName)
    {
        CrabState state = GetState(stateName);
        if (state != null)
        {
            TransitionQueue.Enqueue(state);
        }
    }

    public void EnqueueTransition<T>() where T : CrabState
    {
        CrabState state = GetState<T>();
        if (state != null)
        {
            TransitionQueue.Enqueue(state);
        }
    }

    public void EnqueueTransitionToPreviousState()
    {
        if (PreviousState != null)
        {
            TransitionQueue.Enqueue(PreviousState);
        }
    }

    private void InitializeStates()
    {
        CrabState[] statesArray = GetComponents<CrabState>();

        foreach (var state in statesArray)
        {
            string stateName = state.GetType().Name;

            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
                state.SetStateMachine(this);
            }
            else
            {
                Debug.LogWarning($"FSM: Duplicate state '{stateName}' found in {state.gameObject.name}. Skipping...");
            }
        }
    }
}

