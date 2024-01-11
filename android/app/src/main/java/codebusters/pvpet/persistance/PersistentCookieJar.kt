package codebusters.pvpet.persistance

import codebusters.pvpet.App
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.Cookie

object PersistentCookieJar {
    private const val FILE = "codebusters.pvpet.PREFERENCE_FILE_KEY"

    private var cookiesJson = ""
        get() {
            if (field != "") return field

            val pref = App.getSharedPreferences(FILE)
            field = pref.getString("cookie", "")!!

            return field
        }
        set(value) {
            field = value

            val pref = App.getSharedPreferences(FILE)
            with(pref.edit()) {
                putString("cookie", value)
                apply()
            }
        }

    fun loadForRequest(): List<Cookie> {
        if (cookiesJson == "")
            return listOf()

        return Gson().fromJson(cookiesJson, object : TypeToken<List<Cookie>>() {}.type)
    }

    fun saveFromResponse(cookies: List<Cookie>) {
        cookiesJson = Gson().toJson(cookies)
    }
}