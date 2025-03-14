extends FmodEventEmitter2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	FmodManagerSingleton.connect("set_volume", fmodeventemitter2D_set_volume)
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func fmodeventemitter2D_set_volume(value):
	self.volume = value
	
