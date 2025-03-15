extends Node2D

@onready var audio_settings_manager: Control = $UIMainMenu/AudioSettingsManager
@onready var credits_control: Control = $UIMainMenu/CreditsControl
@onready var tablet_animation: AnimatedSprite2D = $UIMainMenu/Panel/TabletAnimation
@onready var sound_control: Control = $UIMainMenu/SoundControl
@onready var animation_player: AnimationPlayer = $UIMainMenu/AnimationPlayer
@onready var panel_2: Panel = $UIMainMenu/AnimationPlayer/Panel2
@onready var poster: AnimatedSprite2D = $UIMainMenu/Panel/Sprite2D/Poster
@onready var book: AnimatedSprite2D = $UIMainMenu/Panel/Sprite2D/Book
@onready var tuer: AnimatedSprite2D = $UIMainMenu/Panel/Sprite2D/Tuer
@onready var controls: Control = $UIMainMenu/Controls



signal main_menu_start
signal main_menu_play_clicked
signal main_menu_options_button_pressed

var mouse_in_tablet : bool
var tablet_open : bool

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	show()
	tablet_open = false
	animation_player.play("fade out")
	await animation_player.animation_finished
	panel_2.hide()
	print("hidden")
	
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if mouse_in_tablet == true and Input.is_action_just_pressed("click") and tablet_open == false:
		tablet_animation.play("tablet_animated")
		tablet_open = true
		sound_control.show()
			

#Main Menu Buttons and Controls

func _on_start_game_button_pressed() -> void:
	main_menu_play_clicked.emit()

func _on_options_button_pressed() -> void:
	sound_control.show()
	main_menu_options_button_pressed.emit()

func _on_exit_button_pressed() -> void:
	get_tree().quit()

func _on_credits_pressed() -> void:
	credits_control.show()

func _on_area_2d_mouse_entered() -> void:
	mouse_in_tablet = true

func _on_area_2d_mouse_exited() -> void:
	mouse_in_tablet = false

#Tablet Buttons and Controls

func _on_sound_button_pressed() -> void:
	audio_settings_manager.show()

func _on_exit_credits_pressed() -> void:
	credits_control.hide()

func _on_exit_settings_button_pressed() -> void:
	tablet_animation.play_backwards()
	tablet_open = false
	sound_control.hide()

func _on_reset_save_button_pressed() -> void:
	GameStateManager.ResetSave()


func _on_credits_mouse_entered() -> void:
	poster.play("poster_shimmer")


func _on_credits_mouse_exited() -> void:
	poster.play("poster_default")


func _on_start_game_button_mouse_entered() -> void:
	book.play("book_shimmer")


func _on_start_game_button_mouse_exited() -> void:
	book.play("book_default")


func _on_exit_button_mouse_entered() -> void:
	tuer.play("tuer_shimmer")


func _on_exit_button_mouse_exited() -> void:
	tuer.play("tuer_default")


func _on_controls_button_pressed() -> void:
	controls.show()


func _on_exit_controls_pressed() -> void:
	controls.hide()
