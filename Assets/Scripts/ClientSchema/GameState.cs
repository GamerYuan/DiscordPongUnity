// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.32
// 

using Colyseus.Schema;
using Action = System.Action;

public partial class GameState : Schema {
	[Type(0, "map", typeof(MapSchema<Participant>))]
	public MapSchema<Participant> participants = new MapSchema<Participant>();

	[Type(1, "ref", typeof(Ball))]
	public Ball ball = new Ball();

	[Type(2, "map", typeof(MapSchema<int>), "int32")]
	public MapSchema<int> scoreboard = new MapSchema<int>();

	[Type(3, "map", typeof(MapSchema<string>), "string")]
	public MapSchema<string> usernames = new MapSchema<string>();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<MapSchema<Participant>> __participantsChange;
	public Action OnParticipantsChange(PropertyChangeHandler<MapSchema<Participant>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.participants));
		__participantsChange += __handler;
		if (__immediate && this.participants != null) { __handler(this.participants, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(participants));
			__participantsChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Ball> __ballChange;
	public Action OnBallChange(PropertyChangeHandler<Ball> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.ball));
		__ballChange += __handler;
		if (__immediate && this.ball != null) { __handler(this.ball, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(ball));
			__ballChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<MapSchema<int>> __scoreboardChange;
	public Action OnScoreboardChange(PropertyChangeHandler<MapSchema<int>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.scoreboard));
		__scoreboardChange += __handler;
		if (__immediate && this.scoreboard != null) { __handler(this.scoreboard, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(scoreboard));
			__scoreboardChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<MapSchema<string>> __usernamesChange;
	public Action OnUsernamesChange(PropertyChangeHandler<MapSchema<string>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.usernames));
		__usernamesChange += __handler;
		if (__immediate && this.usernames != null) { __handler(this.usernames, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(usernames));
			__usernamesChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(participants): __participantsChange?.Invoke((MapSchema<Participant>) change.Value, (MapSchema<Participant>) change.PreviousValue); break;
			case nameof(ball): __ballChange?.Invoke((Ball) change.Value, (Ball) change.PreviousValue); break;
			case nameof(scoreboard): __scoreboardChange?.Invoke((MapSchema<int>) change.Value, (MapSchema<int>) change.PreviousValue); break;
			case nameof(usernames): __usernamesChange?.Invoke((MapSchema<string>) change.Value, (MapSchema<string>) change.PreviousValue); break;
			default: break;
		}
	}
}

