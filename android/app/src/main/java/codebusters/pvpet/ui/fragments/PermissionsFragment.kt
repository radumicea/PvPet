package codebusters.pvpet.ui.fragments

import android.app.AlertDialog
import android.content.pm.PackageManager
import android.os.Bundle
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.lifecycle.withStarted
import androidx.navigation.NavController
import androidx.navigation.fragment.findNavController
import codebusters.pvpet.R
import codebusters.pvpet.constants.Constants
import kotlinx.coroutines.launch

class PermissionsFragment : Fragment() {
    companion object {
        private var toast: Toast? = null
    }

    private lateinit var navController: NavController

    private val requestPermissionLauncher =
        registerForActivityResult(
            ActivityResultContracts.RequestMultiplePermissions()
        ) { permissions ->
            if (Constants.PERMISSIONS_REQUIRED.all {
                    permissions[it] == true
                }) {
                navigateHome()
            } else {
                toast?.cancel()
                toast = Toast.makeText(
                    requireContext(),
                    "Please allow access to precise location.",
                    Toast.LENGTH_LONG
                )
                toast!!.show()
            }
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        navController = findNavController()
        askForPermission()
    }

    private fun askForPermission() {
        if (Constants.PERMISSIONS_REQUIRED.all {
                ContextCompat.checkSelfPermission(
                    requireContext(),
                    it
                ) == PackageManager.PERMISSION_GRANTED
            }) {
            navigateHome()
        } else if (Constants.PERMISSIONS_REQUIRED.any {
                shouldShowRequestPermissionRationale(
                    it
                )
            }) {
            AlertDialog.Builder(requireContext()).setMessage(
                "This application needs access to precise location in order to function.\n\n" +
                        "Please accept this permission.\n\n" +
                        "This can be done through the application, or through the phone's settings."
            ).setPositiveButton(
                "Sure"
            ) { _, _ ->
                requestPermissionLauncher.launch(Constants.PERMISSIONS_REQUIRED)
            }.setNegativeButton(
                "No, thanks"
            ) { _, _ -> }.create().show()
        } else {
            requestPermissionLauncher.launch(Constants.PERMISSIONS_REQUIRED)
        }
    }

    private fun navigateHome() {
        lifecycleScope.launch {
            withStarted {
                navController.navigate(R.id.home_fragment)
            }
        }
    }
}