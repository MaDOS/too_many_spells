extends Node2D

@onready var wd_desk_1: AudioStreamPlayer2D = $WD_Desk1
@onready var wd_desk_2: AudioStreamPlayer2D = $WD_Desk2
@onready var wd_fair_2: AudioStreamPlayer2D = $WD_Fair2


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_top_node_ui_main_menu_main_menu_options_button_pressed() -> void:
	wd_desk_1.stop()
	await get_tree().create_timer(1.0).timeout
	wd_fair_2.play()


func _on_settings_panel_container_settings_closed() -> void:
	wd_fair_2.stop()
	await get_tree().create_timer(1.0).timeout
	wd_desk_1.play()


func _on_top_node_ui_main_menu_main_menu_start() -> void:
	wd_desk_1.play()
