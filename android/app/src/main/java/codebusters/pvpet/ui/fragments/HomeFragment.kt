package codebusters.pvpet.ui.fragments

import android.annotation.SuppressLint
import android.content.pm.PackageManager
import android.graphics.drawable.Drawable
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import codebusters.pvpet.R
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.data.GameData
import codebusters.pvpet.databinding.FragmentHomeBinding


class HomeFragment : Fragment() {
    private lateinit var binding: FragmentHomeBinding

    @SuppressLint("SetTextI18n")
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        super.onCreateView(inflater, container, savedInstanceState)
        binding = FragmentHomeBinding.inflate(layoutInflater)

        GameData.petState.observe(viewLifecycleOwner) { pet ->
            binding.petName.text = pet.name
            binding.petHp.text = "${pet.hp} / 100"
            binding.petFood.text = "${pet.food} / 100"
            val inS = requireContext().assets.open("pet_${pet.variant}.png")
            val d = Drawable.createFromStream(inS, null)
            binding.petImage.setImageDrawable(d)
            inS.close()
        }

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