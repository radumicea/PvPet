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


    fun post(
        url: String,
        payload: Any,
        jsonType: JsonType,
        clazz: Class<*>? = null
    ): Any {
        return doRequest(url, HttpMethod.POST, payload, jsonType, clazz)
    }

    fun patch(
        url: String,
        payload: Any,
        jsonType: JsonType,
        clazz: Class<*>? = null
    ): Any {
        return doRequest(url, HttpMethod.PATCH, payload, jsonType, clazz)
    }

    private fun doRequest(
        url: String,
        httpMethod: HttpMethod,
        payload: Any? = null,
        jsonType: JsonType,
        clazz: Class<*>? = null
    ): Any {
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

        return when (jsonType) {
            JsonType.NONE -> Unit

            JsonType.OBJECT -> {
                val responseJson = response.body?.string()
                Gson().fromJson(responseJson, clazz)
            }

            JsonType.COLLECTION -> {
                val responseJson = response.body?.string()
                Gson().fromJson(
                    responseJson,
                    TypeToken.getParameterized(List::class.java, clazz).type
                )
            }
        }
    }

    enum class JsonType {
        NONE, OBJECT, COLLECTION
    }

    private enum class HttpMethod {
        GET, POST, PATCH, DELETE
    }
}