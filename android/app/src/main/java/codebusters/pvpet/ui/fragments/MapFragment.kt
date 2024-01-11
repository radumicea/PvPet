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
import codebusters.pvpet.providers.LocationProvider
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions

@SuppressWarnings("MissingPermission")
class MapFragment : Fragment() {
    private lateinit var map: GoogleMap

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        super.onCreateView(inflater, container, savedInstanceState)
        val view = inflater.inflate(R.layout.fragment_map, container, false)

        if (Constants.PERMISSIONS_REQUIRED.any {
                ContextCompat.checkSelfPermission(
                    requireContext(),
                    it
                ) != PackageManager.PERMISSION_GRANTED
            }) {
            return view
        }

        val mapFragment =
            childFragmentManager.findFragmentById(R.id.google_map) as SupportMapFragment
        mapFragment.getMapAsync { googleMap ->
            map = googleMap

            googleMap.isMyLocationEnabled = true

            LocationProvider.onLastLocationReceived { location ->
                googleMap.animateCamera(
                    CameraUpdateFactory.newLatLngZoom(
                        LatLng(
                            location.latitude,
                            location.longitude
                        ), 17f
                    )
                )
            }
        }

        LocationProvider.enemyLocations.observe(viewLifecycleOwner) { locations ->
            if (!::map.isInitialized) {
                return@observe
            }

            map.clear()
            locations.forEach { location ->
                map.addMarker(MarkerOptions().position(location))
            }
        }

        return view
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