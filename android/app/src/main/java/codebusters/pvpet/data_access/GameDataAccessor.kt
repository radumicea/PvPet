package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.Pet
import codebusters.pvpet.providers.HttpClient
import com.google.android.gms.maps.model.LatLng

object GameDataAccessor {
    fun updatePetLocation(pet: Pet): List<LatLng> {
        return HttpClient.patch(
            "${BuildConfig.API_URI}/Game/updatePetLocation",
            pet, HttpClient.JsonType.COLLECTION, LatLng::class.java
        ) as List<LatLng>
    }
}