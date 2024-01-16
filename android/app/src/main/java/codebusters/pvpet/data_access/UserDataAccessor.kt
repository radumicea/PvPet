package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.User
import codebusters.pvpet.providers.HttpClient
import com.google.gson.reflect.TypeToken
import okhttp3.internal.notify

data class buyITem(val id : String?)
object UserDataAccessor {
    fun login(user: User, onSuccess: (Any) -> Unit, onError: (Exception) -> Unit) {
        HttpClient.post("${BuildConfig.API_URI}/User/login", user, object : TypeToken<Any>() {
            fun onSuccess(response: Any) {
                onSuccess.invoke(response)
            }

            fun onError(error: Exception) {
                onError.invoke(error)
            }
        })?.notify()
    }


    fun register(user: User, onSuccess: (Any) -> Unit, onError: (Exception) -> Unit) {
        HttpClient.post("${BuildConfig.API_URI}/User", user, object : TypeToken<Any>() {
            fun onSuccess(response: Any) {
                onSuccess.invoke(response)
            }

            fun onError(error: Exception) {
                onError.invoke(error)
            }
        })?.notify()
    }

    fun buyItem(itemId: String?) {
        if (itemId != null) {
            HttpClient.post("${BuildConfig.API_URI}/Pet/Buy", itemId, object : TypeToken<Any>() {

            })
        }
    }
}