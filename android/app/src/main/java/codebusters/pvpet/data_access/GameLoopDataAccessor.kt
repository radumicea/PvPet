package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.Pet
import codebusters.pvpet.providers.HttpClient
import com.google.android.gms.maps.model.LatLng
import com.google.gson.reflect.TypeToken

object GameLoopDataAccessor {
    fun updatePetLocation(pet: Pet): List<LatLng> {
        return HttpClient.patch(
            "${BuildConfig.API_URI}/Game/updatePetLocation",
            pet,
            object : TypeToken<List<LatLng>>() {})
    }
}