extends Control

@onready var audio_window: Panel = $AudioWindow
@onready var master_volume_slider: HSlider = $AudioWindow/VBoxContainer/MasterVolumeSlider
@onready var music_volume_slider: HSlider = $AudioWindow/VBoxContainer/MusicVolumeSlider
@onready var sfx_volume_slider: HSlider = $AudioWindow/VBoxContainer/SFXVolumeSlider

var master_index : int
var music_index : int
var sfx_index : int

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	master_index = AudioServer.get_bus_index("Master")
	music_index = AudioServer.get_bus_index("Music")
	sfx_index = AudioServer.get_bus_index("SFX")
	
	master_volume_slider.connect("value_changed", FmodManagerSingleton.master_volume_extender)
	
func _process(delta: float) -> void:
	pass

func _get_volume (bus_index : int) -> float:
	var db_volume = AudioServer.get_bus_volume_db(bus_index)
	return db_to_linear(db_volume)

func _set_volume (bus_index : int, volume : float):
	var db_volume = linear_to_db(volume)
	AudioServer.set_bus_volume_db(bus_index, db_volume)

# Slider change leads to change of AudioBus volume 

func _on_master_volume_slider_value_changed(value: float) -> void:
	_set_volume(master_index, value)

func _on_music_volume_slider_value_changed(value: float) -> void:
	_set_volume(music_index, value)

func _on_sfx_volume_slider_value_changed(value: float) -> void:
	_set_volume(sfx_index, value)


func _on_button_options_pressed() -> void:
	audio_window.visible = !audio_window.visible

func _on_return_pressed() -> void:
	self.hide()
