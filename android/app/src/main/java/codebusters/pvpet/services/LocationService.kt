package codebusters.pvpet.services

import android.app.Service
import android.content.Intent
import android.content.pm.ServiceInfo
import android.os.IBinder
import android.util.Log
import androidx.core.app.NotificationCompat
import androidx.core.app.ServiceCompat
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.data_access.GameDataAccessor
import codebusters.pvpet.models.Pet
import codebusters.pvpet.providers.LocationProvider
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.flow.launchIn
import kotlinx.coroutines.flow.onEach

class LocationService : Service() {
    private val serviceScope = CoroutineScope(SupervisorJob() + Dispatchers.IO)

    override fun onBind(intent: Intent?): IBinder? {
        return null
    }

    override fun onCreate() {
        val notification = NotificationCompat.Builder(this, Constants.NOTIFICATION_CHANNEL_ID)
            .setPriority(NotificationCompat.PRIORITY_LOW)
            .build()

        LocationProvider.getLocationUpdates(Constants.TICK_IN_MS)
            .onEach { location ->
                val enemyLocations = GameDataAccessor.updatePetLocation(
                    Pet(
                        id = "1F6DBB97-B39A-46AA-A66D-15B5894675DD",
                        latitude = location.latitude,
                        longitude = location.longitude
                    )
                )

                if (enemyLocations != null) {
                    Log.i("i", enemyLocations.toString())
                } else {
                    Log.e("e", "Error requesting")
                }
            }
            .launchIn(serviceScope)

        ServiceCompat.startForeground(
            this,
            (System.currentTimeMillis() % 1_000_000_000).toInt(),
            notification,
            ServiceInfo.FOREGROUND_SERVICE_TYPE_LOCATION
        )
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        return START_STICKY
    }
}