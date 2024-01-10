package codebusters.pvpet.ui.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import codebusters.pvpet.R
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
}