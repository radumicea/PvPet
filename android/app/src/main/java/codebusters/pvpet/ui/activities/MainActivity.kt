package codebusters.pvpet.ui.activities

import android.app.AlertDialog
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import codebusters.pvpet.providers.LocationProvider
import codebusters.pvpet.services.LocationService
import codebusters.pvpet.ui.composables.Button
import codebusters.pvpet.ui.theme.PvPetTheme

class MainActivity : ComponentActivity() {
    private val requestPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestMultiplePermissions(),
    ) { permissions ->
        permissions.entries.forEach { permission ->
            if (permission.key == android.Manifest.permission.ACCESS_FINE_LOCATION) {
                if (permission.value) {
                    initProviderAndStartService()
                    return@registerForActivityResult
                }
            }
        }

        Toast.makeText(
            this, "Please allow access to precise location.", Toast.LENGTH_LONG
        ).show()
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            PvPetTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Button(
                        text = "TEST",
                        onClick = { askForPermission() },
                        width = 300.dp,
                        height = 100.dp
                    )
                }
            }
        }
    }

    private fun askForPermission() {
        if (checkSelfPermission(android.Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED &&
            checkSelfPermission(android.Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED
        ) {
            initProviderAndStartService()
        } else if (shouldShowRequestPermissionRationale(android.Manifest.permission.ACCESS_COARSE_LOCATION) ||
            shouldShowRequestPermissionRationale(android.Manifest.permission.ACCESS_FINE_LOCATION)
        ) {
            AlertDialog.Builder(this).setMessage(
                "This application needs access to precise location in order to function.\n\n" +
                        "Please accept this permission.\n\n" +
                        "This can be done through the application, or through the phone's settings."
            ).setPositiveButton(
                "Sure"
            ) { _, _ ->
                requestPermissionLauncher.launch(
                    arrayOf(
                        android.Manifest.permission.ACCESS_FINE_LOCATION,
                        android.Manifest.permission.ACCESS_COARSE_LOCATION
                    )
                )
            }.setNegativeButton(
                "No, thanks"
            ) { _, _ -> }.create().show()

        } else {
            requestPermissionLauncher.launch(
                arrayOf(
                    android.Manifest.permission.ACCESS_FINE_LOCATION,
                    android.Manifest.permission.ACCESS_COARSE_LOCATION
                )
            )
        }
    }

    private fun initProviderAndStartService() {
        // init location provider
        LocationProvider.init(this)

        // start location service
        val serviceIntent = Intent(this, LocationService::class.java)
        startForegroundService(serviceIntent)
    }
}