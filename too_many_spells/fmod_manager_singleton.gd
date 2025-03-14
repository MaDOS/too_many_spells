extends Node

var master_volume : float = 1

signal set_volume(master_volume)
signal play_music_singleton

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func master_volume_extender(value):
	master_volume = value
	set_volume.emit(master_volume)
	
func playmusic_singleton():
	play_music_singleton.emit()
