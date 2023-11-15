package codebusters.pvpet.ui.composables

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.requiredHeight
import androidx.compose.foundation.layout.requiredWidth
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.RectangleShape
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import codebusters.pvpet.ui.coloredShadow

@Composable
fun Button(text: String, onClick: () -> Unit, width: Dp, height: Dp) {
    TextButton(
        onClick = onClick,
        modifier = Modifier
            .requiredWidth(width)
            .requiredHeight(height)
            .coloredShadow(
                color = Color.Black,
                alpha = 0.8f,
                shadowRadius = 0.0001.dp,
                offsetX = 3.5.dp,
                offsetY = 3.5.dp
            ),
        shape = RectangleShape,
        colors = ButtonDefaults.buttonColors((Color(0xFFF1CAD6))),
        border = BorderStroke(1.dp, Color.Black),
    ) {
        Text(text = text, style = MaterialTheme.typography.bodySmall, fontWeight = FontWeight.Bold)
    }
}
