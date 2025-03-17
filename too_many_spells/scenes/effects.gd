extends Node2D

@onready var _closeBookEmitter: FmodEventEmitter2D = $CloseBookEmitter
@onready var _leafingEmitter: FmodEventEmitter2D = $LeafingEmitter
@onready var _openBookEmitter: FmodEventEmitter2D = $OpenBookEmitter
@onready var _spellBadEmitter: FmodEventEmitter2D = $SpellBadEmitter
@onready var _spellNeutralEmitter: FmodEventEmitter2D = $SpellNeutralEmitter
@onready var _spellGoodEmitter: FmodEventEmitter2D = $SpellGoodEmitter
@onready var _spellPerfectEmitter: FmodEventEmitter2D = $SpellPerfectEmitter
@onready var _dragPlopEmitter: FmodEventEmitter2D = $DragPlopEmitter
@onready var _dropPlopEmitter: FmodEventEmitter2D = $DropPlopEmitter
@onready var _trashcanEmitter: FmodEventEmitter2D = $TrashcanEmitter
@onready var _catEmitter: FmodEventEmitter2D = $CatEmitter

func _ready() -> void:
	print("effect script init")	
	GameStateManager.PlayEffect.connect(play_effect)
	
func play_effect(parameter: String) -> void:	
	print("play effect called" + parameter)

	if parameter == "CloseBook":
		_closeBookEmitter.play_one_shot()
	elif parameter == "Leafing":
		_leafingEmitter.play_one_shot()
	elif parameter == "OpenBook":
		_openBookEmitter.play_one_shot()
	elif parameter == "SpellBad":
		_spellBadEmitter.play_one_shot()
	elif parameter == "SpellNeutral":
		_spellNeutralEmitter.stplay_one_shotart()
	elif parameter == "SpellGood":
		_spellGoodEmitter.play_one_shot()
	elif parameter == "SpellPerfect":
		_spellPerfectEmitter.play_one_shot()
	elif parameter == "DragPlop":
		_dragPlopEmitter.play_one_shot()
	elif parameter == "DropPlop":
		_dropPlopEmitter.play_one_shot()
	elif parameter == "Trashcan":
		_trashcanEmitter.play_one_shot()
	elif parameter == "Cat":
		_catEmitter.play_one_shot()
	else:
		print("No effect found for " + parameter)
