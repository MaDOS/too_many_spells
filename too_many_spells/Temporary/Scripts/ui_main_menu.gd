extends Control

@onready var settings_panel_container: PanelContainer = $SettingsPanelContainer
@onready var audio_settings_manager: Control = $AudioSettingsManager
@onready var credits_control: Control = $CreditsControl

signal main_menu_start
signal main_menu_options_button_pressed

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	show()
	main_menu_start.emit()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	

func _on_start_game_button_pressed() -> void:
	pass

func _on_options_button_pressed() -> void:
	settings_panel_container.show()
	main_menu_options_button_pressed.emit()

func _on_exit_button_pressed() -> void:
	get_tree().quit()


func _on_sound_button_pressed() -> void:
	audio_settings_manager.show()
	
func _on_credits_pressed() -> void:
	credits_control.show()

func _on_exit_credits_pressed() -> void:
	credits_control.hide()

func _on_game_toggle_game_paused():
	self.visible = !self.visible
	
func _on_resume_button_pressed() -> void:
	show()
	#game_root_node.pause_status = false
