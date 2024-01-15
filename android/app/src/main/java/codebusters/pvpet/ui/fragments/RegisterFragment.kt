package codebusters.pvpet.ui.fragments

import android.os.Bundle
import android.os.Handler
import android.os.Looper
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import codebusters.pvpet.R
import codebusters.pvpet.data_access.UserDataAccessor
import codebusters.pvpet.models.User
import codebusters.pvpet.R.layout.fragment_register
import codebusters.pvpet.databinding.FragmentRegisterBinding
import androidx.navigation.fragment.findNavController
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class RegisterFragment : Fragment() {
    private lateinit var binding: FragmentRegisterBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentRegisterBinding.inflate(inflater, container, false)

        binding.registerButton.setOnClickListener {
            // Get user input
            val username = binding.usernameEditText.text.toString()
            val password = binding.passwordEditText.text.toString()

            // Create a User object

            val user = User(username = username, password = password, firebaseToken = null, pet = null)

            lifecycleScope.launch {
                try {
                    val user = User(username = username, password = password, firebaseToken = null, pet = null)

                    val result = withContext(Dispatchers.IO) {
                        UserDataAccessor.register(
                            user,
                            onSuccess = { response -> showToast("Register successful"); Thread.sleep(2000); findNavController().navigate(R.id.login_fragment) },
                            onError = { res -> showToast("Error in register: ${res.message}") })
                    }


                } catch (e: Exception) {
                    withContext(Dispatchers.Main) {
                        showToast("Error in register: ${e.message}")
                    }
                }
            }

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