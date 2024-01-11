package codebusters.pvpet.models

import com.google.android.gms.maps.model.LatLng

data class Pet(
    val id: String? = null,
    val name: String? = null,
    val variant: Int? = null,
    val hp: Int? = null,
    val food: Int? = null,
    val gold: Int? = null,
    val attack: Int? = null,
    val armor: Int? = null,
    val attackSpeed: Double? = null,
    val crit: Int? = null,
    val cooldownSeconds: Int? = null,
    val location: LatLng? = null,
    val secondsToRestockShop: Int? = null,
    val userId: String? = null,
    val items: List<PetItem>? = null,
    val shopItems: List<ShopItem>? = null,
    val user: List<User>? = null,
    val fights: List<PetFight>? = null,
)