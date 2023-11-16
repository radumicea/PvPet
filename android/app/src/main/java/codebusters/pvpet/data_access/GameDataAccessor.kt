package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.Pet
import codebusters.pvpet.providers.HttpClient

data class Location(val latitude: Double, val longitude: Double)

object GameDataAccessor {
    fun updatePetLocation(pet: Pet): List<Location>? {
        return HttpClient.patch<List<Location>>(
            "${BuildConfig.API_URI}/Game/updatePetLocation",
            pet
        )
    }
}