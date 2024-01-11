package codebusters.pvpet.models

import com.google.android.gms.maps.model.LatLng

data class GameState(val pet: Pet, val enemyLocations: List<LatLng>, val itemLocations: List<LatLng>)