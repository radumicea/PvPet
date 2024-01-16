package codebusters.pvpet.ui.fragments

import android.content.Context
import android.content.pm.PackageManager
import android.graphics.drawable.Drawable
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.ImageView
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import codebusters.pvpet.R
import codebusters.pvpet.constants.Constants
import codebusters.pvpet.data.GameData
import codebusters.pvpet.data_access.UserDataAccessor
import codebusters.pvpet.databinding.FragmentShopBinding
import codebusters.pvpet.models.ShopItem
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.io.IOException

class ShopFragment : Fragment() {


    private lateinit var binding: FragmentShopBinding

    private lateinit var itemAdapter: CustomItemAdapter

    override fun onResume() {
        super.onResume()
        // Make sure that all permissions are still present, since the
        // user could have removed them while the app was in paused state.
        if (Constants.PERMISSIONS_REQUIRED.any {
                ContextCompat.checkSelfPermission(
                    requireContext(),
                    it
                ) != PackageManager.PERMISSION_GRANTED
            }) {
            findNavController().navigate(R.id.permissions_fragment)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentShopBinding.inflate(inflater, container, false)

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)


        val from = arrayOf("image", "text")
        val to = intArrayOf(R.id.imageView, R.id.text1)

        itemAdapter = CustomItemAdapter(requireContext(), emptyList())

        binding.itemListView.adapter = itemAdapter


        // Observe the LiveData and update the adapter when the data changes
        GameData.petState.observe(viewLifecycleOwner) { pet ->

            GameData.petState.observe(viewLifecycleOwner) { pet ->

                val adapter = CustomItemAdapter(requireContext(), pet.shopItems ?: emptyList())
                binding.itemListView.adapter = adapter
                binding.text.text = pet.gold.toString() + " Gold left"

                adapter.setOnItemClickListener(object : OnItemClickListener {
                    override fun onItemClick(position: Int) {
                        // Handle item click here
                        val clickedItem = pet.shopItems?.get(position)
                        if (clickedItem?.price!! < pet.gold!!)
                        {
                            pet.gold = pet.gold!! - clickedItem?.price!!
                            val updatedList = pet.shopItems?.filter { it != clickedItem }

                            // Update the original list with the new one
                            pet.shopItems = updatedList as? MutableList<ShopItem>
                            lifecycleScope.launch {
                                try {
                                    val result = withContext(Dispatchers.IO) {
                                        UserDataAccessor.buyItem(clickedItem.id)
                                    }
                                    } catch (e: Exception) {
                                        e
                                    }

                            }
                            // Notify the adapter that the data has changed
                            adapter.notifyDataSetChanged()
                        }

                    }
                })

            }
        }
    }
}

interface OnItemClickListener {
    fun onItemClick(position: Int)
}
class CustomItemAdapter(private val context: Context, private val items: List<ShopItem>) : BaseAdapter() {

    private var onItemClickListener: OnItemClickListener? = null
    override fun getCount(): Int = items.size

    override fun getItem(position: Int): Any = items[position]

    override fun getItemId(position: Int): Long = position.toLong()

    fun setOnItemClickListener(listener: OnItemClickListener) {
        this.onItemClickListener = listener
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val view = convertView ?: LayoutInflater.from(context).inflate(R.layout.item_list, parent, false)

        val item = getItem(position) as ShopItem

        val imageView: ImageView = view.findViewById(R.id.imageView)
        // Load the image dynamically based on the item's properties
        val name = "${item.type?.toLowerCase()}_${item.pictocode}.png"

        val drawable = try {
            Drawable.createFromStream(context.assets.open(name), null)
        } catch (e: IOException) {
            // Handle the exception (e.g., log the error)
            null
        }

        imageView.setImageDrawable(drawable)

        val textView: TextView = view.findViewById(R.id.text1)
        textView.text = "${item.price}"

        view.setOnClickListener {
            onItemClickListener?.onItemClick(position)
        }

        return view
    }
}

