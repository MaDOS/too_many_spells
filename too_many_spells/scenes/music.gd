extends Node2D

@onready var _musicEmitter: FmodEventEmitter2D = $MusicEmitter

func _ready() -> void:
	print("music script init")	
	GameStateManager.MusicChange.connect(music_change)
	
func music_change(parameter: String) -> void:	
	print("music change called " + parameter)
	#_musicEmitter.set_parameter("SceneChange", 1.0)
	#var paramEmitter = _musicEmitter.get_parameter_by_id(-17628221880779407)

	#print("paramEmitter: " + str(paramEmitter))
	
	#_musicEmitter.set_parameter_by_id(-17628221880779407, 1.0)
	#_musicEmitter.play()
