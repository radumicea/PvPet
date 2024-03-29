package codebusters.pvpet.providers

import codebusters.pvpet.persistance.PersistentCookieJar
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.Cookie
import okhttp3.CookieJar
import okhttp3.HttpUrl
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import java.util.concurrent.TimeUnit

object HttpClient {
    private val JsonMediaType = "application/json".toMediaType()

    private val client: OkHttpClient = OkHttpClient.Builder()
        .callTimeout(5, TimeUnit.SECONDS)
        .connectTimeout(5, TimeUnit.SECONDS)
        .readTimeout(5, TimeUnit.SECONDS)
        .writeTimeout(5, TimeUnit.SECONDS)
        .cookieJar(object : CookieJar {
            override fun loadForRequest(url: HttpUrl): List<Cookie> {
                return PersistentCookieJar.loadForRequest()
            }

            override fun saveFromResponse(url: HttpUrl, cookies: List<Cookie>) {
                PersistentCookieJar.saveFromResponse(cookies)
            }
        })
        .build()

    fun <T> post(
        url: String,
        payload: Any,
        typeToken: TypeToken<T>
    ): T {
        return doRequest(url, HttpMethod.POST, payload, typeToken)
    }



    fun <T> patch(
        url: String,
        payload: Any,
        typeToken: TypeToken<T>
    ): T {
        return doRequest(url, HttpMethod.PATCH, payload, typeToken)
    }

    private fun <T> doRequest(
        url: String,
        httpMethod: HttpMethod,
        payload: Any? = null,
        typeToken: TypeToken<T>
    ): T {
        val body = if (payload != null) {
            val json = Gson().toJson(payload)
            json.toRequestBody(JsonMediaType)
        } else {
            "".toRequestBody()
        }

        var requestBuilder = Request.Builder().url(url)
        requestBuilder = when (httpMethod) {
            HttpMethod.GET -> requestBuilder.get()
            HttpMethod.POST -> requestBuilder.post(body)
            HttpMethod.PATCH -> requestBuilder.patch(body)
            HttpMethod.DELETE -> requestBuilder.delete(body)
        }
        val request = requestBuilder.build()

        val response = client.newCall(request).execute()
        if (!response.isSuccessful) {
            throw Exception(response.message)
        }

        val responseJson = response.body?.string()

        return Gson().fromJson(responseJson, typeToken.type)
    }

    private enum class HttpMethod {
        GET, POST, PATCH, DELETE
    }
}