extends FmodEventEmitter2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	FmodManagerSingleton.connect("set_volume", fmodeventemitter2D_set_volume)
	FmodManagerSingleton.connect("play_music_singleton", fmodeventemitter2D_play_song)
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func fmodeventemitter2D_set_volume(value):
	self.volume = value
	
func fmodeventemitter2D_play_song():
	#event_guid = "{81d26a96-93bc-411e-9fb5-071afeb4c6e7}"
	play()
