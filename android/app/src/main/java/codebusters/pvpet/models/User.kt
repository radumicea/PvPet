package codebusters.pvpet.models

data class User(
    val firebaseToken: String?,
    val username: String,
    val password: String,
    val pet: Pet?
)