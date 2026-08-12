using System.Text.Json;

namespace Shape_Calculator
{
    public class JsonShapeSerializer: IShapeSerializer
    {
        public async Task SaveAsync(string path, IEnumerable<Shape> shapes)
        {
            using FileStream createStream = File.Create(path);
            await JsonSerializer.SerializeAsync(createStream, shapes);
        }

        public async Task<IEnumerable<Shape>> LoadAsync(string path)
        {
            if (!File.Exists(path))
            {
                await Logger.Instance.LogAsync($"Файл {path} не найден");
                return Enumerable.Empty<Shape>();
            }

            try
            {
                using FileStream openStream = File.OpenRead(path);
                var shapes = await JsonSerializer.DeserializeAsync<List<Shape>>(openStream);
                return shapes ?? new List<Shape>();
            }
            catch (JsonException ex)
            {
                await Logger.Instance.LogErrorAsync($"Ошибка десериализации: {ex.Message}");
                return Enumerable.Empty<Shape>();
            }
        }
    }

    interface IShapeSerializer
    {
        Task SaveAsync(string path, IEnumerable<Shape> shapes);
        Task<IEnumerable<Shape>> LoadAsync(string path);
    }
}
