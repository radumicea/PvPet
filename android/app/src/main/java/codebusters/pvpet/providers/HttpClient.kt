package codebusters.pvpet.providers

import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import java.util.concurrent.TimeUnit

object HttpClient {
    private val JsonMediaType = "application/json".toMediaType()

    private val client: OkHttpClient = OkHttpClient.Builder()
        .callTimeout(2, TimeUnit.SECONDS)
        .connectTimeout(2, TimeUnit.SECONDS)
        .readTimeout(2, TimeUnit.SECONDS)
        .writeTimeout(2, TimeUnit.SECONDS)
        .build()

    fun <TResponse> post(url: String, payload: Any): TResponse? {
        val json = Gson().toJson(payload)

        val body = json.toRequestBody(JsonMediaType)
        val request = Request.Builder().url(url).post(body).build()

        return try {
            val response = client.newCall(request).execute()
            val responseJson = response.body?.string()
            Gson().fromJson(responseJson, object : TypeToken<TResponse>() {}.type)
        } catch (e: Exception) {
            null
        }
    }

    fun <TResponse> patch(url: String, payload: Any): TResponse? {
        val json = Gson().toJson(payload)

        val body = json.toRequestBody(JsonMediaType)
        val request = Request.Builder().url(url).patch(body).build()

        return try {
            val response = client.newCall(request).execute()
            val responseJson = response.body?.string()
            Gson().fromJson(responseJson, object : TypeToken<TResponse>() {}.type)
        } catch (e: Exception) {
            null
        }
    }
}