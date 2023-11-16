package codebusters.pvpet.services

import android.app.Service
import android.content.Intent
import android.content.pm.ServiceInfo
import android.os.IBinder
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
                try {
                    val enemyLocations = GameDataAccessor.updatePetLocation(
                        Pet(
                            // TO DO
                            id = "3B5625EB-CC25-4F97-A3FB-8BBFB76BCD14",
                            latitude = location.latitude,
                            longitude = location.longitude
                        )
                    )

                    LocationProvider.enemyLocations.postValue(enemyLocations)
                } catch (e: Exception) {
                    e.printStackTrace()
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