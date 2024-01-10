package codebusters.pvpet.ui.activities

import android.content.pm.PackageManager
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.WindowCompat
import androidx.core.view.WindowInsetsCompat
import androidx.core.view.WindowInsetsControllerCompat
import androidx.navigation.NavController
import androidx.navigation.fragment.NavHostFragment
import androidx.navigation.ui.setupWithNavController
import codebusters.pvpet.R
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {
    private lateinit var activityMainBinding: ActivityMainBinding
    private lateinit var navController: NavController

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        activityMainBinding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(activityMainBinding.root)

        val navHostFragment =
            supportFragmentManager.findFragmentById(R.id.fragment_container) as NavHostFragment
        navController = navHostFragment.navController
        activityMainBinding.navigation.setupWithNavController(navController)
        activityMainBinding.navigation.setOnItemReselectedListener {
            // ignore the reselection
        }
        activityMainBinding.navigation.setOnItemSelectedListener { menuItem ->
            if (Constants.PERMISSIONS_REQUIRED.any {
                    checkSelfPermission(it) != PackageManager.PERMISSION_GRANTED
                }) {
                navController.navigate(R.id.permissions_fragment)
                return@setOnItemSelectedListener false
            }

            when (menuItem.itemId) {
                R.id.menu_item_home -> {
                    navController.navigate(R.id.home_fragment)
                    true
                }

                R.id.menu_item_map -> {
                    navController.navigate(R.id.map_fragment)
                    true
                }

                R.id.menu_item_shop -> {
                    navController.navigate(R.id.shop_fragment)
                    true
                }

                R.id.menu_item_notifications -> {
                    navController.navigate(R.id.notifications_fragment)
                    true
                }

                else -> false
            }
        }

        WindowCompat.setDecorFitsSystemWindows(window, false)
        WindowInsetsControllerCompat(window, window.decorView).let { controller ->
            controller.hide(WindowInsetsCompat.Type.systemBars())
            controller.systemBarsBehavior =
                WindowInsetsControllerCompat.BEHAVIOR_SHOW_TRANSIENT_BARS_BY_SWIPE
        }
    }

    override fun onStart() {
        super.onStart()
        // Make sure that all permissions are still present, since the
        // user could have removed them while the app was in paused state.
        if (Constants.PERMISSIONS_REQUIRED.any {
                checkSelfPermission(it) != PackageManager.PERMISSION_GRANTED
            }) {
            navController.navigate(R.id.permissions_fragment)
        }
    }
}