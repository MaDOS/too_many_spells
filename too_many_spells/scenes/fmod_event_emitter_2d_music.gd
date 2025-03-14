extends FmodEventEmitter2D


var volume_master : float = 1
var volume_specific : float = 1

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	FmodManagerSingleton.connect("set_volume_music", fmodeventemitter2D_set_volume)
	FmodManagerSingleton.connect("set_volume_master", fmodeventemitter2D_set_volume_master)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	self.volume = volume_master * volume_specific

func fmodeventemitter2D_set_volume(value):
	volume_specific = value
	
func fmodeventemitter2D_set_volume_master(value):
	volume_master = value 
	
