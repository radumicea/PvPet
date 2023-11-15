package codebusters.pvpet

import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.content.Intent
import androidx.core.content.ContextCompat.getSystemService
import androidx.startup.Initializer
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.providers.LocationProvider
import codebusters.pvpet.services.LocationService

class AppInitializer : Initializer<Unit> {
    override fun create(context: Context) {
        // init location provider
        LocationProvider.init(context)

        // init notification channel
        val channel = NotificationChannel(
            Constants.NOTIFICATION_CHANNEL_ID,
            "PvPet Notification Channel",
            NotificationManager.IMPORTANCE_LOW
        )
        channel.description = "Notification Channel for PvPet"
        val notificationManager = getSystemService(context, NotificationManager::class.java)
        notificationManager!!.createNotificationChannel(channel)

        // start location service
        val serviceIntent = Intent(context, LocationService::class.java)
        context.startForegroundService(serviceIntent)
    }

    override fun dependencies(): List<Class<out Initializer<*>>> = emptyList()
}