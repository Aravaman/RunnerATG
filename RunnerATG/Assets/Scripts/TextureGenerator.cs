using UnityEngine;

public static class TextureGenerator
{
	// Генерация текстуры на основе массива плиток
	public static Texture2D GetTexture(int width, int height, Tile[,] tiles)
	{
		// Создание новой текстуры с указанными размерами
		var texture = new Texture2D(width, height);
		// Создание массива цветов для всех пикселей текстуры
		var pixels = new Color[width * height];

		// Обход каждой плитки в массиве
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				// Получение значения высоты плитки
				float value = tiles[x, y].HeightValue;
				// Установка цвета пикселя на основе значения высоты плитки (0 = черный, 1 = белый)
				pixels[x + y * width] = Color.Lerp(Color.black, Color.white, value);
            }
		}

		// Применение массива цветов к текстуре
		texture.SetPixels(pixels);
		// Установка режима обертывания текстуры на "Clamp" (ограничение)
		texture.wrapMode = TextureWrapMode.Clamp;
		// Применение изменений к текстуре
		texture.Apply();
		// Возврат сгенерированной текстуры
		return texture;
	}

}
