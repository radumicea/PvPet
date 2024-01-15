package codebusters.pvpet.ui.fragments

import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.content.ContentProviderCompat.requireContext
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import codebusters.pvpet.R
import codebusters.pvpet.data_access.UserDataAccessor
import codebusters.pvpet.databinding.FragmentLoginBinding
import codebusters.pvpet.databinding.FragmentRegisterBinding
import codebusters.pvpet.models.User
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class LoginFragment : Fragment() {
    private lateinit var binding: FragmentLoginBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

    }
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentLoginBinding.inflate(inflater, container, false)

        binding.loginButton.setOnClickListener {
            // Get user input
            val username = binding.usernameEditText.text.toString()
            val password = binding.passwordEditText.text.toString()

            // Create a User object
            val user = User(username = username, password = password, firebaseToken = null, pet = null)

            lifecycleScope.launch {
                try {
                    val user = User(username = username, password = password, firebaseToken = null, pet = null)

                    val result = withContext(Dispatchers.IO) {
                        UserDataAccessor.login(
                            user,
                            onSuccess = { response -> showToast("Login successful");Thread.sleep(2000); findNavController().navigate(R.id.home_fragment)},
                            onError = { res -> showToast("Error in login: ${res.message}") })
                    }


                } catch (e: Exception) {
                    withContext(Dispatchers.Main) {
                        showToast("Error in login: ${e.message}")
                    }
                }
            }

        }

        binding.registerTextView.setOnClickListener(){
            findNavController().navigate(R.id.register_fragment)
        }
        return binding.root
    }

    private fun showToast(message: String) {
        Handler(Looper.getMainLooper()).post {
            context?.let {
                Toast.makeText(it, message, Toast.LENGTH_SHORT).show()
            }
        }
    }
}
