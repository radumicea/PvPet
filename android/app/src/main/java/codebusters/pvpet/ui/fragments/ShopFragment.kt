package codebusters.pvpet.ui.fragments

import android.content.pm.PackageManager
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import codebusters.pvpet.R
import codebusters.pvpet.constants.Constants

class ShopFragment : Fragment() {

    override fun onResume() {
        super.onResume()
        // Make sure that all permissions are still present, since the
        // user could have removed them while the app was in paused state.
        if (Constants.PERMISSIONS_REQUIRED.any {
                ContextCompat.checkSelfPermission(
                    requireContext(),
                    it
                ) != PackageManager.PERMISSION_GRANTED
            }) {
            findNavController().navigate(R.id.permissions_fragment)
        }
    }
}