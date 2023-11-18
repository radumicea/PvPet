package codebusters.pvpet.models

import com.google.android.gms.maps.model.LatLng

data class Pet(
    val id: String,
    val name: String? = null,
    val variant: String? = null,
    val hp: Int? = null,
    val food: Int? = null,
    val starvation: Int? = null,
    val attack: Int? = null,
    val armor: Int? = null,
    val attackSpeed: Double? = null,
    val crit: Int? = null,
    val location: LatLng? = null,
    val accountId: String? = null
)