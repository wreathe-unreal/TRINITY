using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue: CrabState
{

    public override void CheckEnterTransition()
    {
        //base.CheckEnterTransition();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // Custom behavior when entering the state
        Debug.Log("Entering Pursue State");
    }

    public override void EnterBehaviour(float dt, IState fromState)
    {
        //DO NOT DELETE
        base.EnterBehaviour(dt, fromState); //sets animator controller
                                            //DO NOT DELETE

    }

    public override void PreUpdateBehaviour(float dt)
    {
        //base.PreUpdateBehaviour(dt);
    }


    public override void UpdateBehaviour(float dt)
    {
        //base.UpdateBehaviour(dt);
    }


    public override void PostUpdateBehaviour(float dt)
    {
        //base.PostUpdateBehaviour(dt);
    }

    public override void ExitBehaviour(float dt, IState toState)
    {
        //base.ExitBehaviour(dt, toState);
    }

    public override void CheckExitTransition()
    {
        //base.CheckExitTransition();
    }

    public override void OnExit()
    {
        //base.OnExit();
    }

    public override void FixedUpdate()
    {
        //base.FixedUpdate();
    }

    public override void SetStateMachine(ACrabFSM aCrabStateMachine)
    {
        //DO NOT DELETE
        base.SetStateMachine(aCrabStateMachine); // Set state machine variable
        //DO NOT DELETE

    }
}