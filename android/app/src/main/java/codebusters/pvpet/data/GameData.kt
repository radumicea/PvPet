package codebusters.pvpet.data

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import codebusters.pvpet.models.GameState
import codebusters.pvpet.models.Pet
import com.google.android.gms.maps.model.LatLng

object GameData {
    private val _enemyLocations = MutableLiveData<List<LatLng>>()
    val enemyLocations: LiveData<List<LatLng>> = _enemyLocations

    private val _itemLocations = MutableLiveData<List<LatLng>>()
    val itemLocations: LiveData<List<LatLng>> = _itemLocations

    private val _petState = MutableLiveData<Pet>()
    val petState: LiveData<Pet> = _petState

    fun updateGameState(gameState: GameState) {
        _enemyLocations.postValue(gameState.enemyLocations)
        _itemLocations.postValue(gameState.itemLocations)
        _petState.postValue(gameState.pet)
    }
}