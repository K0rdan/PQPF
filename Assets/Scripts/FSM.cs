using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FSMState
{
	public string Name;
	public FSM Parent;
	public List<FSMTransition> Transitions;
	
	public FSMState (string name)
	{
		Name = name;
		Transitions = new List<FSMTransition> ();
	}
	
	public void AddTransition (FSMTransition trans)
	{
		// Check if anyone of the args is invalid
		if (trans == null) {
			Debug.LogError ("FSMState ERROR: NullTransition is not allowed for a real transition");
			return;
		}
		
		// Since this is a Deterministic FSM,
		//   check if the current transition was already inside the map
		if (Transitions.Contains (trans)) {
			Debug.LogError ("FSMState ERROR: State " + Name + " already has transition " + trans.Name + 
				"Impossible to assign to another state");
			return;
		}

		// Insertion sort
		int i = 0;
		while ((i < Transitions.Count) && (Transitions[i].Priority < trans.Priority))
		{
			i++;
		}

		Transitions.Insert(i, trans);
		trans.LinkState (this);
	}
	
	/// <summary>
	/// This method deletes a pair transition-state from this state's map.
	/// If the transition was not inside the state's map, an ERROR message is printed.
	/// </summary>
	public void DeleteTransition (FSMTransition trans)
	{
		// Check for NullTransition
		if (trans == null) {
			Debug.LogError ("FSMState ERROR: NullTransition is not allowed");
			return;
		}
		
		// Check if the pair is inside the map before deleting
		if (Transitions.Contains (trans)) {
			Transitions.Remove (trans);
			return;
		}
		Debug.LogError ("FSMState ERROR: Transition " + trans.Name + " passed to " + Name + 
			" was not on the state's transition list");
	}
	
	public bool CheckTransitions ()
	{
		//Debug.Log(Transitions.Count.ToString() + " TRANSITIONS FROM " + Name + " TO CHECK...");
		foreach (FSMTransition trans in Transitions) {
			//Debug.Log("CHECKING TRANSITION : " + trans.Name);
			if (trans.Check ()) {
				Parent.PerformTransition (trans);
				return true;
			}
		}
		return false;
	}
	
	public virtual void DoBeforeEntering ()
	{
	}
	
	public virtual void DoBeforeLeaving ()
	{
	}
	
	public abstract void Do ();
	
	public virtual bool IsDone ()
	{
		return false;
	}
} // class FSMState

public abstract class FSM : FSMState
{
	private List<FSMState> States;
	protected FSMState initState;
	protected FSMState currentState;

	public FSMState CurrentState { get { return currentState; } }
	
	public FSM (string name) : base(name)
	{
		States = new List<FSMState> ();
	}
	
	public void AddState (FSMState s)
	{
		// Check for Null reference before deleting
		if (s == null) {
			Debug.LogError ("FSM ERROR: Null reference for FSMState is not allowed");
		}
		
		// First State inserted is also the Initial state,
		//   the state the machine is in when the simulation begins
		if (States.Count == 0) {
			States.Add (s);
			s.Parent = this;
			initState = s;
			currentState = s;
			return;
		}
		
		// Add the state to the List if it's not inside it
		foreach (FSMState state in States) {
			if (state.Name == s.Name) {
				Debug.LogError ("FSM ERROR: Impossible to add state " + s.Name + 
					" because state has already been added");
				return;
			}
		}
		States.Add (s);
		s.Parent = this;
	}
	
	public void DeleteState (string stateName)
	{
		// Check for NullState before deleting
		if (stateName == CurrentState.Name) {
			Debug.LogError ("FSM ERROR: Null string is not allowed for a real state");
			return;
		}
		
		// Search the List and delete the state if it's inside it
		foreach (FSMState state in States) {
			if (stateName == state.Name) {
				States.Remove (state);
				return;
			}
		}
		Debug.LogError ("FSM ERROR: Impossible to delete state " + stateName + 
			". It was not on the list of states");
	}

	public void PerformTransition (FSMTransition trans)
	{
		if (trans == null) {
			Debug.LogError ("FSMState ERROR: NullTransition is not allowed");
			return;
		}
		
		if (currentState.Transitions.Contains (trans)) {
			//Debug.Log("Performing [" + trans.ToString() + "] - From " + trans.FromState.ToString() + " To " + trans.ToState.ToString());

			currentState.DoBeforeLeaving ();

			//Debug.Log (currentState.ToString());
			currentState = trans.ToState;

			//Debug.Log (currentState.ToString() + " - Entering from FSM");
			currentState.DoBeforeEntering ();
			//Debug.Log (currentState.ToString());

			return;
		}
		Debug.LogError ("FSMState ERROR: Transition " + trans.Name + " passed to " + Name + 
			" was not on the state's transition list");
	} // PerformTransition()
	
	public override void Do ()
	{
		if (CurrentState.CheckTransitions ())
		{
			//Debug.Log("CurrentState Transition checked and performed");
			return;
		}

		if (CheckTransitions ())
		{
			//Debug.Log("Transition checked and performed");
			return;
		}

		CurrentState.Do ();
	}
	
	public override bool IsDone ()
	{
		//Debug.Log ("Checking if FSM is Done");
		return CurrentState.IsDone () && (CurrentState.Transitions.Count == 0);
	}

	public override void DoBeforeEntering ()
	{
		Debug.Log ("Initializing FSM");
		currentState = initState;
		currentState.DoBeforeEntering ();
	}

	public override void DoBeforeLeaving ()
	{
		currentState.DoBeforeLeaving ();
	}
} // class FSM

public abstract class FSMTransition
{
	public string Name;
	public FSMState FromState;
	public FSMState ToState;
	public int Priority;
	
	public FSMTransition (string name, FSMState toState, int priority = 5)
	{
		Name = name;
		ToState = toState;
		Priority = priority;
	}
	
	public void LinkState (FSMState state)
	{
		FromState = state;
	}
	
	public abstract bool Check ();
	
	public static int ComparePriority (FSMTransition t1, FSMTransition t2)
	{
		if (t1 == null) {
			if (t2 == null) {
				return 0;
			}
			return 1;
		}
		if (t2 == null) {
			return -1;
		}
		
		if (t1.Priority > t2.Priority) {
			return 1;
		} else {
			return -1;
		}
	}
} //class FSMSystem