package codebusters.pvpet.data_access

import codebusters.pvpet.BuildConfig
import codebusters.pvpet.models.User
import codebusters.pvpet.providers.HttpClient
import com.google.gson.reflect.TypeToken

object UserDataAccessor {
    fun login(user: User) {
        HttpClient.post("${BuildConfig.API_URI}/User/login", user, object : TypeToken<Any>() {})
    }
}