package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.Pet
import codebusters.pvpet.providers.HttpClient
import com.google.android.gms.maps.model.LatLng
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

object GameDataAccessor {
    fun updatePetLocation(pet: Pet): List<LatLng>? {
        val responseJson = HttpClient.patch(
            "${BuildConfig.API_URI}/Game/updatePetLocation",
            pet
        ) ?: return null

        return Gson().fromJson(responseJson, object : TypeToken<List<LatLng>>() {}.type)
    }
}