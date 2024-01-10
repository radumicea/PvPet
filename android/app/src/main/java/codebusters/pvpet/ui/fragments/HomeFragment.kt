package codebusters.pvpet.ui.fragments

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat.startForegroundService
import androidx.fragment.app.Fragment
import codebusters.pvpet.databinding.FragmentHomeBinding
import codebusters.pvpet.providers.LocationProvider
import codebusters.pvpet.services.LocationService

class HomeFragment : Fragment() {
    private lateinit var binding: FragmentHomeBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // init location provider
        LocationProvider.init(requireContext())

        // start location service
        val serviceIntent = Intent(requireContext(), LocationService::class.java)
        startForegroundService(requireContext(), serviceIntent)
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        super.onCreateView(inflater, container, savedInstanceState)
        binding = FragmentHomeBinding.inflate(layoutInflater)

        return binding.root
    }
}