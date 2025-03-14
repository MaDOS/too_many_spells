extends Node

var master_volume : float = 1
var music_volume : float = 1
var sfx_volume : float = 1

signal set_volume_music(music_volume)
signal set_volume_sfx(sfx_volume)
signal set_volume_master(master_volume)

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func master_volume_extender(value):
	master_volume = value
	set_volume_master.emit(master_volume)

func music_volume_extender(value):
	music_volume = value * master_volume
	set_volume_music.emit(music_volume)


func sfx_volume_extender(value):
	sfx_volume = value
	set_volume_sfx.emit(sfx_volume)
