extends Node2D

@onready var settings_panel_container: PanelContainer = $UIMainMenu/SettingsPanelContainer
@onready var audio_settings_manager: Control = $UIMainMenu/AudioSettingsManager
@onready var credits_control: Control = $UIMainMenu/CreditsControl
@onready var tablet_animation: AnimatedSprite2D = $UIMainMenu/Panel/TabletAnimation


signal main_menu_start
signal main_menu_play_clicked
signal main_menu_options_button_pressed

var mouse_in_tablet : bool
var tablet_open : bool

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	show()
	tablet_open = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if mouse_in_tablet == true and Input.is_action_just_pressed("click") and tablet_open == false:
		tablet_animation.play("tablet_animated")
		tablet_open = true
			

func _on_start_game_button_pressed() -> void:
	main_menu_play_clicked.emit()

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


func _on_area_2d_mouse_entered() -> void:
	mouse_in_tablet = true
	print("true")

func _on_area_2d_mouse_exited() -> void:
	mouse_in_tablet = false
	print ("false")
