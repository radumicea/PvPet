package codebusters.pvpet.ui.fragments

import android.content.pm.PackageManager
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import codebusters.pvpet.R
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.databinding.FragmentHomeBinding

class HomeFragment : Fragment() {
    private lateinit var binding: FragmentHomeBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        super.onCreateView(inflater, container, savedInstanceState)
        binding = FragmentHomeBinding.inflate(layoutInflater)

        return binding.root
    }

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