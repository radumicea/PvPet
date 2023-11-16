package codebusters.pvpet.providers

import com.google.gson.Gson
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

    fun post(url: String, payload: Any): String? {
        val json = Gson().toJson(payload)

        val body = json.toRequestBody(JsonMediaType)
        val request = Request.Builder().url(url).post(body).build()

        return try {
            val response = client.newCall(request).execute()
            return response.body?.string()
        } catch (e: Exception) {
            null
        }
    }

    fun patch(url: String, payload: Any): String? {
        val json = Gson().toJson(payload)

        val body = json.toRequestBody(JsonMediaType)
        val request = Request.Builder().url(url).patch(body).build()

        return try {
            val response = client.newCall(request).execute()
            return response.body?.string()
        } catch (e: Exception) {
            null
        }
    }
}