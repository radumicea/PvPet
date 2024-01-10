package codebusters.pvpet

import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import androidx.core.content.ContextCompat.getSystemService
import androidx.startup.Initializer
import codebusters.pvpet.constants.Constants

@Suppress("unused")
class AppInitializer : Initializer<Unit> {
    override fun create(context: Context) {
        // init notificationChannel
        val channel = NotificationChannel(
            Constants.NOTIFICATION_CHANNEL_ID,
            "PvPet Notification Channel",
                NotificationManager.IMPORTANCE_DEFAULT
        )
        channel.description = "Notification Channel for PvPet"
        val notificationManager = getSystemService(context, NotificationManager::class.java)
        notificationManager!!.createNotificationChannel(channel)
    }

    override fun dependencies(): List<Class<out Initializer<*>>> = emptyList()
}