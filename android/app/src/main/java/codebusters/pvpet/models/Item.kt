package codebusters.pvpet.models

import java.util.UUID

data class PetItem(val id: String? = null)
data class ShopItem(
    var id : String?,
var pictocode: Int?,
    var type: String?,
var attack: Int?,
var armor: Int?,
var attackSpeed: Double?,
var crit: Int?,
var hp: Int?,
var food: Int?,
var gold: Int?,
var price: Int?,
var petId: String?
)