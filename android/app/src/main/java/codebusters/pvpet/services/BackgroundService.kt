package codebusters.pvpet.services

import android.app.Service
import android.content.Intent
import android.content.pm.ServiceInfo
import android.os.IBinder
import androidx.core.app.NotificationCompat
import androidx.core.app.ServiceCompat
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.data.GameData
import codebusters.pvpet.data_access.GameLoopDataAccessor
import codebusters.pvpet.data_access.UserDataAccessor
import codebusters.pvpet.models.Pet
import codebusters.pvpet.models.User
import codebusters.pvpet.providers.LocationProvider
import com.google.android.gms.maps.model.LatLng
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.flow.launchIn
import kotlinx.coroutines.flow.onEach
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.withContext

class BackgroundService : Service() {
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
                    val gameState = GameLoopDataAccessor.updatePetState(
                        Pet(
                            location = LatLng(
                                location.latitude,
                                location.longitude
                            )
                        )
                    )

                    GameData.updateGameState(gameState)
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