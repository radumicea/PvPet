package codebusters.pvpet.models

data class Pet(
    val id: String,
    val name: String? = null,
    val variant: String? = null,
    val hp: Int? = null,
    val food: Int? = null,
    val starvation: Int? = null,
    val attack: Int? = null,
    val armor: Int? = null,
    val attackSpeed: Int? = null,
    val crit: Int? = null,
    val latitude: Double? = null,
    val longitude: Double? = null,
    val accountId: String? = null
)