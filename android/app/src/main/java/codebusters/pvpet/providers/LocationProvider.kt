package codebusters.pvpet.providers

import android.content.Context
import android.location.Location
import android.os.Looper
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationCallback
import com.google.android.gms.location.LocationRequest
import com.google.android.gms.location.LocationResult
import com.google.android.gms.location.LocationServices
import kotlinx.coroutines.channels.awaitClose
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.callbackFlow
import kotlinx.coroutines.launch

object LocationProvider {
    private var _fusedLocationClient: FusedLocationProviderClient? = null
    private val fusedLocationClient: FusedLocationProviderClient
        get() = _fusedLocationClient!!

    fun init(context: Context) {
        if (_fusedLocationClient == null) {
            _fusedLocationClient = LocationServices.getFusedLocationProviderClient(context)
        }
    }

    fun onLastLocationReceived(callback: (location: Location) -> Unit) {
        fusedLocationClient.lastLocation.addOnSuccessListener(callback)
    }

    fun getLocationUpdates(interval: Long): Flow<Location> {
        return callbackFlow {
            val request = LocationRequest.Builder(interval).build()

            val callback = object : LocationCallback() {
                override fun onLocationResult(result: LocationResult) {
                    super.onLocationResult(result)
                    result.locations.lastOrNull()?.let { location ->
                        launch { send(location) }
                    }
                }
            }

            fusedLocationClient.requestLocationUpdates(
                request,
                callback,
                Looper.getMainLooper()
            )

            awaitClose {
                fusedLocationClient.removeLocationUpdates(callback)
            }
        }
    }
}