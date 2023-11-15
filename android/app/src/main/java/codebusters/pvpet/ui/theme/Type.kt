package codebusters.pvpet.ui.theme

import androidx.compose.material3.Typography
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.Font
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.unit.sp
import codebusters.pvpet.R

private val font = FontFamily(Font(R.font.robotomono))
private val primaryColor = Color(0xFF232323)
private val secondaryColor = Color(0xFF5A5A5A)

val Typography = Typography(
    titleMedium = TextStyle(
        fontFamily = font,
        fontSize = 24.sp,
        lineHeight = 36.sp,
        color = primaryColor
    ),
    bodyMedium = TextStyle(
        fontFamily = font,
        fontSize = 16.sp,
        lineHeight = 24.sp,
        color = primaryColor
    ),
    bodySmall = TextStyle(
        fontFamily = font,
        fontSize = 14.sp,
        lineHeight = 21.sp,
        color = primaryColor
    ),
    labelMedium = TextStyle(
        fontFamily = font,
        fontSize = 12.sp,
        lineHeight = 18.sp,
        color = secondaryColor
    ),
)