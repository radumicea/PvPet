package codebusters.pvpet.constants

import android.Manifest

object Constants {
    val PERMISSIONS_REQUIRED = arrayOf(
        Manifest.permission.ACCESS_FINE_LOCATION,
        Manifest.permission.ACCESS_COARSE_LOCATION
    )
    const val NOTIFICATION_CHANNEL_ID = "PvPetChannel"
    const val TICK_IN_MS = 5_000L
}