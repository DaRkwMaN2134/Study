namespace File_IO
{
    public class FilesWork
    {
        string file = "sales.csv";
        string[] example = [ "Дата; Товар; Количество; Цена",
                             "2026-08-01; Ноутбук; 2; 50000",
                             "2026-08-02; Мышь; 5; 1500",
                             "2026-08-03; Ноутбук; 1; 52000"];
        public async Task<string[]> BasicFileIO()
        {
            try
            {
                string[] fileRead = await File.ReadAllLinesAsync(file);
                return fileRead;
            }
            catch (FileNotFoundException)
            {
                File.Create("sales.csv").Close();
                await File.WriteAllLinesAsync(file, example);
                Console.WriteLine($"Файл не был найден\nПоэтому был создан пример файла по пути '{Path.GetFullPath(file)}'");
                return null;
            }
        }

        public async Task ReportFileIO(string file, List<string> report)
        {
            await File.WriteAllLinesAsync(file, report);
        }

        public async Task ErrorFileIO(string file, List<string> report)
        {
            await File.WriteAllLinesAsync(file, report);
        }
    }
}
