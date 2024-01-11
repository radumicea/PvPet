package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.GameState
import codebusters.pvpet.models.Pet
import codebusters.pvpet.providers.HttpClient
import com.google.gson.reflect.TypeToken

object GameLoopDataAccessor {
    fun updatePetState(pet: Pet): GameState {
        return HttpClient.patch(
            "${BuildConfig.API_URI}/GameLoop/updateGameState",
            pet,
            object : TypeToken<GameState>() {})
    }
}