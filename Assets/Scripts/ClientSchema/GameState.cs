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

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(participants): __participantsChange?.Invoke((MapSchema<Participant>) change.Value, (MapSchema<Participant>) change.PreviousValue); break;
			default: break;
		}
	}
}

